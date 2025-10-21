using FinTechBanking.Core.Interfaces;
using FinTechBanking.Banking.Services;

namespace FinTechBanking.Banking.Hub;

public class BankingHub : IBankingHub
{
    private readonly Dictionary<string, SicoobBankService> _bankServices;

    public BankingHub(Dictionary<string, SicoobBankService> bankServices)
    {
        _bankServices = bankServices;
    }

    public async Task<string> GeneratePixQrCodeAsync(string bankCode, decimal amount, string recipientKey, string description)
    {
        if (!_bankServices.TryGetValue(bankCode, out var bankService))
            throw new InvalidOperationException($"Bank service not found for code: {bankCode}");

        return await bankService.GeneratePixQrCodeAsync(amount, recipientKey, description);
    }

    public async Task<bool> ProcessWithdrawalAsync(string bankCode, decimal amount, string accountNumber)
    {
        if (!_bankServices.TryGetValue(bankCode, out var bankService))
            throw new InvalidOperationException($"Bank service not found for code: {bankCode}");

        return await bankService.ProcessWithdrawalAsync(amount, accountNumber);
    }

    public async Task<decimal> GetBalanceAsync(string bankCode, string accountNumber)
    {
        if (!_bankServices.TryGetValue(bankCode, out var bankService))
            throw new InvalidOperationException($"Bank service not found for code: {bankCode}");

        return await bankService.GetBalanceAsync(accountNumber);
    }
}

