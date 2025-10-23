using FinTechBanking.Core.Entities;

namespace FinTechBanking.Core.Interfaces;

public interface IScheduledTransferRepository
{
    Task<ScheduledTransfer> CreateAsync(ScheduledTransfer transfer);
    Task<ScheduledTransfer?> GetByIdAsync(Guid id);
    Task<List<ScheduledTransfer>> GetByUserIdAsync(Guid userId);
    Task<List<ScheduledTransfer>> GetPendingTransfersAsync();
    Task<ScheduledTransfer> UpdateAsync(ScheduledTransfer transfer);
    Task<bool> DeleteAsync(Guid id);
}

