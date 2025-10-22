using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using FinTechBanking.Core.Entities;
using FinTechBanking.Core.Interfaces;

namespace FinTechBanking.Services.Webhooks;

public class WebhookService : IWebhookService
{
    private readonly IUserRepository _userRepository;
    private readonly IWebhookLogRepository _webhookLogRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILogger<WebhookService> _logger;
    private readonly HttpClient _httpClient;

    public WebhookService(
        IUserRepository userRepository,
        IWebhookLogRepository webhookLogRepository,
        ITransactionRepository transactionRepository,
        ILogger<WebhookService> logger,
        HttpClient httpClient)
    {
        _userRepository = userRepository;
        _webhookLogRepository = webhookLogRepository;
        _transactionRepository = transactionRepository;
        _logger = logger;
        _httpClient = httpClient;
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
    }

    public async Task<bool> RegisterWebhookAsync(Guid userId, string webhookUrl)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            user.WebhookUrl = webhookUrl;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            _logger.LogInformation($"Webhook registrado para usuário {userId}: {webhookUrl}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao registrar webhook para usuário {userId}");
            return false;
        }
    }

    public async Task<bool> UnregisterWebhookAsync(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            user.WebhookUrl = null;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            _logger.LogInformation($"Webhook removido para usuário {userId}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao remover webhook para usuário {userId}");
            return false;
        }
    }

    public async Task<string?> GetWebhookUrlAsync(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user?.WebhookUrl;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao obter webhook para usuário {userId}");
            return null;
        }
    }

    public async Task<bool> SendWebhookAsync(Guid userId, string eventType, object payload, int maxRetries = 3)
    {
        try
        {
            var webhookUrl = await GetWebhookUrlAsync(userId);
            if (string.IsNullOrEmpty(webhookUrl))
            {
                _logger.LogWarning($"Nenhuma URL de webhook configurada para usuário {userId}");
                return false;
            }

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    var response = await _httpClient.PostAsync(webhookUrl, content);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation($"Webhook enviado com sucesso para {userId}: {eventType}");
                        return true;
                    }

                    _logger.LogWarning($"Webhook falhou (tentativa {attempt}/{maxRetries}) para {userId}: {response.StatusCode}");
                    
                    if (attempt < maxRetries)
                        await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt))); // Exponential backoff
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogWarning($"Erro ao enviar webhook (tentativa {attempt}/{maxRetries}) para {userId}: {ex.Message}");
                    
                    if (attempt < maxRetries)
                        await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)));
                }
            }

            _logger.LogError($"Webhook falhou após {maxRetries} tentativas para usuário {userId}");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao enviar webhook para usuário {userId}");
            return false;
        }
    }

    public async Task RetryFailedWebhooksAsync()
    {
        try
        {
            var failedWebhooks = await _webhookLogRepository.GetFailedAsync(100);
            
            foreach (var webhook in failedWebhooks)
            {
                try
                {
                    var transaction = await _transactionRepository.GetByIdAsync(webhook.TransactionId);
                    if (transaction == null)
                        continue;

                    var user = await _userRepository.GetByIdAsync(transaction.UserId);
                    if (user == null)
                        continue;

                    var payload = JsonSerializer.Deserialize<object>(webhook.Payload);
                    var success = await SendWebhookAsync(user.Id, webhook.EventType, payload, maxRetries: 3);

                    if (success)
                    {
                        webhook.Status = "PROCESSED";
                        webhook.ProcessedAt = DateTime.UtcNow;
                    }
                    else
                    {
                        webhook.Status = "FAILED";
                        webhook.ErrorMessage = "Falha após retry";
                    }

                    await _webhookLogRepository.UpdateAsync(webhook);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Erro ao reprocessar webhook {webhook.Id}");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao reprocessar webhooks falhados");
        }
    }
}

