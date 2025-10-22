using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FinTechBanking.Core.Interfaces;
using FinTechBanking.Core.Entities;
using FinTechBanking.API.Interna.Attributes;
using Npgsql;

namespace FinTechBanking.API.Interna.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[RateLimit(maxRequests: 100, windowSeconds: 60)]
public class AdminController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IConfiguration _configuration;

    public AdminController(
        IUserRepository userRepository,
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _configuration = configuration;
    }

    [HttpPost("fix-admin-role")]
    [AllowAnonymous]
    public async Task<ActionResult<object>> FixAdminRole()
    {
        try
        {
            var admin = await _userRepository.GetByEmailAsync("admin@owaypay.com");
            if (admin == null)
                return NotFound(new { message = "Admin user not found" });

            admin.Role = "admin";
            await _userRepository.UpdateAsync(admin);

            return Ok(new { message = "Admin role updated successfully", admin = new { email = admin.Email, role = admin.Role } });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error updating admin role", error = ex.Message });
        }
    }

    [HttpGet("check-admin-role")]
    [AllowAnonymous]
    public async Task<ActionResult<object>> CheckAdminRole()
    {
        try
        {
            var admin = await _userRepository.GetByEmailAsync("admin@owaypay.com");
            if (admin == null)
                return NotFound(new { message = "Admin user not found" });

            return Ok(new { email = admin.Email, role = admin.Role });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error checking admin role", error = ex.Message });
        }
    }

    [HttpPost("migrate-add-role")]
    [AllowAnonymous]
    public async Task<ActionResult<object>> MigrateAddRole()
    {
        try
        {
            using var connection = new Npgsql.NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();

            // Check if role column exists
            var checkSql = @"
                SELECT COUNT(*) FROM information_schema.columns
                WHERE table_name = 'users' AND column_name = 'role'";

            var command = connection.CreateCommand();
            command.CommandText = checkSql;
            var result = (long)await command.ExecuteScalarAsync();

            if (result > 0)
                return Ok(new { message = "Role column already exists" });

            // Add role column
            var alterSql = "ALTER TABLE users ADD COLUMN role VARCHAR(50) DEFAULT 'user'";
            command.CommandText = alterSql;
            await command.ExecuteNonQueryAsync();

            // Update admin user
            var updateSql = "UPDATE users SET role = 'admin' WHERE email = 'admin@owaypay.com'";
            command.CommandText = updateSql;
            await command.ExecuteNonQueryAsync();

            return Ok(new { message = "Migration completed successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error running migration", error = ex.Message });
        }
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<ActionResult<object>> GetProfile()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized(new { message = "Invalid user ID" });

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(new
            {
                message = "Profile retrieved successfully",
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    fullName = user.FullName,
                    role = user.Role ?? "user"
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving profile", error = ex.Message });
        }
    }

    [HttpPost("users")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<object>> CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { message = "Email and password are required" });

            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
                return BadRequest(new { message = "User already exists" });

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                FullName = request.FullName,
                Document = request.Document,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                IsActive = true,
                Role = "user",
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user);

            // Create account for the user
            var account = new Account
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                AccountNumber = $"OWP{user.Id.ToString().Substring(0, 8).ToUpper()}",
                BankCode = "001",
                Balance = 0,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _accountRepository.CreateAsync(account);

            return Ok(new
            {
                message = "User created successfully",
                data = new
                {
                    id = user.Id,
                    email = user.Email,
                    fullName = user.FullName,
                    accountId = account.Id
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error creating user", error = ex.Message });
        }
    }

    [HttpGet("users")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<object>> ListUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var allUsers = await _userRepository.GetAllAsync();
            var users = allUsers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new
                {
                    id = u.Id,
                    email = u.Email,
                    fullName = u.FullName,
                    isActive = u.IsActive,
                    createdAt = u.CreatedAt
                })
                .ToList();

            return Ok(new
            {
                message = "Users listed successfully",
                data = new
                {
                    users = users,
                    page = page,
                    pageSize = pageSize,
                    total = allUsers.Count()
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error listing users", error = ex.Message });
        }
    }

    [HttpGet("users/{userId}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<object>> GetUserDetails(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return NotFound(new { message = "User not found" });

            var account = await _accountRepository.GetByUserIdAsync(userId);

            return Ok(new
            {
                message = "User details retrieved successfully",
                data = new
                {
                    id = user.Id,
                    email = user.Email,
                    fullName = user.FullName,
                    isActive = user.IsActive,
                    createdAt = user.CreatedAt,
                    account = account != null ? new
                    {
                        accountNumber = account.AccountNumber,
                        balance = account.Balance,
                        bankCode = account.BankCode
                    } : null
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving user details", error = ex.Message });
        }
    }

    [HttpGet("transactions")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<object>> ListTransactions([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var allTransactions = await _transactionRepository.GetAllAsync();
            var transactions = allTransactions
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new
                {
                    id = t.Id,
                    type = t.TransactionType,
                    amount = t.Amount,
                    status = t.Status,
                    description = t.Description,
                    createdAt = t.CreatedAt,
                    completedAt = t.CompletedAt
                })
                .ToList();

            return Ok(new
            {
                message = "Transactions listed successfully",
                data = new
                {
                    transactions = transactions,
                    page = page,
                    pageSize = pageSize,
                    total = allTransactions.Count()
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error listing transactions", error = ex.Message });
        }
    }

    [HttpGet("reports/transactions")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<object>> GetTransactionReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            var allTransactions = await _transactionRepository.GetAllAsync();
            var filteredTransactions = allTransactions
                .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate)
                .ToList();

            var report = new
            {
                period = new { start = startDate, end = endDate },
                totalTransactions = filteredTransactions.Count,
                totalAmount = filteredTransactions.Sum(t => t.Amount),
                byType = filteredTransactions
                    .GroupBy(t => t.TransactionType)
                    .Select(g => new
                    {
                        type = g.Key,
                        count = g.Count(),
                        amount = g.Sum(t => t.Amount)
                    })
                    .ToList(),
                byStatus = filteredTransactions
                    .GroupBy(t => t.Status)
                    .Select(g => new
                    {
                        status = g.Key,
                        count = g.Count(),
                        amount = g.Sum(t => t.Amount)
                    })
                    .ToList()
            };

            return Ok(new
            {
                message = "Report generated successfully",
                data = report
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error generating report", error = ex.Message });
        }
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult<object>> GetDashboard()
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            var allTransactions = (await _transactionRepository.GetAllAsync()).ToList();
            var allUsers = (await _userRepository.GetAllAsync()).ToList();

            var dashboard = new
            {
                userId = userId,
                role = role,
                stats = new
                {
                    totalTransactions = allTransactions.Count,
                    totalAmount = allTransactions.Sum(t => t.Amount),
                    pendingTransactions = allTransactions.Count(t => t.Status == "PENDING"),
                    activeUsers = allUsers.Count(u => u.IsActive)
                },
                recentTransactions = allTransactions
                    .OrderByDescending(t => t.CreatedAt)
                    .Take(5)
                    .Select(t => new
                    {
                        id = t.Id,
                        type = t.TransactionType,
                        amount = t.Amount,
                        status = t.Status,
                        date = t.CreatedAt
                    })
                    .ToList()
            };

            return Ok(new
            {
                message = "Dashboard loaded successfully",
                data = dashboard
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error loading dashboard", error = ex.Message });
        }
    }
}

/// <summary>
/// Request para criar novo usuário
/// </summary>
public class CreateUserRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? FullName { get; set; }
    public string? Document { get; set; }
    public string? PhoneNumber { get; set; }
}

