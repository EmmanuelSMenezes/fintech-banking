using System.Text.Json;
using FinTechBanking.Core.Interfaces;
using FinTechBanking.Core.Entities;

namespace FinTechBanking.Workers.Consumers;

/// <summary>
/// Consumer para processar eventos de webhooks da fila RabbitMQ
/// </summary>
public class WebhookEventConsumer
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMessageBroker _messageBroker;

    public WebhookEventConsumer(
        ITransactionRepository transactionRepository,
        IUserRepository userRepository,
        IMessageBroker messageBroker)
    {
        _transactionRepository = transactionRepository;
        _userRepository = userRepository;
        _messageBroker = messageBroker;
    }

    /// <summary>
    /// Processa um evento de webhook
    /// </summary>
    public async Task ProcessAsync(WebhookEventDto webhookEvent)
    {
        try
        {
            Console.WriteLine($"[WebhookEventConsumer] Processando webhook: {webhookEvent.TransactionId}");

            // 1. Obter transação
            var transaction = await _transactionRepository.GetByIdAsync(webhookEvent.TransactionId);
            if (transaction == null)
            {
                Console.WriteLine($"[WebhookEventConsumer] Transação não encontrada: {webhookEvent.TransactionId}");
                return;
            }

            // 2. Validar assinatura (TODO: implementar validação real)
            ValidateWebhookSignature(webhookEvent);

            // 3. Atualizar status da transação
            transaction.Status = webhookEvent.Status;
            transaction.ExternalId = webhookEvent.ExternalId;
            transaction.UpdatedAt = DateTime.UtcNow;
            await _transactionRepository.UpdateAsync(transaction);

            Console.WriteLine($"[WebhookEventConsumer] Transação atualizada: {webhookEvent.TransactionId} -> {webhookEvent.Status}");

            // 4. Obter usuário para notificação
            var user = await _userRepository.GetByIdAsync(transaction.UserId);
            if (user != null && !string.IsNullOrEmpty(user.WebhookUrl))
            {
                // 5. Notificar cliente via webhook
                await NotifyClientAsync(user.WebhookUrl, transaction, webhookEvent);
            }

            // 6. Publicar evento de conclusão
            var completionEvent = new
            {
                TransactionId = webhookEvent.TransactionId,
                Status = webhookEvent.Status,
                Timestamp = DateTime.UtcNow
            };

            await _messageBroker.PublishAsync("webhook-processed", completionEvent);
            Console.WriteLine($"[WebhookEventConsumer] Evento de conclusão publicado: {webhookEvent.TransactionId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WebhookEventConsumer] Erro ao processar: {ex.Message}");
        }
    }

    /// <summary>
    /// Valida a assinatura do webhook
    /// </summary>
    private void ValidateWebhookSignature(WebhookEventDto webhookEvent)
    {
        // TODO: Implementar validação real de assinatura
        // Por enquanto, apenas log
        Console.WriteLine($"[WebhookEventConsumer] Validando assinatura do webhook");
    }

    /// <summary>
    /// Notifica o cliente sobre a atualização da transação
    /// </summary>
    private async Task NotifyClientAsync(string webhookUrl, Transaction transaction, WebhookEventDto webhookEvent)
    {
        try
        {
            Console.WriteLine($"[WebhookEventConsumer] Notificando cliente: {webhookUrl}");

            using var httpClient = new HttpClient();
            var payload = new
            {
                TransactionId = transaction.Id,
                Status = webhookEvent.Status,
                Amount = transaction.Amount,
                Timestamp = DateTime.UtcNow
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(webhookUrl, content);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[WebhookEventConsumer] Cliente notificado com sucesso");
            }
            else
            {
                Console.WriteLine($"[WebhookEventConsumer] Erro ao notificar cliente: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WebhookEventConsumer] Erro ao notificar cliente: {ex.Message}");
        }
    }
}

/// <summary>
/// DTO para eventos de webhook
/// </summary>
public class WebhookEventDto
{
    public Guid TransactionId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ExternalId { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public string Signature { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}

