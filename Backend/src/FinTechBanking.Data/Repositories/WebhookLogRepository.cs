using Dapper;
using Npgsql;
using FinTechBanking.Core.Entities;
using FinTechBanking.Core.Interfaces;

namespace FinTechBanking.Data.Repositories;

public class WebhookLogRepository : IWebhookLogRepository
{
    private readonly string _connectionString;

    public WebhookLogRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<WebhookLog> CreateAsync(WebhookLog webhookLog)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            INSERT INTO webhook_logs (id, transaction_id, event_type, payload, status, error_message, received_at, processed_at)
            VALUES (@Id, @TransactionId, @EventType, @Payload, @Status, @ErrorMessage, @ReceivedAt, @ProcessedAt)
            RETURNING *";
        
        return await connection.QueryFirstOrDefaultAsync<WebhookLog>(sql, webhookLog);
    }

    public async Task<WebhookLog?> GetByIdAsync(Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "SELECT * FROM webhook_logs WHERE id = @Id";
        return await connection.QueryFirstOrDefaultAsync<WebhookLog>(sql, new { Id = id });
    }

    public async Task<IEnumerable<WebhookLog>> GetByTransactionIdAsync(Guid transactionId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "SELECT * FROM webhook_logs WHERE transaction_id = @TransactionId ORDER BY received_at DESC";
        return await connection.QueryAsync<WebhookLog>(sql, new { TransactionId = transactionId });
    }

    public async Task<IEnumerable<WebhookLog>> GetFailedAsync(int limit = 100)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            SELECT * FROM webhook_logs 
            WHERE status = 'FAILED' 
            ORDER BY received_at ASC 
            LIMIT @Limit";
        return await connection.QueryAsync<WebhookLog>(sql, new { Limit = limit });
    }

    public async Task UpdateAsync(WebhookLog webhookLog)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            UPDATE webhook_logs 
            SET status = @Status, error_message = @ErrorMessage, processed_at = @ProcessedAt
            WHERE id = @Id";
        
        await connection.ExecuteAsync(sql, webhookLog);
    }

    public async Task<IEnumerable<WebhookLog>> GetAllAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "SELECT * FROM webhook_logs ORDER BY received_at DESC";
        return await connection.QueryAsync<WebhookLog>(sql);
    }
}

