using Dapper;
using Npgsql;
using FinTechBanking.Core.Entities;
using FinTechBanking.Core.Interfaces;

namespace FinTechBanking.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            SELECT
                id as Id,
                email as Email,
                password_hash as PasswordHash,
                full_name as FullName,
                document as Document,
                phone_number as PhoneNumber,
                is_active as IsActive,
                created_at as CreatedAt,
                updated_at as UpdatedAt,
                webhook_url as WebhookUrl
            FROM users WHERE id = @Id";
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            SELECT
                id as Id,
                email as Email,
                password_hash as PasswordHash,
                full_name as FullName,
                document as Document,
                phone_number as PhoneNumber,
                is_active as IsActive,
                created_at as CreatedAt,
                updated_at as UpdatedAt,
                webhook_url as WebhookUrl
            FROM users WHERE email = @Email";
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            SELECT
                id as Id,
                email as Email,
                password_hash as PasswordHash,
                full_name as FullName,
                document as Document,
                phone_number as PhoneNumber,
                is_active as IsActive,
                created_at as CreatedAt,
                updated_at as UpdatedAt,
                webhook_url as WebhookUrl
            FROM users ORDER BY created_at DESC";
        return await connection.QueryAsync<User>(sql);
    }

    public async Task<User> CreateAsync(User user)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            INSERT INTO users (id, email, password_hash, full_name, document, phone_number, is_active, created_at)
            VALUES (@Id, @Email, @PasswordHash, @FullName, @Document, @PhoneNumber, @IsActive, @CreatedAt)
            RETURNING *";
        
        return await connection.QueryFirstOrDefaultAsync<User>(sql, user);
    }

    public async Task<User> UpdateAsync(User user)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            UPDATE users 
            SET email = @Email, password_hash = @PasswordHash, full_name = @FullName, 
                document = @Document, phone_number = @PhoneNumber, is_active = @IsActive, 
                updated_at = @UpdatedAt, webhook_url = @WebhookUrl
            WHERE id = @Id
            RETURNING *";
        
        return await connection.QueryFirstOrDefaultAsync<User>(sql, user);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "DELETE FROM users WHERE id = @Id";
        var result = await connection.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "SELECT COUNT(*) FROM users WHERE email = @Email";
        var count = await connection.ExecuteScalarAsync<int>(sql, new { Email = email });
        return count > 0;
    }
}

