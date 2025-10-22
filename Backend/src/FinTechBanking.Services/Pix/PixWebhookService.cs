using System.Text;
using System.Text.Json;
using FinTechBanking.Core.DTOs;
using FinTechBanking.Core.Entities;
using FinTechBanking.Core.Interfaces;

namespace FinTechBanking.Services.Pix;

/// <summary>
/// Serviço para gerenciar Webhooks de PIX
/// </summary>
public class PixWebhookService : IPixWebhookService
{
    private readonly IPixWebhookRepository _webhookRepository;
    private const int MaxRetries = 3;
    private const int TimeoutSeconds = 10;

    public PixWebhookService(IPixWebhookRepository webhookRepository)
    {
        _webhookRepository = webhookRepository;
    }

    public async Task<PixWebhook> RegistrarWebhookAsync(Guid userId, string eventType, string webhookUrl)
    {
        // Validações
        if (string.IsNullOrWhiteSpace(eventType))
            throw new ArgumentException("Tipo de evento é obrigatório");

        if (string.IsNullOrWhiteSpace(webhookUrl))
            throw new ArgumentException("URL do webhook é obrigatória");

        if (!Uri.TryCreate(webhookUrl, UriKind.Absolute, out _))
            throw new ArgumentException("URL do webhook inválida");

        // Validar tipo de evento
        var eventosValidos = new[] { "pix-dinamico-criado", "pix-dinamico-pago", "pix-dinamico-expirado", "pix-dinamico-cancelado" };
        if (!eventosValidos.Contains(eventType))
            throw new ArgumentException($"Tipo de evento inválido. Eventos válidos: {string.Join(", ", eventosValidos)}");

        var webhook = new PixWebhook
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            EventType = eventType,
            WebhookUrl = webhookUrl,
            IsActive = true,
            RetryCount = 0,
            CreatedAt = DateTime.UtcNow
        };

        return await _webhookRepository.CreateAsync(webhook);
    }

    public async Task<List<PixWebhook>> ListarWebhooksAsync(Guid userId)
    {
        return await _webhookRepository.GetByUserIdAsync(userId);
    }

    public async Task<bool> DeletarWebhookAsync(Guid webhookId, Guid userId)
    {
        var webhook = await _webhookRepository.GetByIdAsync(webhookId);
        if (webhook == null || webhook.UserId != userId)
            throw new InvalidOperationException("Webhook não encontrado");

        return await _webhookRepository.DeleteAsync(webhookId);
    }

    public async Task<bool> TestarWebhookAsync(Guid webhookId, Guid userId)
    {
        var webhook = await _webhookRepository.GetByIdAsync(webhookId);
        if (webhook == null || webhook.UserId != userId)
            throw new InvalidOperationException("Webhook não encontrado");

        var testEvent = new PixWebhookEventDto
        {
            EventType = "test",
            Timestamp = DateTime.UtcNow,
            Data = new { message = "Teste de webhook" }
        };

        return await SendWebhookAsync(webhook.WebhookUrl, testEvent);
    }

    public async Task<bool> EnviarNotificacaoAsync(Guid userId, string eventType, object payload)
    {
        var webhooks = await _webhookRepository.GetActiveByUserAndEventAsync(userId, eventType);
        
        if (!webhooks.Any())
            return true;

        var webhookEvent = new PixWebhookEventDto
        {
            EventType = eventType,
            Timestamp = DateTime.UtcNow,
            Data = payload
        };

        var tasks = webhooks.Select(w => SendWebhookWithRetryAsync(w, webhookEvent));
        await Task.WhenAll(tasks);

        return true;
    }

    public async Task<PixWebhook> AtivarDesativarWebhookAsync(Guid webhookId, Guid userId, bool isActive)
    {
        var webhook = await _webhookRepository.GetByIdAsync(webhookId);
        if (webhook == null || webhook.UserId != userId)
            throw new InvalidOperationException("Webhook não encontrado");

        webhook.IsActive = isActive;
        webhook.UpdatedAt = DateTime.UtcNow;

        await _webhookRepository.UpdateAsync(webhook);
        return webhook;
    }

    private async Task SendWebhookWithRetryAsync(PixWebhook webhook, PixWebhookEventDto webhookEvent)
    {
        for (int i = 0; i < MaxRetries; i++)
        {
            try
            {
                var success = await SendWebhookAsync(webhook.WebhookUrl, webhookEvent);
                if (success)
                {
                    webhook.RetryCount = 0;
                    webhook.LastAttempt = DateTime.UtcNow;
                    await _webhookRepository.UpdateAsync(webhook);
                    return;
                }
            }
            catch (Exception ex)
            {
                webhook.RetryCount = i + 1;
                webhook.LastAttempt = DateTime.UtcNow;
                
                if (i == MaxRetries - 1)
                {
                    await _webhookRepository.UpdateAsync(webhook);
                    return;
                }

                // Aguardar antes de tentar novamente (backoff exponencial)
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, i)));
            }
        }
    }

    private async Task<bool> SendWebhookAsync(string webhookUrl, PixWebhookEventDto webhookEvent)
    {
        try
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(TimeoutSeconds);

            var json = JsonSerializer.Serialize(webhookEvent);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(webhookUrl, content);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}

