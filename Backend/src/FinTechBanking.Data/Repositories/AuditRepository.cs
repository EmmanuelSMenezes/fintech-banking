using Dapper;
using Npgsql;
using FinTechBanking.Core.Entities;
using FinTechBanking.Core.Interfaces;

namespace FinTechBanking.Data.Repositories;

public class AuditRepository : IAuditRepository
{
    private readonly string _connectionString;

    public AuditRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<AuditLog> CreateAsync(AuditLog auditLog)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            INSERT INTO audit_logs (id, user_id, user_email, action, entity, entity_id, description, 
                                   old_values, new_values, http_method, endpoint, ip_address, user_agent, 
                                   status_code, execution_time_ms, result, error_message, created_at)
            VALUES (@Id, @UserId, @UserEmail, @Action, @Entity, @EntityId, @Description, 
                   @OldValues, @NewValues, @HttpMethod, @Endpoint, @IpAddress, @UserAgent, 
                   @StatusCode, @ExecutionTimeMs, @Result, @ErrorMessage, @CreatedAt)
            RETURNING *";
        
        return await connection.QueryFirstOrDefaultAsync<AuditLog>(sql, auditLog) ?? auditLog;
    }

    public async Task<AuditLog?> GetByIdAsync(Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "SELECT * FROM audit_logs WHERE id = @Id";
        return await connection.QueryFirstOrDefaultAsync<AuditLog>(sql, new { Id = id });
    }

    public async Task<IEnumerable<AuditLog>> GetByUserIdAsync(Guid userId, int limit = 100)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            SELECT * FROM audit_logs 
            WHERE user_id = @UserId 
            ORDER BY created_at DESC 
            LIMIT @Limit";
        return await connection.QueryAsync<AuditLog>(sql, new { UserId = userId, Limit = limit });
    }

    public async Task<IEnumerable<AuditLog>> GetByEntityAsync(string entity, int limit = 100)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            SELECT * FROM audit_logs 
            WHERE entity = @Entity 
            ORDER BY created_at DESC 
            LIMIT @Limit";
        return await connection.QueryAsync<AuditLog>(sql, new { Entity = entity, Limit = limit });
    }

    public async Task<IEnumerable<AuditLog>> GetByActionAsync(string action, int limit = 100)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            SELECT * FROM audit_logs 
            WHERE action = @Action 
            ORDER BY created_at DESC 
            LIMIT @Limit";
        return await connection.QueryAsync<AuditLog>(sql, new { Action = action, Limit = limit });
    }

    public async Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, int limit = 100)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            SELECT * FROM audit_logs 
            WHERE created_at BETWEEN @StartDate AND @EndDate 
            ORDER BY created_at DESC 
            LIMIT @Limit";
        return await connection.QueryAsync<AuditLog>(sql, new { StartDate = startDate, EndDate = endDate, Limit = limit });
    }

    public async Task<IEnumerable<AuditLog>> GetAllAsync(int page = 1, int limit = 50)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var offset = (page - 1) * limit;
        const string sql = @"
            SELECT * FROM audit_logs 
            ORDER BY created_at DESC 
            LIMIT @Limit OFFSET @Offset";
        return await connection.QueryAsync<AuditLog>(sql, new { Limit = limit, Offset = offset });
    }

    public async Task<IEnumerable<AuditLog>> GetWithFiltersAsync(
        string? userId = null,
        string? action = null,
        string? entity = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int page = 1,
        int limit = 50)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var offset = (page - 1) * limit;
        
        var sql = "SELECT * FROM audit_logs WHERE 1=1";
        var parameters = new DynamicParameters();

        if (!string.IsNullOrEmpty(userId))
        {
            sql += " AND user_id = @UserId";
            parameters.Add("@UserId", Guid.Parse(userId));
        }

        if (!string.IsNullOrEmpty(action))
        {
            sql += " AND action = @Action";
            parameters.Add("@Action", action);
        }

        if (!string.IsNullOrEmpty(entity))
        {
            sql += " AND entity = @Entity";
            parameters.Add("@Entity", entity);
        }

        if (startDate.HasValue)
        {
            sql += " AND created_at >= @StartDate";
            parameters.Add("@StartDate", startDate.Value);
        }

        if (endDate.HasValue)
        {
            sql += " AND created_at <= @EndDate";
            parameters.Add("@EndDate", endDate.Value);
        }

        sql += " ORDER BY created_at DESC LIMIT @Limit OFFSET @Offset";
        parameters.Add("@Limit", limit);
        parameters.Add("@Offset", offset);

        return await connection.QueryAsync<AuditLog>(sql, parameters);
    }

    public async Task<int> CountAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "SELECT COUNT(*) FROM audit_logs";
        return await connection.ExecuteScalarAsync<int>(sql);
    }

    public async Task<int> DeleteOlderThanAsync(DateTime date)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "DELETE FROM audit_logs WHERE created_at < @Date";
        return await connection.ExecuteAsync(sql, new { Date = date });
    }
}

