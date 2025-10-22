using Dapper;
using Npgsql;
using FinTechBanking.Core.Entities;
using FinTechBanking.Core.Interfaces;

namespace FinTechBanking.Data.Repositories;

/// <summary>
/// Repositório para Webhook de PIX
/// </summary>
public class PixWebhookRepository : IPixWebhookRepository
{
    private readonly string _connectionString;

    public PixWebhookRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<PixWebhook> CreateAsync(PixWebhook webhook)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            INSERT INTO pix_webhooks (
                id, user_id, event_type, webhook_url, is_active, 
                retry_count, last_attempt, created_at, updated_at
            )
            VALUES (
                @Id, @UserId, @EventType, @WebhookUrl, @IsActive,
                @RetryCount, @LastAttempt, @CreatedAt, @UpdatedAt
            )
            RETURNING *";

        return await connection.QueryFirstOrDefaultAsync<PixWebhook>(sql, webhook) 
            ?? throw new InvalidOperationException("Failed to create webhook");
    }

    public async Task<PixWebhook?> GetByIdAsync(Guid webhookId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "SELECT * FROM pix_webhooks WHERE id = @Id";
        return await connection.QueryFirstOrDefaultAsync<PixWebhook>(sql, new { Id = webhookId });
    }

    public async Task<List<PixWebhook>> GetByUserIdAsync(Guid userId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "SELECT * FROM pix_webhooks WHERE user_id = @UserId ORDER BY created_at DESC";
        var result = await connection.QueryAsync<PixWebhook>(sql, new { UserId = userId });
        return result.ToList();
    }

    public async Task<List<PixWebhook>> GetActiveByUserAndEventAsync(Guid userId, string eventType)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            SELECT * FROM pix_webhooks 
            WHERE user_id = @UserId AND event_type = @EventType AND is_active = true
            ORDER BY created_at DESC";
        var result = await connection.QueryAsync<PixWebhook>(sql, new { UserId = userId, EventType = eventType });
        return result.ToList();
    }

    public async Task<bool> UpdateAsync(PixWebhook webhook)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            UPDATE pix_webhooks 
            SET event_type = @EventType, webhook_url = @WebhookUrl, is_active = @IsActive,
                retry_count = @RetryCount, last_attempt = @LastAttempt, updated_at = @UpdatedAt
            WHERE id = @Id";

        var result = await connection.ExecuteAsync(sql, webhook);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid webhookId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "DELETE FROM pix_webhooks WHERE id = @Id";
        var result = await connection.ExecuteAsync(sql, new { Id = webhookId });
        return result > 0;
    }
}

