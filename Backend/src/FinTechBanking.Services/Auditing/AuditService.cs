using System.Text.Json;
using Microsoft.Extensions.Logging;
using FinTechBanking.Core.Entities;
using FinTechBanking.Core.Interfaces;

namespace FinTechBanking.Services.Auditing;

public class AuditService : IAuditService
{
    private readonly IAuditRepository _auditRepository;
    private readonly ILogger<AuditService> _logger;

    public AuditService(IAuditRepository auditRepository, ILogger<AuditService> logger)
    {
        _auditRepository = auditRepository;
        _logger = logger;
    }

    public async Task LogActionAsync(
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
        string? newValues = null)
    {
        try
        {
            var auditLog = new AuditLog
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                UserEmail = userEmail,
                Action = action,
                Entity = entity,
                EntityId = entityId,
                Description = description,
                HttpMethod = httpMethod,
                Endpoint = endpoint,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                StatusCode = statusCode,
                ExecutionTimeMs = executionTimeMs,
                Result = result,
                ErrorMessage = errorMessage,
                OldValues = oldValues,
                NewValues = newValues,
                CreatedAt = DateTime.UtcNow
            };

            await _auditRepository.CreateAsync(auditLog);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao registrar auditoria: {action} em {entity}");
        }
    }

    public async Task LogLoginAsync(Guid userId, string email, string ipAddress, string? userAgent)
    {
        await LogActionAsync(
            userId: userId,
            userEmail: email,
            action: "LOGIN",
            entity: "User",
            entityId: userId.ToString(),
            description: $"Usuário {email} realizou login",
            httpMethod: "POST",
            endpoint: "/api/auth/login",
            ipAddress: ipAddress,
            userAgent: userAgent,
            statusCode: 200,
            executionTimeMs: null,
            result: "Success");
    }

    public async Task LogLogoutAsync(Guid userId, string email, string ipAddress, string? userAgent)
    {
        await LogActionAsync(
            userId: userId,
            userEmail: email,
            action: "LOGOUT",
            entity: "User",
            entityId: userId.ToString(),
            description: $"Usuário {email} realizou logout",
            httpMethod: "POST",
            endpoint: "/api/auth/logout",
            ipAddress: ipAddress,
            userAgent: userAgent,
            statusCode: 200,
            executionTimeMs: null,
            result: "Success");
    }

    public async Task LogCreateAsync(Guid? userId, string entity, string entityId, string description, object? newValues = null)
    {
        var newValuesJson = newValues != null ? JsonSerializer.Serialize(newValues) : null;

        await LogActionAsync(
            userId: userId,
            userEmail: null,
            action: "CREATE",
            entity: entity,
            entityId: entityId,
            description: description,
            httpMethod: "POST",
            endpoint: $"/api/{entity.ToLower()}",
            ipAddress: null,
            userAgent: null,
            statusCode: 201,
            executionTimeMs: null,
            result: "Success",
            newValues: newValuesJson);
    }

    public async Task LogUpdateAsync(Guid? userId, string entity, string entityId, string description, object? oldValues = null, object? newValues = null)
    {
        var oldValuesJson = oldValues != null ? JsonSerializer.Serialize(oldValues) : null;
        var newValuesJson = newValues != null ? JsonSerializer.Serialize(newValues) : null;

        await LogActionAsync(
            userId: userId,
            userEmail: null,
            action: "UPDATE",
            entity: entity,
            entityId: entityId,
            description: description,
            httpMethod: "PUT",
            endpoint: $"/api/{entity.ToLower()}",
            ipAddress: null,
            userAgent: null,
            statusCode: 200,
            executionTimeMs: null,
            result: "Success",
            oldValues: oldValuesJson,
            newValues: newValuesJson);
    }

    public async Task LogDeleteAsync(Guid? userId, string entity, string entityId, string description, object? oldValues = null)
    {
        var oldValuesJson = oldValues != null ? JsonSerializer.Serialize(oldValues) : null;

        await LogActionAsync(
            userId: userId,
            userEmail: null,
            action: "DELETE",
            entity: entity,
            entityId: entityId,
            description: description,
            httpMethod: "DELETE",
            endpoint: $"/api/{entity.ToLower()}",
            ipAddress: null,
            userAgent: null,
            statusCode: 204,
            executionTimeMs: null,
            result: "Success",
            oldValues: oldValuesJson);
    }

    public async Task LogErrorAsync(Guid? userId, string action, string entity, string description, string errorMessage, string endpoint)
    {
        await LogActionAsync(
            userId: userId,
            userEmail: null,
            action: action,
            entity: entity,
            entityId: null,
            description: description,
            httpMethod: "POST",
            endpoint: endpoint,
            ipAddress: null,
            userAgent: null,
            statusCode: 500,
            executionTimeMs: null,
            result: "Error",
            errorMessage: errorMessage);
    }

    public async Task<IEnumerable<AuditLog>> GetUserLogsAsync(Guid userId, int limit = 100)
    {
        return await _auditRepository.GetByUserIdAsync(userId, limit);
    }

    public async Task<IEnumerable<AuditLog>> GetEntityLogsAsync(string entity, int limit = 100)
    {
        return await _auditRepository.GetByEntityAsync(entity, limit);
    }

    public async Task<IEnumerable<AuditLog>> GetLogsAsync(
        string? userId = null,
        string? action = null,
        string? entity = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int page = 1,
        int limit = 50)
    {
        return await _auditRepository.GetWithFiltersAsync(userId, action, entity, startDate, endDate, page, limit);
    }
}

