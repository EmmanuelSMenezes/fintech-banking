using Microsoft.AspNetCore.Mvc;
using FinTechBanking.Core.DTOs;
using FinTechBanking.Core.Interfaces;
using FinTechBanking.Services.Auth;

namespace FinTechBanking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthController(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var authService = new AuthService(
                _userRepository,
                jwtSettings["SecretKey"],
                jwtSettings["Issuer"],
                jwtSettings["Audience"],
                int.Parse(jwtSettings["ExpirationMinutes"])
            );

            var response = await authService.RegisterAsync(request);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred during registration", error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var authService = new AuthService(
                _userRepository,
                jwtSettings["SecretKey"],
                jwtSettings["Issuer"],
                jwtSettings["Audience"],
                int.Parse(jwtSettings["ExpirationMinutes"])
            );

            var response = await authService.LoginAsync(request);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred during login", error = ex.Message });
        }
    }
}

