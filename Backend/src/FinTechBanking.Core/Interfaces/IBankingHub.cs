namespace FinTechBanking.Core.Interfaces;

public interface IBankingHub
{
    Task<string> GeneratePixQrCodeAsync(string bankCode, decimal amount, string recipientKey, string description);
    Task<bool> ProcessWithdrawalAsync(string bankCode, decimal amount, string accountNumber);
    Task<decimal> GetBalanceAsync(string bankCode, string accountNumber);
}

