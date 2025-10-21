using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FinTechBanking.Core.DTOs;
using FinTechBanking.Core.Interfaces;

namespace FinTechBanking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountsController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly IBankingHub _bankingHub;

    public AccountsController(IAccountRepository accountRepository, IBankingHub bankingHub)
    {
        _accountRepository = accountRepository;
        _bankingHub = bankingHub;
    }

    [HttpGet("balance")]
    public async Task<ActionResult<AccountBalanceResponse>> GetBalance()
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var account = await _accountRepository.GetByUserIdAsync(userId);

            if (account == null)
                return NotFound(new { message = "Account not found" });

            return Ok(new AccountBalanceResponse
            {
                AccountId = account.Id,
                Balance = account.Balance,
                BankCode = account.BankCode
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving balance", error = ex.Message });
        }
    }
}

