using FinTechBanking.Core.DTOs;

namespace FinTechBanking.Core.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<RegisterResponse> RegisterAsync(RegisterRequest request);
    Task<bool> ValidateTokenAsync(string token);
    string GenerateJwtToken(Guid userId, string email);
}

