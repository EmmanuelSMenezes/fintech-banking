using FinTechBanking.Core.Entities;

namespace FinTechBanking.Core.Interfaces;

public interface ITransactionRepository
{
    Task<Transaction> GetByIdAsync(Guid id);
    Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId);
    Task<IEnumerable<Transaction>> GetAllAsync();
    Task<Transaction> CreateAsync(Transaction transaction);
    Task<Transaction> UpdateAsync(Transaction transaction);
    Task<Transaction> GetByExternalIdAsync(string externalId);
}

