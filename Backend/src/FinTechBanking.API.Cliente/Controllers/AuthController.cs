using Microsoft.AspNetCore.Mvc;
using FinTechBanking.Core.Interfaces;
using FinTechBanking.Core.DTOs;
using FinTechBanking.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FinTechBanking.API.Cliente.Controllers;

/// <summary>
/// API Cliente - Autenticação
/// Endpoints para clientes gerarem tokens e autenticarem
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IUserRepository userRepository, IConfiguration configuration, ILogger<AuthController> logger)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Registrar novo cliente
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<object>> Register([FromBody] RegisterRequest request)
    {
        try
        {
            _logger.LogInformation("Cliente registrando: {Email}", request.Email);

            // Validar dados
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { message = "Email e senha são obrigatórios" });

            // Verificar se usuário já existe
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
                return BadRequest(new { message = "Email já cadastrado" });

            // Criar novo usuário
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new FinTechBanking.Core.Entities.User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                PasswordHash = passwordHash,
                FullName = request.FullName ?? "Cliente",
                Document = request.Document,
                PhoneNumber = request.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _userRepository.CreateAsync(user);

            _logger.LogInformation("Cliente registrado com sucesso: {Email}", request.Email);

            return Ok(new
            {
                message = "Registro realizado com sucesso",
                userId = user.Id,
                email = user.Email
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar cliente");
            return StatusCode(500, new { message = "Erro ao registrar", error = ex.Message });
        }
    }

    /// <summary>
    /// Login do cliente e geração de token
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<object>> Login([FromBody] LoginRequest request)
    {
        try
        {
            _logger.LogInformation("Cliente fazendo login: {Email}", request.Email);

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

            _logger.LogInformation("Cliente logado com sucesso: {Email}", request.Email);

            return Ok(new
            {
                message = "Login realizado com sucesso",
                token = token,
                userId = user.Id,
                email = user.Email,
                fullName = user.FullName
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer login");
            return StatusCode(500, new { message = "Erro ao fazer login", error = ex.Message });
        }
    }

    private string GenerateJwtToken(User user, IConfigurationSection jwtSettings)
    {
        var secretKey = jwtSettings["SecretKey"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "60");

        var key = new System.Text.UTF8Encoding().GetBytes(secretKey);
        var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

        var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
        {
            Subject = new System.Security.Claims.ClaimsIdentity(new[]
            {
                new System.Security.Claims.Claim("sub", user.Id.ToString()),
                new System.Security.Claims.Claim("email", user.Email),
                new System.Security.Claims.Claim("role", "cliente")
            }),
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

public class RegisterRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string Document { get; set; }
    public string PhoneNumber { get; set; }
}

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

