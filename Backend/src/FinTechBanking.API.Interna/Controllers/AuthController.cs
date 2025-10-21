using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinTechBanking.Core.Interfaces;
using FinTechBanking.Core.Entities;

namespace FinTechBanking.API.Interna.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IUserRepository userRepository,
        IConfiguration configuration,
        ILogger<AuthController> logger)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Login para administradores e clientes
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<object>> Login([FromBody] LoginRequest request)
    {
        try
        {
            _logger.LogInformation("Tentativa de login: {Email}", request.Email);

            // Validar dados
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { message = "Email e senha são obrigatórios" });

            // Buscar usuário
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                _logger.LogWarning("Falha de login para: {Email}", request.Email);
                return Unauthorized(new { message = "Email ou senha inválidos" });
            }

            // Gerar token JWT
            var jwtSettings = _configuration.GetSection("Jwt");
            var token = GenerateJwtToken(user, jwtSettings);

            _logger.LogInformation("Login bem-sucedido: {Email}", request.Email);

            return Ok(new
            {
                message = "Login realizado com sucesso",
                accessToken = token,
                refreshToken = token, // Simplificado para este exemplo
                expiresIn = "3600",
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    name = user.FullName,
                    role = user.Role ?? "user"
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer login");
            return StatusCode(500, new { message = "Erro ao fazer login", error = ex.Message });
        }
    }

    /// <summary>
    /// Logout
    /// </summary>
    [HttpPost("logout")]
    public ActionResult<object> Logout()
    {
        return Ok(new { message = "Logout realizado com sucesso" });
    }

    /// <summary>
    /// Seed - Criar usuário admin padrão (apenas em desenvolvimento)
    /// </summary>
    [HttpPost("seed")]
    public async Task<ActionResult<object>> Seed()
    {
        try
        {
            var adminEmail = "admin@owaypay.com";
            var existingAdmin = await _userRepository.GetByEmailAsync(adminEmail);

            if (existingAdmin != null)
                return Ok(new { message = "Admin user already exists" });

            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                Email = adminEmail,
                FullName = "Administrador Owaypay",
                Document = "000.000.000-00",
                PhoneNumber = "(11) 0000-0000",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                IsActive = true,
                Role = "admin",
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(adminUser);

            _logger.LogInformation("Admin user created successfully");

            return Ok(new
            {
                message = "Admin user created successfully",
                data = new
                {
                    id = adminUser.Id,
                    email = adminUser.Email,
                    fullName = adminUser.FullName,
                    role = adminUser.Role
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating admin user");
            return StatusCode(500, new { message = "Error creating admin user", error = ex.Message });
        }
    }

    private string GenerateJwtToken(User user, IConfigurationSection jwtSettings)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role ?? "user")
        };

        var tokenOptions = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpirationMinutes"])),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
}

public class LoginRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}

