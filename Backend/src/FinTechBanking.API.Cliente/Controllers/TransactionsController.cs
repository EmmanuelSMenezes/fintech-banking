using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FinTechBanking.Core.DTOs;
using FinTechBanking.Core.Entities;
using FinTechBanking.Core.Interfaces;

namespace FinTechBanking.API.Cliente.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IBankingHub _bankingHub;
    private readonly IMessageBroker _messageBroker;

    public TransactionsController(
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository,
        IBankingHub bankingHub,
        IMessageBroker messageBroker)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _bankingHub = bankingHub;
        _messageBroker = messageBroker;
    }

    [HttpPost("pix-qrcode")]
    public async Task<ActionResult<CreatePixQrCodeResponse>> CreatePixQrCode([FromBody] CreatePixQrCodeRequest request)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var account = await _accountRepository.GetByUserIdAsync(userId);

            if (account == null)
                return NotFound(new { message = "Account not found" });

            // Generate QR Code via Banking Hub
            var qrCode = await _bankingHub.GeneratePixQrCodeAsync(account.BankCode, request.Amount, request.RecipientKey, request.Description);

            // Create transaction record
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                TransactionType = "PIX_QR_CODE",
                Amount = request.Amount,
                Status = "PENDING",
                QrCodeData = qrCode,
                RecipientKey = request.RecipientKey,
                Description = request.Description,
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

            return Ok(new CreatePixQrCodeResponse
            {
                TransactionId = createdTransaction.Id,
                QrCode = qrCode,
                QrCodeUrl = $"https://api.example.com/qrcode/{createdTransaction.Id}",
                Amount = createdTransaction.Amount,
                Status = createdTransaction.Status
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error creating PIX QR Code", error = ex.Message });
        }
    }



    [HttpPost("withdrawal")]
    public async Task<ActionResult<WithdrawalResponse>> RequestWithdrawal([FromBody] WithdrawalRequest request)
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
                Description = request.Description,
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

            return Ok(new WithdrawalResponse
            {
                TransactionId = createdTransaction.Id,
                Amount = createdTransaction.Amount,
                Status = createdTransaction.Status
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error requesting withdrawal", error = ex.Message });
        }
    }

    [HttpGet("balance")]
    public async Task<ActionResult<object>> GetBalance()
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var account = await _accountRepository.GetByUserIdAsync(userId);

            if (account == null)
                return NotFound(new { message = "Account not found" });

            return Ok(new
            {
                balance = account.Balance,
                accountNumber = account.AccountNumber,
                bankCode = account.BankCode
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving balance", error = ex.Message });
        }
    }

    [HttpGet("history")]
    public async Task<ActionResult<object>> GetTransactionHistory()
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var account = await _accountRepository.GetByUserIdAsync(userId);

            if (account == null)
                return NotFound(new { message = "Account not found" });

            // Get all transactions for this account
            var transactions = await _transactionRepository.GetByAccountIdAsync(account.Id);

            return Ok(new
            {
                transactions = transactions.Select(t => new
                {
                    id = t.Id,
                    transactionType = t.TransactionType,
                    type = t.TransactionType,
                    amount = t.Amount,
                    status = t.Status,
                    description = t.Description,
                    createdAt = t.CreatedAt,
                    completedAt = t.CompletedAt
                }).ToList()
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving transaction history", error = ex.Message });
        }
    }

    [HttpGet("{transactionId}")]
    public async Task<ActionResult<TransactionStatusResponse>> GetTransactionStatus(Guid transactionId)
    {
        try
        {
            var transaction = await _transactionRepository.GetByIdAsync(transactionId);
            if (transaction == null)
                return NotFound(new { message = "Transaction not found" });

            return Ok(new TransactionStatusResponse
            {
                TransactionId = transaction.Id,
                TransactionType = transaction.TransactionType,
                Amount = transaction.Amount,
                Status = transaction.Status,
                CreatedAt = transaction.CreatedAt,
                CompletedAt = transaction.CompletedAt
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving transaction", error = ex.Message });
        }
    }
}

public class GeneratePixQrCodeRequest
{
    public decimal Amount { get; set; }
    public string Description { get; set; }
}

public class WithdrawalRequest
{
    public decimal Amount { get; set; }
    public string Description { get; set; }
}

