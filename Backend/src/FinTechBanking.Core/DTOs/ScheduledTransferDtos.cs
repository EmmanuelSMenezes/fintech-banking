namespace FinTechBanking.Core.DTOs;

public class AgendarTransferenciaRequest
{
    public Guid AccountId { get; set; }
    public string RecipientKey { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
}

public class ScheduledTransferResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid AccountId { get; set; }
    public string RecipientKey { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? ExecutedAt { get; set; }
    public string? ErrorMessage { get; set; }
}

public class ListarTransferenciasAgendadasResponse
{
    public List<ScheduledTransferResponse> Transferencias { get; set; } = new();
    public int Total { get; set; }
}

public class CancelarTransferenciaRequest
{
    public Guid TransferId { get; set; }
}

