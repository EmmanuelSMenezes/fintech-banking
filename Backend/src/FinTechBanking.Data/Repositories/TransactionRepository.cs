using Dapper;
using Npgsql;
using FinTechBanking.Core.Entities;
using FinTechBanking.Core.Interfaces;

namespace FinTechBanking.Data.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly string _connectionString;

    public TransactionRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Transaction> GetByIdAsync(Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "SELECT * FROM transactions WHERE id = @Id";
        return await connection.QueryFirstOrDefaultAsync<Transaction>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "SELECT * FROM transactions WHERE account_id = @AccountId ORDER BY created_at DESC";
        return await connection.QueryAsync<Transaction>(sql, new { AccountId = accountId });
    }

    public async Task<IEnumerable<Transaction>> GetAllAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "SELECT * FROM transactions ORDER BY created_at DESC";
        return await connection.QueryAsync<Transaction>(sql);
    }

    public async Task<Transaction> CreateAsync(Transaction transaction)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            INSERT INTO transactions (id, account_id, transaction_type, amount, status, external_id, description, qr_code_data, recipient_key, created_at)
            VALUES (@Id, @AccountId, @TransactionType, @Amount, @Status, @ExternalId, @Description, @QrCodeData, @RecipientKey, @CreatedAt)
            RETURNING *";
        
        return await connection.QueryFirstOrDefaultAsync<Transaction>(sql, transaction);
    }

    public async Task<Transaction> UpdateAsync(Transaction transaction)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            UPDATE transactions 
            SET status = @Status, external_id = @ExternalId, updated_at = @UpdatedAt, completed_at = @CompletedAt
            WHERE id = @Id
            RETURNING *";
        
        return await connection.QueryFirstOrDefaultAsync<Transaction>(sql, transaction);
    }

    public async Task<Transaction> GetByExternalIdAsync(string externalId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "SELECT * FROM transactions WHERE external_id = @ExternalId";
        return await connection.QueryFirstOrDefaultAsync<Transaction>(sql, new { ExternalId = externalId });
    }
}

