using Dapper;
using FinTechBanking.Core.Entities;
using FinTechBanking.Core.Interfaces;
using Npgsql;

namespace FinTechBanking.Data.Repositories;

public class ScheduledTransferRepository : IScheduledTransferRepository
{
    private readonly string _connectionString;

    public ScheduledTransferRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<ScheduledTransfer> CreateAsync(ScheduledTransfer transfer)
    {
        const string sql = @"
            INSERT INTO scheduled_transfers 
            (id, user_id, account_id, recipient_key, amount, description, scheduled_date, status, created_at, updated_at)
            VALUES (@Id, @UserId, @AccountId, @RecipientKey, @Amount, @Description, @ScheduledDate, @Status, @CreatedAt, @UpdatedAt)
            RETURNING *";

        using var connection = new NpgsqlConnection(_connectionString);
        var result = await connection.QuerySingleAsync<ScheduledTransfer>(sql, transfer);
        return result;
    }

    public async Task<ScheduledTransfer?> GetByIdAsync(Guid id)
    {
        const string sql = "SELECT * FROM scheduled_transfers WHERE id = @Id";

        using var connection = new NpgsqlConnection(_connectionString);
        var result = await connection.QuerySingleOrDefaultAsync<ScheduledTransfer>(sql, new { Id = id });
        return result;
    }

    public async Task<List<ScheduledTransfer>> GetByUserIdAsync(Guid userId)
    {
        const string sql = @"
            SELECT * FROM scheduled_transfers 
            WHERE user_id = @UserId 
            ORDER BY scheduled_date DESC";

        using var connection = new NpgsqlConnection(_connectionString);
        var results = await connection.QueryAsync<ScheduledTransfer>(sql, new { UserId = userId });
        return results.ToList();
    }

    public async Task<List<ScheduledTransfer>> GetPendingTransfersAsync()
    {
        const string sql = @"
            SELECT * FROM scheduled_transfers 
            WHERE status = 'PENDING' AND scheduled_date <= NOW()
            ORDER BY scheduled_date ASC";

        using var connection = new NpgsqlConnection(_connectionString);
        var results = await connection.QueryAsync<ScheduledTransfer>(sql);
        return results.ToList();
    }

    public async Task<ScheduledTransfer> UpdateAsync(ScheduledTransfer transfer)
    {
        const string sql = @"
            UPDATE scheduled_transfers 
            SET status = @Status, 
                executed_at = @ExecutedAt, 
                error_message = @ErrorMessage,
                updated_at = @UpdatedAt
            WHERE id = @Id
            RETURNING *";

        using var connection = new NpgsqlConnection(_connectionString);
        var result = await connection.QuerySingleAsync<ScheduledTransfer>(sql, transfer);
        return result;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        const string sql = "DELETE FROM scheduled_transfers WHERE id = @Id";

        using var connection = new NpgsqlConnection(_connectionString);
        var result = await connection.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }
}

