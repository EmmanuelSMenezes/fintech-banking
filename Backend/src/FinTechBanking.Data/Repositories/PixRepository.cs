using Dapper;
using Npgsql;
using FinTechBanking.Core.Entities;
using FinTechBanking.Core.Interfaces;

namespace FinTechBanking.Data.Repositories;

/// <summary>
/// Repositório para PIX Dinâmico
/// </summary>
public class PixRepository : IPixRepository
{
    private readonly string _connectionString;

    public PixRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<PixDinamico> CreateAsync(PixDinamico pixDinamico)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            INSERT INTO pix_dinamicos (
                id, user_id, account_id, amount, description, recipient_key, 
                qr_code_data, qr_code_url, status, external_id, bank_code, 
                created_at, expires_at
            )
            VALUES (
                @Id, @UserId, @AccountId, @Amount, @Description, @RecipientKey,
                @QrCodeData, @QrCodeUrl, @Status, @ExternalId, @BankCode,
                @CreatedAt, @ExpiresAt
            )
            RETURNING *";

        return await connection.QueryFirstOrDefaultAsync<PixDinamico>(sql, pixDinamico) 
            ?? throw new InvalidOperationException("Failed to create PIX dinâmico");
    }

    public async Task<PixDinamico?> GetByIdAsync(Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "SELECT * FROM pix_dinamicos WHERE id = @Id";
        return await connection.QueryFirstOrDefaultAsync<PixDinamico>(sql, new { Id = id });
    }

    public async Task<List<PixDinamico>> GetByUserIdAsync(Guid userId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "SELECT * FROM pix_dinamicos WHERE user_id = @UserId ORDER BY created_at DESC";
        var result = await connection.QueryAsync<PixDinamico>(sql, new { UserId = userId });
        return result.ToList();
    }

    public async Task<PixDinamico> UpdateAsync(PixDinamico pixDinamico)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            UPDATE pix_dinamicos
            SET status = @Status, paid_at = @PaidAt, updated_at = @UpdatedAt
            WHERE id = @Id
            RETURNING *";

        return await connection.QueryFirstOrDefaultAsync<PixDinamico>(sql, pixDinamico)
            ?? throw new InvalidOperationException("Failed to update PIX dinâmico");
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "DELETE FROM pix_dinamicos WHERE id = @Id";
        var result = await connection.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }
}

