namespace FinTechBanking.Core.Interfaces;

/// <summary>
/// Interface para integração com Sicoob
/// </summary>
public interface ISicoobBankService
{
    /// <summary>
    /// Gera um QR Code PIX
    /// </summary>
    Task<string> GeneratePixQrCodeAsync(decimal amount, string recipientKey, string description);

    /// <summary>
    /// Processa um saque
    /// </summary>
    Task<bool> ProcessWithdrawalAsync(decimal amount, string accountNumber);

    /// <summary>
    /// Obtém o saldo da conta
    /// </summary>
    Task<decimal> GetBalanceAsync(string accountNumber);
}

