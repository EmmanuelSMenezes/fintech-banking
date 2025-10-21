using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FinTechBanking.Core.Interfaces;
using FinTechBanking.Core.Entities;

namespace FinTechBanking.API.Interna.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
}

public class UpdatePerfilRequest
{
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
}

