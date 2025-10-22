using FinTechBanking.Core.Entities;

namespace FinTechBanking.Core.Interfaces;

/// <summary>
/// Interface para repositório de PIX Dinâmico
/// </summary>
public interface IPixRepository
{
    /// <summary>
    /// Criar novo PIX dinâmico
    /// </summary>
    Task<PixDinamico> CreateAsync(PixDinamico pixDinamico);
    
    /// <summary>
    /// Obter PIX dinâmico por ID
    /// </summary>
    Task<PixDinamico?> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Obter PIX dinâmicos por usuário
    /// </summary>
    Task<List<PixDinamico>> GetByUserIdAsync(Guid userId);
    
    /// <summary>
    /// Atualizar PIX dinâmico
    /// </summary>
    Task<PixDinamico> UpdateAsync(PixDinamico pixDinamico);
    
    /// <summary>
    /// Deletar PIX dinâmico
    /// </summary>
    Task<bool> DeleteAsync(Guid id);
}

