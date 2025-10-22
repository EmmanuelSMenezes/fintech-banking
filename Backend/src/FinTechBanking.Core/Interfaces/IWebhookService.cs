namespace FinTechBanking.Core.Interfaces;

public interface IWebhookService
{
    /// <summary>
    /// Registra uma URL de webhook para um usuário
    /// </summary>
    Task<bool> RegisterWebhookAsync(Guid userId, string webhookUrl);

    /// <summary>
    /// Remove o webhook de um usuário
    /// </summary>
    Task<bool> UnregisterWebhookAsync(Guid userId);

    /// <summary>
    /// Obtém a URL de webhook de um usuário
    /// </summary>
    Task<string?> GetWebhookUrlAsync(Guid userId);

    /// <summary>
    /// Envia notificação de webhook com retry logic
    /// </summary>
    Task<bool> SendWebhookAsync(Guid userId, string eventType, object payload, int maxRetries = 3);

    /// <summary>
    /// Reprocessa webhooks que falharam
    /// </summary>
    Task RetryFailedWebhooksAsync();
}

