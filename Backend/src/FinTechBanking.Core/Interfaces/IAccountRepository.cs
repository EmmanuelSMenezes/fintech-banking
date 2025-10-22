using FinTechBanking.Core.Entities;

namespace FinTechBanking.Core.Interfaces;

public interface IAccountRepository
{
    Task<Account> GetByIdAsync(Guid id);
    Task<Account> GetByUserIdAsync(Guid userId);
    Task<Account> GetByAccountNumberAsync(string accountNumber);
    Task<Account> CreateAsync(Account account);
    Task<Account> UpdateAsync(Account account);
    Task<bool> DeleteAsync(Guid id);
}

