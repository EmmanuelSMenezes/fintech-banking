using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinTechBanking.Core.Entities;
using FinTechBanking.Core.Interfaces;
using FinTechBanking.API.Interna.Attributes;

namespace FinTechBanking.API.Interna.Controllers;

/// <summary>
/// Controller para gerenciar transferências bancárias
/// </summary>
[ApiController]
[Route("api/transferencias")]
[Authorize]
[RateLimit(maxRequests: 50, windowSeconds: 60)]
public class TransferenciasController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMessageBroker _messageBroker;
    private readonly ILogger<TransferenciasController> _logger;

    public TransferenciasController(
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        IMessageBroker messageBroker,
        ILogger<TransferenciasController> logger)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _messageBroker = messageBroker;
        _logger = logger;
    }

    /// <summary>
    /// Realizar transferência entre contas
    /// </summary>
    [HttpPost("transferir")]
    public async Task<ActionResult<object>> TransferirAsync([FromBody] TransferRequest request)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var senderAccount = await _accountRepository.GetByUserIdAsync(userId);

            if (senderAccount == null)
                return NotFound(new { message = "Conta do remetente não encontrada" });

            // Validações
            if (request.Amount <= 0)
                return BadRequest(new { message = "Valor deve ser maior que zero" });

            if (senderAccount.Balance < request.Amount)
                return BadRequest(new { message = "Saldo insuficiente" });

            if (string.IsNullOrWhiteSpace(request.RecipientAccountNumber))
                return BadRequest(new { message = "Número da conta do destinatário é obrigatório" });

            // Buscar conta do destinatário
            var recipientAccount = await _accountRepository.GetByAccountNumberAsync(request.RecipientAccountNumber);
            if (recipientAccount == null)
                return NotFound(new { message = "Conta do destinatário não encontrada" });

            if (!recipientAccount.IsActive)
                return BadRequest(new { message = "Conta do destinatário está inativa" });

            if (senderAccount.Id == recipientAccount.Id)
                return BadRequest(new { message = "Não é possível transferir para a mesma conta" });

            // Criar transação de transferência
            var transferTransaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = senderAccount.Id,
                UserId = userId,
                TransactionType = "TRANSFER",
                Amount = request.Amount,
                Status = "PENDING",
                Description = request.Description ?? "Transferência bancária",
                RecipientKey = recipientAccount.AccountNumber,
                CreatedAt = DateTime.UtcNow
            };

            var createdTransaction = await _transactionRepository.CreateAsync(transferTransaction);

            // Atualizar saldo do remetente (débito)
            senderAccount.Balance -= request.Amount;
            senderAccount.UpdatedAt = DateTime.UtcNow;
            await _accountRepository.UpdateAsync(senderAccount);

            // Atualizar saldo do destinatário (crédito)
            recipientAccount.Balance += request.Amount;
            recipientAccount.UpdatedAt = DateTime.UtcNow;
            await _accountRepository.UpdateAsync(recipientAccount);

            // Criar transação de crédito para o destinatário
            var creditTransaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = recipientAccount.Id,
                UserId = recipientAccount.UserId,
                TransactionType = "TRANSFER_RECEIVED",
                Amount = request.Amount,
                Status = "COMPLETED",
                Description = $"Transferência recebida de {senderAccount.AccountNumber}",
                RecipientKey = senderAccount.AccountNumber,
                CreatedAt = DateTime.UtcNow
            };

            await _transactionRepository.CreateAsync(creditTransaction);

            // Atualizar status da transação original
            createdTransaction.Status = "COMPLETED";
            createdTransaction.UpdatedAt = DateTime.UtcNow;
            await _transactionRepository.UpdateAsync(createdTransaction);

            // Publicar evento para fila de mensagens
            await _messageBroker.PublishAsync("transfer-completed", new
            {
                TransactionId = createdTransaction.Id,
                SenderId = userId,
                RecipientId = recipientAccount.UserId,
                Amount = request.Amount,
                SenderAccount = senderAccount.AccountNumber,
                RecipientAccount = recipientAccount.AccountNumber,
                Timestamp = DateTime.UtcNow
            });

            _logger.LogInformation("Transferência realizada: {TransactionId} de {Sender} para {Recipient}", 
                createdTransaction.Id, senderAccount.AccountNumber, recipientAccount.AccountNumber);

            return Ok(new
            {
                message = "Transferência realizada com sucesso",
                data = new
                {
                    transactionId = createdTransaction.Id,
                    amount = request.Amount,
                    senderAccount = senderAccount.AccountNumber,
                    recipientAccount = recipientAccount.AccountNumber,
                    status = createdTransaction.Status,
                    timestamp = createdTransaction.CreatedAt,
                    description = request.Description
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao realizar transferência");
            return StatusCode(500, new { message = "Erro ao realizar transferência", error = ex.Message });
        }
    }

    /// <summary>
    /// Obter histórico de transferências do usuário
    /// </summary>
    [HttpGet("historico")]
    public async Task<ActionResult<object>> GetHistoricoTransferenciasAsync([FromQuery] int page = 1, [FromQuery] int limit = 10)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var allTransactions = (await _transactionRepository.GetAllAsync()).ToList();

            var transfers = allTransactions
                .Where(t => t.UserId == userId && (t.TransactionType == "TRANSFER" || t.TransactionType == "TRANSFER_RECEIVED"))
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList();

            return Ok(new
            {
                message = "Histórico de transferências carregado com sucesso",
                data = transfers.Select(t => new
                {
                    id = t.Id,
                    type = t.TransactionType,
                    amount = t.Amount,
                    status = t.Status,
                    description = t.Description,
                    recipientKey = t.RecipientKey,
                    createdAt = t.CreatedAt
                }),
                pagination = new
                {
                    page,
                    limit,
                    total = allTransactions.Count(t => t.UserId == userId && (t.TransactionType == "TRANSFER" || t.TransactionType == "TRANSFER_RECEIVED"))
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar histórico de transferências");
            return StatusCode(500, new { message = "Erro ao carregar histórico", error = ex.Message });
        }
    }
}

/// <summary>
/// Request para transferência
/// </summary>
public class TransferRequest
{
    public string RecipientAccountNumber { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}

