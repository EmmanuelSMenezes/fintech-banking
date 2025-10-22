using FinTechBanking.Core.Entities;

namespace FinTechBanking.Core.Interfaces;

/// <summary>
/// Interface para serviço de PIX Dinâmico
/// </summary>
public interface IPixService
{
    /// <summary>
    /// Criar um novo PIX dinâmico
    /// </summary>
    Task<PixDinamico> CriarPixDinamicoAsync(Guid userId, decimal amount, string description, string recipientKey);
    
    /// <summary>
    /// Obter status de um PIX dinâmico
    /// </summary>
    Task<PixDinamico?> ObterStatusAsync(Guid pixId);
    
    /// <summary>
    /// Confirmar pagamento de um PIX dinâmico
    /// </summary>
    Task<PixDinamico> ConfirmarPagamentoAsync(Guid pixId);
    
    /// <summary>
    /// Listar PIX dinâmicos do usuário
    /// </summary>
    Task<List<PixDinamico>> ListarPixDinamicosAsync(Guid userId);
    
    /// <summary>
    /// Cancelar um PIX dinâmico
    /// </summary>
    Task<PixDinamico> CancelarPixAsync(Guid pixId);
}

