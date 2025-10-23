namespace FinTechBanking.Core.Entities;

/// <summary>
/// Entidade que representa uma transferência agendada
/// </summary>
public class ScheduledTransfer
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid AccountId { get; set; }
    public string RecipientKey { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public string Status { get; set; } = "PENDING"; // PENDING, EXECUTED, CANCELLED, FAILED
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? ExecutedAt { get; set; }
    public string? ErrorMessage { get; set; }
}

