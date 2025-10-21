namespace FinTechBanking.Core.DTOs;

public class CreatePixQrCodeRequest
{
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string RecipientKey { get; set; } // Chave PIX do destinatário
}

public class CreatePixQrCodeResponse
{
    public Guid TransactionId { get; set; }
    public string QrCode { get; set; }
    public string QrCodeUrl { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; }
}

public class WithdrawalRequest
{
    public decimal Amount { get; set; }
    public string Description { get; set; }
}

public class WithdrawalResponse
{
    public Guid TransactionId { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; }
}

public class TransactionStatusResponse
{
    public Guid TransactionId { get; set; }
    public string TransactionType { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}

public class AccountBalanceResponse
{
    public Guid AccountId { get; set; }
    public decimal Balance { get; set; }
    public string BankCode { get; set; }
}

// DTOs para Consumers
public class PixQrCodeRequestDto
{
    public Guid TransactionId { get; set; }
    public string RecipientKey { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class WithdrawalRequestDto
{
    public Guid TransactionId { get; set; }
    public decimal Amount { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public string BankCode { get; set; } = string.Empty;
}
