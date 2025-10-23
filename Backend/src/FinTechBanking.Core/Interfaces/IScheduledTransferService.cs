using FinTechBanking.Core.DTOs;

namespace FinTechBanking.Core.Interfaces;

public interface IScheduledTransferService
{
    Task<ScheduledTransferResponse> AgendarTransferenciaAsync(Guid userId, AgendarTransferenciaRequest request);
    Task<List<ScheduledTransferResponse>> ListarTransferenciasAgendadasAsync(Guid userId);
    Task<ScheduledTransferResponse> ObterDetalhesAsync(Guid transferId, Guid userId);
    Task<bool> CancelarTransferenciaAsync(Guid transferId, Guid userId);
    Task<bool> ExecutarTransferenciasAgendadasAsync();
}

