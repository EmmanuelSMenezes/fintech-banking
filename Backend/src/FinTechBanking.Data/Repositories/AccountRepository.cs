using Dapper;
using Npgsql;
using FinTechBanking.Core.Entities;
using FinTechBanking.Core.Interfaces;

namespace FinTechBanking.Data.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly string _connectionString;

    public AccountRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Account> GetByIdAsync(Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "SELECT * FROM accounts WHERE id = @Id";
        return await connection.QueryFirstOrDefaultAsync<Account>(sql, new { Id = id });
    }

    public async Task<Account> GetByUserIdAsync(Guid userId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "SELECT * FROM accounts WHERE user_id = @UserId";
        return await connection.QueryFirstOrDefaultAsync<Account>(sql, new { UserId = userId });
    }

    public async Task<Account> GetByAccountNumberAsync(string accountNumber)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "SELECT * FROM accounts WHERE account_number = @AccountNumber";
        return await connection.QueryFirstOrDefaultAsync<Account>(sql, new { AccountNumber = accountNumber });
    }

    public async Task<Account> CreateAsync(Account account)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            INSERT INTO accounts (id, user_id, account_number, bank_code, balance, is_active, created_at)
            VALUES (@Id, @UserId, @AccountNumber, @BankCode, @Balance, @IsActive, @CreatedAt)
            RETURNING *";
        
        return await connection.QueryFirstOrDefaultAsync<Account>(sql, account);
    }

    public async Task<Account> UpdateAsync(Account account)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            UPDATE accounts 
            SET balance = @Balance, is_active = @IsActive, updated_at = @UpdatedAt
            WHERE id = @Id
            RETURNING *";
        
        return await connection.QueryFirstOrDefaultAsync<Account>(sql, account);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "DELETE FROM accounts WHERE id = @Id";
        var result = await connection.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }
}

