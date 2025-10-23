using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FinTechBanking.Core.Entities;
using FinTechBanking.Core.Interfaces;

namespace FinTechBanking.API.Cliente.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClienteController : ControllerBase
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBankingHub _bankingHub;
    private readonly IMessageBroker _messageBroker;

    public ClienteController(
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository,
        IUserRepository userRepository,
        IBankingHub bankingHub,
        IMessageBroker messageBroker)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _userRepository = userRepository;
        _bankingHub = bankingHub;
        _messageBroker = messageBroker;
    }

    /// <summary>
    /// Obter saldo da conta do cliente
    /// </summary>
    [HttpGet("saldo")]
    public async Task<ActionResult<object>> GetSaldo()
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var account = await _accountRepository.GetByUserIdAsync(userId);

            if (account == null)
                return NotFound(new { message = "Account not found" });

            return Ok(new
            {
                message = "Saldo retrieved successfully",
                data = new
                {
                    total = account.Balance,
                    accountNumber = account.AccountNumber,
                    bankCode = account.BankCode
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving saldo", error = ex.Message });
        }
    }

    /// <summary>
    /// Listar transações do cliente
    /// </summary>
    [HttpGet("transacoes")]
    public async Task<ActionResult<object>> GetTransacoes()
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var account = await _accountRepository.GetByUserIdAsync(userId);

            if (account == null)
                return NotFound(new { message = "Account not found" });

            var transactions = await _transactionRepository.GetByAccountIdAsync(account.Id);

            return Ok(new
            {
                message = "Transações retrieved successfully",
                data = transactions.Select(t => new
                {
                    id = t.Id,
                    type = t.TransactionType,
                    amount = t.Amount,
                    status = t.Status,
                    description = t.Description,
                    date = t.CreatedAt
                }).ToList()
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving transações", error = ex.Message });
        }
    }

    /// <summary>
    /// Obter perfil do cliente
    /// </summary>
    [HttpGet("perfil")]
    public async Task<ActionResult<object>> GetPerfil()
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(new
            {
                message = "Perfil retrieved successfully",
                data = new
                {
                    id = user.Id,
                    email = user.Email,
                    fullName = user.FullName,
                    document = user.Document,
                    phoneNumber = user.PhoneNumber,
                    isActive = user.IsActive
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving perfil", error = ex.Message });
        }
    }

    /// <summary>
    /// Atualizar perfil do cliente
    /// </summary>
    [HttpPut("perfil")]
    public async Task<ActionResult<object>> UpdatePerfil([FromBody] UpdatePerfilRequest request)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return NotFound(new { message = "User not found" });

            if (!string.IsNullOrEmpty(request.FullName))
                user.FullName = request.FullName;

            if (!string.IsNullOrEmpty(request.PhoneNumber))
                user.PhoneNumber = request.PhoneNumber;

            if (!string.IsNullOrEmpty(request.Document))
                user.Document = request.Document;

            await _userRepository.UpdateAsync(user);

            return Ok(new
            {
                message = "Perfil updated successfully",
                data = new
                {
                    id = user.Id,
                    email = user.Email,
                    fullName = user.FullName,
                    phoneNumber = user.PhoneNumber
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error updating perfil", error = ex.Message });
        }
    }

    /// <summary>
    /// Gerar cobrança PIX (QR Code Dinâmico)
    /// </summary>
    [HttpPost("cobrancas")]
    public async Task<ActionResult<object>> CreateCobranca([FromBody] CreateCobrancaRequest request)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var account = await _accountRepository.GetByUserIdAsync(userId);

            if (account == null)
                return NotFound(new { message = "Account not found" });

            // Generate QR Code via Banking Hub
            var qrCode = await _bankingHub.GeneratePixQrCodeAsync(
                account.BankCode,
                request.Amount,
                account.AccountNumber,
                request.Description ?? "Cobrança PIX"
            );

            // Create transaction record
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                TransactionType = "PIX_COBRANCA",
                Amount = request.Amount,
                Status = "PENDING",
                QrCodeData = qrCode,
                RecipientKey = account.AccountNumber,
                Description = request.Description ?? "Cobrança PIX",
                CreatedAt = DateTime.UtcNow
            };

            var createdTransaction = await _transactionRepository.CreateAsync(transaction);

            // Publish to message queue for processing
            await _messageBroker.PublishAsync("pix-requests", new
            {
                TransactionId = createdTransaction.Id,
                Amount = createdTransaction.Amount,
                RecipientKey = createdTransaction.RecipientKey,
                BankCode = account.BankCode
            });

            return Ok(new
            {
                message = "Cobrança created successfully",
                data = new
                {
                    transactionId = createdTransaction.Id,
                    qrCode = qrCode,
                    pixKey = account.AccountNumber,
                    amount = createdTransaction.Amount,
                    status = createdTransaction.Status
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error creating cobrança", error = ex.Message });
        }
    }

    /// <summary>
    /// Listar saques do cliente
    /// </summary>
    [HttpGet("saques")]
    public async Task<ActionResult<object>> ListSaques()
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var account = await _accountRepository.GetByUserIdAsync(userId);

            if (account == null)
                return NotFound(new { message = "Account not found" });

            var saques = await _transactionRepository.GetByAccountIdAsync(account.Id);
            var saquesList = saques
                .Where(t => t.TransactionType == "WITHDRAWAL")
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new
                {
                    id = t.Id,
                    amount = t.Amount,
                    status = t.Status,
                    description = t.Description,
                    date = t.CreatedAt
                })
                .ToList();

            return Ok(new
            {
                message = "Saques retrieved successfully",
                data = saquesList
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving saques", error = ex.Message });
        }
    }

    /// <summary>
    /// Solicitar novo saque
    /// </summary>
    [HttpPost("saques")]
    public async Task<ActionResult<object>> CreateSaque([FromBody] CreateSaqueRequest request)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var account = await _accountRepository.GetByUserIdAsync(userId);

            if (account == null)
                return NotFound(new { message = "Account not found" });

            if (account.Balance < request.Amount)
                return BadRequest(new { message = "Insufficient balance" });

            // Create transaction record
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                TransactionType = "WITHDRAWAL",
                Amount = request.Amount,
                Status = "PENDING",
                Description = request.Description ?? "Saque",
                CreatedAt = DateTime.UtcNow
            };

            var createdTransaction = await _transactionRepository.CreateAsync(transaction);

            // Publish to message queue for processing
            await _messageBroker.PublishAsync("withdrawal-requests", new
            {
                TransactionId = createdTransaction.Id,
                Amount = createdTransaction.Amount,
                AccountNumber = account.AccountNumber,
                BankCode = account.BankCode
            });

            return Ok(new
            {
                message = "Saque created successfully",
                data = new
                {
                    transactionId = createdTransaction.Id,
                    amount = createdTransaction.Amount,
                    status = createdTransaction.Status
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error creating saque", error = ex.Message });
        }
    }
}

public class UpdatePerfilRequest
{
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Document { get; set; }
}

public class CreateCobrancaRequest
{
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}

public class CreateSaqueRequest
{
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}

