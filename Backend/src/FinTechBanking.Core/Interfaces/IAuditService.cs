using FinTechBanking.Core.Entities;

namespace FinTechBanking.Core.Interfaces;

public interface IAuditService
{
    /// <summary>
    /// Registra uma ação de auditoria
    /// </summary>
    Task LogActionAsync(
        Guid? userId,
        string? userEmail,
        string action,
        string entity,
        string? entityId,
        string description,
        string httpMethod,
        string endpoint,
        string? ipAddress,
        string? userAgent,
        int? statusCode,
        long? executionTimeMs,
        string result = "Success",
        string? errorMessage = null,
        string? oldValues = null,
        string? newValues = null);

    /// <summary>
    /// Registra um login
    /// </summary>
    Task LogLoginAsync(Guid userId, string email, string ipAddress, string? userAgent);

    /// <summary>
    /// Registra um logout
    /// </summary>
    Task LogLogoutAsync(Guid userId, string email, string ipAddress, string? userAgent);

    /// <summary>
    /// Registra uma criação de entidade
    /// </summary>
    Task LogCreateAsync(Guid? userId, string entity, string entityId, string description, object? newValues = null);

    /// <summary>
    /// Registra uma atualização de entidade
    /// </summary>
    Task LogUpdateAsync(Guid? userId, string entity, string entityId, string description, object? oldValues = null, object? newValues = null);

    /// <summary>
    /// Registra uma exclusão de entidade
    /// </summary>
    Task LogDeleteAsync(Guid? userId, string entity, string entityId, string description, object? oldValues = null);

    /// <summary>
    /// Registra um erro
    /// </summary>
    Task LogErrorAsync(Guid? userId, string action, string entity, string description, string errorMessage, string endpoint);

    /// <summary>
    /// Obtém logs de um usuário
    /// </summary>
    Task<IEnumerable<AuditLog>> GetUserLogsAsync(Guid userId, int limit = 100);

    /// <summary>
    /// Obtém logs de uma entidade
    /// </summary>
    Task<IEnumerable<AuditLog>> GetEntityLogsAsync(string entity, int limit = 100);

    /// <summary>
    /// Obtém todos os logs com filtros
    /// </summary>
    Task<IEnumerable<AuditLog>> GetLogsAsync(
        string? userId = null,
        string? action = null,
        string? entity = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int page = 1,
        int limit = 50);
}

