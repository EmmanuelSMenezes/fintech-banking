namespace FinTechBanking.Core.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
    public string TransactionType { get; set; } // "PIX_QR_CODE", "WITHDRAWAL", "DEPOSIT"
    public decimal Amount { get; set; }
    public string Status { get; set; } // "PENDING", "COMPLETED", "FAILED", "CANCELLED"
    public string? ExternalId { get; set; } // ID do banco
    public string? Description { get; set; }
    public string? QrCodeData { get; set; } // Dados do QR Code (para PIX)
    public string? RecipientKey { get; set; } // Chave PIX do destinatário
    public string BankCode { get; set; } // Código do banco (ex: "001" para Sicoob)
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}

