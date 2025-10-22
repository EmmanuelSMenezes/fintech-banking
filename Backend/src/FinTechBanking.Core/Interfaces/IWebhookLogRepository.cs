using FinTechBanking.Core.Entities;

namespace FinTechBanking.Core.Interfaces;

public interface IWebhookLogRepository
{
    Task<WebhookLog> CreateAsync(WebhookLog webhookLog);
    Task<WebhookLog?> GetByIdAsync(Guid id);
    Task<IEnumerable<WebhookLog>> GetByTransactionIdAsync(Guid transactionId);
    Task<IEnumerable<WebhookLog>> GetFailedAsync(int limit = 100);
    Task UpdateAsync(WebhookLog webhookLog);
    Task<IEnumerable<WebhookLog>> GetAllAsync();
}

