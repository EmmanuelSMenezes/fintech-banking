namespace FinTechBanking.Core.DTOs;

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresIn { get; set; }
}

public class RegisterRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string Document { get; set; }
    public string PhoneNumber { get; set; }
}

public class RegisterResponse
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
}

