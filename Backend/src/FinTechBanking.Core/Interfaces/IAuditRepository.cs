using FinTechBanking.Core.Entities;

namespace FinTechBanking.Core.Interfaces;

public interface IAuditRepository
{
    /// <summary>
    /// Cria um novo log de auditoria
    /// </summary>
    Task<AuditLog> CreateAsync(AuditLog auditLog);

    /// <summary>
    /// Obtém um log por ID
    /// </summary>
    Task<AuditLog?> GetByIdAsync(Guid id);

    /// <summary>
    /// Obtém todos os logs de um usuário
    /// </summary>
    Task<IEnumerable<AuditLog>> GetByUserIdAsync(Guid userId, int limit = 100);

    /// <summary>
    /// Obtém logs por entidade
    /// </summary>
    Task<IEnumerable<AuditLog>> GetByEntityAsync(string entity, int limit = 100);

    /// <summary>
    /// Obtém logs por ação
    /// </summary>
    Task<IEnumerable<AuditLog>> GetByActionAsync(string action, int limit = 100);

    /// <summary>
    /// Obtém logs por período
    /// </summary>
    Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, int limit = 100);

    /// <summary>
    /// Obtém todos os logs com paginação
    /// </summary>
    Task<IEnumerable<AuditLog>> GetAllAsync(int page = 1, int limit = 50);

    /// <summary>
    /// Obtém logs com filtros
    /// </summary>
    Task<IEnumerable<AuditLog>> GetWithFiltersAsync(
        string? userId = null,
        string? action = null,
        string? entity = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int page = 1,
        int limit = 50);

    /// <summary>
    /// Conta total de logs
    /// </summary>
    Task<int> CountAsync();

    /// <summary>
    /// Deleta logs antigos (retenção de dados)
    /// </summary>
    Task<int> DeleteOlderThanAsync(DateTime date);
}

