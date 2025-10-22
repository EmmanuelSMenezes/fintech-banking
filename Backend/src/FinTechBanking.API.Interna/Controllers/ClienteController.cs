using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FinTechBanking.Core.Interfaces;
using FinTechBanking.Core.Entities;
using FinTechBanking.API.Interna.Attributes;

namespace FinTechBanking.API.Interna.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[RateLimit(maxRequests: 200, windowSeconds: 60)]
public class ClienteController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILogger<ClienteController> _logger;

    public ClienteController(
        IUserRepository userRepository,
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        ILogger<ClienteController> logger)
    {
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _logger = logger;
    }

    /// <summary>
    /// Obter perfil do cliente autenticado
    /// </summary>
    [HttpGet("perfil")]
    public async Task<ActionResult<object>> GetPerfil()
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return NotFound(new { message = "Usuário não encontrado" });

            return Ok(new
            {
                message = "Perfil carregado com sucesso",
                data = new
                {
                    id = user.Id,
                    email = user.Email,
                    name = user.FullName,
                    document = user.Document,
                    phoneNumber = user.PhoneNumber,
                    createdAt = user.CreatedAt
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar perfil");
            return StatusCode(500, new { message = "Erro ao carregar perfil", error = ex.Message });
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
                return NotFound(new { message = "Usuário não encontrado" });

            // Atualizar dados
            if (!string.IsNullOrWhiteSpace(request.FullName))
                user.FullName = request.FullName;

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                user.PhoneNumber = request.PhoneNumber;

            await _userRepository.UpdateAsync(user);

            _logger.LogInformation("Perfil atualizado: {UserId}", userId);

            return Ok(new
            {
                message = "Perfil atualizado com sucesso",
                data = new
                {
                    id = user.Id,
                    email = user.Email,
                    name = user.FullName,
                    phoneNumber = user.PhoneNumber
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar perfil");
            return StatusCode(500, new { message = "Erro ao atualizar perfil", error = ex.Message });
        }
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
                return NotFound(new { message = "Conta não encontrada" });

            return Ok(new
            {
                message = "Saldo carregado com sucesso",
                data = new
                {
                    total = account.Balance,
                    disponivel = account.Balance,
                    bloqueado = 0,
                    moeda = "BRL"
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar saldo");
            return StatusCode(500, new { message = "Erro ao carregar saldo", error = ex.Message });
        }
    }

    /// <summary>
    /// Obter histórico de transações do cliente
    /// </summary>
    [HttpGet("transacoes")]
    public async Task<ActionResult<object>> GetTransacoes([FromQuery] int page = 1, [FromQuery] int limit = 10)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var allTransactions = (await _transactionRepository.GetAllAsync()).ToList();
            
            // Filtrar transações do usuário
            var userTransactions = allTransactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList();

            return Ok(new
            {
                message = "Transações carregadas com sucesso",
                data = userTransactions.Select(t => new
                {
                    id = t.Id,
                    tipo = t.TransactionType,
                    valor = t.Amount,
                    status = t.Status,
                    data = t.CreatedAt,
                    descricao = t.Description ?? ""
                }).ToList(),
                pagination = new
                {
                    page = page,
                    limit = limit,
                    total = allTransactions.Count(t => t.UserId == userId)
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar transações");
            return StatusCode(500, new { message = "Erro ao carregar transações", error = ex.Message });
        }
    }

    /// <summary>
    /// Obter transação específica
    /// </summary>
    [HttpGet("transacoes/{id}")]
    public async Task<ActionResult<object>> GetTransacao(string id)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var transaction = await _transactionRepository.GetByIdAsync(Guid.Parse(id));

            if (transaction == null || transaction.UserId != userId)
                return NotFound(new { message = "Transação não encontrada" });

            return Ok(new
            {
                message = "Transação carregada com sucesso",
                data = new
                {
                    id = transaction.Id,
                    tipo = transaction.TransactionType,
                    valor = transaction.Amount,
                    status = transaction.Status,
                    data = transaction.CreatedAt,
                    descricao = transaction.Description
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar transação");
            return StatusCode(500, new { message = "Erro ao carregar transação", error = ex.Message });
        }
    }

    /// <summary>
    /// Gerar cobrança/PIX
    /// </summary>
    [HttpPost("cobrancas")]
    public async Task<ActionResult<object>> CreateCobranca([FromBody] CreateCobrancaRequest request)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var account = await _accountRepository.GetByUserIdAsync(userId);

            if (account == null)
                return NotFound(new { message = "Conta não encontrada" });

            // Criar transação de cobrança
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                UserId = userId,
                TransactionType = "PIX",
                Amount = request.Amount,
                Status = "PENDING",
                Description = request.Description ?? "Cobrança PIX",
                CreatedAt = DateTime.UtcNow
            };

            await _transactionRepository.CreateAsync(transaction);

            _logger.LogInformation("Cobrança criada: {TransactionId}", transaction.Id);

            return Ok(new
            {
                message = "Cobrança criada com sucesso",
                data = new
                {
                    id = transaction.Id,
                    qrCode = $"00020126580014br.gov.bcb.pix0136{transaction.Id}520400005303986540510.005802BR5913OWAYPAY6009SAO PAULO62410503***63041D3D",
                    pixKey = "owaypay@pix",
                    amount = transaction.Amount,
                    status = transaction.Status
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar cobrança");
            return StatusCode(500, new { message = "Erro ao criar cobrança", error = ex.Message });
        }
    }

    /// <summary>
    /// Solicitar saque
    /// </summary>
    [HttpPost("saques")]
    public async Task<ActionResult<object>> RequestWithdrawal([FromBody] RequestWithdrawalRequest request)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var account = await _accountRepository.GetByUserIdAsync(userId);

            if (account == null)
                return NotFound(new { message = "Conta não encontrada" });

            if (account.Balance < request.Amount)
                return BadRequest(new { message = "Saldo insuficiente" });

            // Criar transação de saque
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                UserId = userId,
                TransactionType = "WITHDRAWAL",
                Amount = request.Amount,
                Status = "PENDING",
                Description = request.Description ?? "Saque solicitado",
                CreatedAt = DateTime.UtcNow
            };

            await _transactionRepository.CreateAsync(transaction);

            _logger.LogInformation("Saque solicitado: {TransactionId}", transaction.Id);

            return Ok(new
            {
                message = "Saque solicitado com sucesso",
                data = new
                {
                    id = transaction.Id,
                    amount = transaction.Amount,
                    status = transaction.Status,
                    createdAt = transaction.CreatedAt
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao solicitar saque");
            return StatusCode(500, new { message = "Erro ao solicitar saque", error = ex.Message });
        }
    }
}

public class UpdatePerfilRequest
{
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
}

public class CreateCobrancaRequest
{
    public long Amount { get; set; }
    public string? Description { get; set; }
    public string? PixKey { get; set; }
}

public class RequestWithdrawalRequest
{
    public long Amount { get; set; }
    public string? Description { get; set; }
}

