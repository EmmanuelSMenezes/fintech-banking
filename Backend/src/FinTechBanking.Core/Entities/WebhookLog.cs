namespace FinTechBanking.Core.Entities;

public class WebhookLog
{
    public Guid Id { get; set; }
    public Guid TransactionId { get; set; }
    public string EventType { get; set; } // "PIX_RECEIVED", "WITHDRAWAL_CONFIRMED", etc
    public string Payload { get; set; } // JSON do webhook
    public string Status { get; set; } // "RECEIVED", "PROCESSED", "FAILED"
    public string? ErrorMessage { get; set; }
    public DateTime ReceivedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
}

