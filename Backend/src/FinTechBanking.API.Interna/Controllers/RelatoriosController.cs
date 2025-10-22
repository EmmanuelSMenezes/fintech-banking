using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinTechBanking.Core.Entities;
using FinTechBanking.Core.Interfaces;
using FinTechBanking.API.Interna.Attributes;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace FinTechBanking.API.Interna.Controllers;

/// <summary>
/// Controller para gerar relatórios e extratos
/// </summary>
[ApiController]
[Route("api/relatorios")]
[Authorize]
[RateLimit(maxRequests: 30, windowSeconds: 60)]
public class RelatoriosController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILogger<RelatoriosController> _logger;

    public RelatoriosController(
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        ILogger<RelatoriosController> logger)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _logger = logger;
    }

    static RelatoriosController()
    {
        // Configurar licença do EPPlus uma única vez
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    /// <summary>
    /// Gerar relatório de transações em Excel
    /// </summary>
    [HttpGet("transacoes-excel")]
    public async Task<IActionResult> GerarRelatorioTransacoesExcelAsync(
        [FromQuery] DateTime? dataInicio = null,
        [FromQuery] DateTime? dataFim = null,
        [FromQuery] string? tipoTransacao = null)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var account = await _accountRepository.GetByUserIdAsync(userId);

            if (account == null)
                return NotFound(new { message = "Conta não encontrada" });

            var allTransactions = (await _transactionRepository.GetAllAsync()).ToList();
            var transactions = allTransactions
                .Where(t => t.AccountId == account.Id)
                .Where(t => !dataInicio.HasValue || t.CreatedAt >= dataInicio.Value)
                .Where(t => !dataFim.HasValue || t.CreatedAt <= dataFim.Value)
                .Where(t => string.IsNullOrEmpty(tipoTransacao) || t.TransactionType == tipoTransacao)
                .OrderByDescending(t => t.CreatedAt)
                .ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Transações");

                // Headers
                worksheet.Cells[1, 1].Value = "Data";
                worksheet.Cells[1, 2].Value = "Tipo";
                worksheet.Cells[1, 3].Value = "Valor";
                worksheet.Cells[1, 4].Value = "Descrição";
                worksheet.Cells[1, 5].Value = "Status";
                worksheet.Cells[1, 6].Value = "Chave Destinatário";

                // Estilo do header
                for (int col = 1; col <= 6; col++)
                {
                    worksheet.Cells[1, col].Style.Font.Bold = true;
                    worksheet.Cells[1, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Dados
                int row = 2;
                foreach (var transaction in transactions)
                {
                    worksheet.Cells[row, 1].Value = transaction.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss");
                    worksheet.Cells[row, 2].Value = transaction.TransactionType;
                    worksheet.Cells[row, 3].Value = transaction.Amount;
                    worksheet.Cells[row, 4].Value = transaction.Description ?? "-";
                    worksheet.Cells[row, 5].Value = transaction.Status;
                    worksheet.Cells[row, 6].Value = transaction.RecipientKey ?? "-";
                    row++;
                }

                // Auto-fit columns
                worksheet.Columns[1].AutoFit();
                worksheet.Columns[2].AutoFit();
                worksheet.Columns[3].AutoFit();
                worksheet.Columns[4].AutoFit();
                worksheet.Columns[5].AutoFit();
                worksheet.Columns[6].AutoFit();

                var excelBytes = package.GetAsByteArray();
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                    $"relatorio_transacoes_{account.AccountNumber}_{DateTime.Now:yyyyMMdd}.xlsx");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar relatório Excel");
            return StatusCode(500, new { message = "Erro ao gerar relatório", error = ex.Message });
        }
    }

    /// <summary>
    /// Obter resumo de transações
    /// </summary>
    [HttpGet("resumo")]
    public async Task<ActionResult<object>> GetResumoAsync(
        [FromQuery] DateTime? dataInicio = null,
        [FromQuery] DateTime? dataFim = null)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var account = await _accountRepository.GetByUserIdAsync(userId);

            if (account == null)
                return NotFound(new { message = "Conta não encontrada" });

            var allTransactions = (await _transactionRepository.GetAllAsync()).ToList();
            var transactions = allTransactions
                .Where(t => t.AccountId == account.Id)
                .Where(t => !dataInicio.HasValue || t.CreatedAt >= dataInicio.Value)
                .Where(t => !dataFim.HasValue || t.CreatedAt <= dataFim.Value)
                .ToList();

            var totalEntradas = transactions
                .Where(t => t.TransactionType == "TRANSFER_RECEIVED" || t.TransactionType == "DEPOSIT")
                .Sum(t => t.Amount);

            var totalSaidas = transactions
                .Where(t => t.TransactionType == "TRANSFER" || t.TransactionType == "WITHDRAWAL")
                .Sum(t => t.Amount);

            return Ok(new
            {
                message = "Resumo de transações",
                data = new
                {
                    saldoAtual = account.Balance,
                    totalTransacoes = transactions.Count,
                    totalEntradas = totalEntradas,
                    totalSaidas = totalSaidas,
                    saldoLiquido = totalEntradas - totalSaidas,
                    periodo = new
                    {
                        inicio = dataInicio?.ToString("dd/MM/yyyy") ?? "Início",
                        fim = dataFim?.ToString("dd/MM/yyyy") ?? "Hoje"
                    }
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar resumo");
            return StatusCode(500, new { message = "Erro ao gerar resumo", error = ex.Message });
        }
    }
}

