using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinTechBanking.Core.DTOs;
using FinTechBanking.Core.Interfaces;
using FinTechBanking.API.Interna.Attributes;

namespace FinTechBanking.API.Interna.Controllers;

/// <summary>
/// Controller para gerenciar PIX Dinâmico
/// </summary>
[ApiController]
[Route("api/pix")]
[Authorize]
[RateLimit(maxRequests: 100, windowSeconds: 60)]
public class PixController : ControllerBase
{
    private readonly IPixService _pixService;
    private readonly ILogger<PixController> _logger;

    public PixController(
        IPixService pixService,
        ILogger<PixController> logger)
    {
        _pixService = pixService;
        _logger = logger;
    }

    /// <summary>
    /// Criar um novo PIX dinâmico
    /// </summary>
    [HttpPost("criar-dinamico")]
    public async Task<ActionResult<object>> CriarPixDinamicoAsync([FromBody] CriarPixDinamicoRequest request)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());

            // Validações
            if (request.Amount <= 0)
                return BadRequest(new { message = "Valor deve ser maior que zero" });

            if (string.IsNullOrWhiteSpace(request.Description))
                return BadRequest(new { message = "Descrição é obrigatória" });

            if (string.IsNullOrWhiteSpace(request.RecipientKey))
                return BadRequest(new { message = "Chave PIX é obrigatória" });

            // Criar PIX dinâmico
            var pixDinamico = await _pixService.CriarPixDinamicoAsync(
                userId,
                request.Amount,
                request.Description,
                request.RecipientKey);

            _logger.LogInformation($"PIX dinâmico criado: {pixDinamico.Id} para usuário {userId}");

            var response = new CriarPixDinamicoResponse
            {
                PixId = pixDinamico.Id,
                Amount = pixDinamico.Amount,
                Description = pixDinamico.Description,
                QrCode = pixDinamico.QrCodeData,
                QrCodeUrl = pixDinamico.QrCodeUrl,
                Status = pixDinamico.Status,
                CreatedAt = pixDinamico.CreatedAt,
                ExpiresAt = pixDinamico.ExpiresAt
            };

            return Ok(new
            {
                message = "PIX dinâmico criado com sucesso",
                data = response
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning($"Erro de validação ao criar PIX: {ex.Message}");
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Erro ao criar PIX: {ex.Message}");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao criar PIX dinâmico: {ex.Message}");
            return StatusCode(500, new { message = "Erro ao criar PIX dinâmico", error = ex.Message });
        }
    }

    /// <summary>
    /// Obter status de um PIX dinâmico
    /// </summary>
    [HttpGet("status/{pixId}")]
    public async Task<ActionResult<object>> ObterStatusAsync(Guid pixId)
    {
        try
        {
            var pixDinamico = await _pixService.ObterStatusAsync(pixId);
            if (pixDinamico == null)
                return NotFound(new { message = "PIX dinâmico não encontrado" });

            var response = new PixDinamicoStatusResponse
            {
                PixId = pixDinamico.Id,
                Amount = pixDinamico.Amount,
                Description = pixDinamico.Description,
                Status = pixDinamico.Status,
                CreatedAt = pixDinamico.CreatedAt,
                ExpiresAt = pixDinamico.ExpiresAt,
                PaidAt = pixDinamico.PaidAt
            };

            return Ok(new
            {
                message = "Status obtido com sucesso",
                data = response
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao obter status do PIX: {ex.Message}");
            return StatusCode(500, new { message = "Erro ao obter status", error = ex.Message });
        }
    }

    /// <summary>
    /// Confirmar pagamento de um PIX dinâmico
    /// </summary>
    [HttpPost("confirmar/{pixId}")]
    public async Task<ActionResult<object>> ConfirmarPagamentoAsync(Guid pixId)
    {
        try
        {
            var pixDinamico = await _pixService.ConfirmarPagamentoAsync(pixId);

            _logger.LogInformation($"PIX dinâmico confirmado: {pixId}");

            var response = new ConfirmarPagamentoResponse
            {
                PixId = pixDinamico.Id,
                Status = pixDinamico.Status,
                PaidAt = pixDinamico.PaidAt ?? DateTime.UtcNow,
                Amount = pixDinamico.Amount
            };

            return Ok(new
            {
                message = "Pagamento confirmado com sucesso",
                data = response
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Erro ao confirmar PIX: {ex.Message}");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao confirmar pagamento: {ex.Message}");
            return StatusCode(500, new { message = "Erro ao confirmar pagamento", error = ex.Message });
        }
    }

    /// <summary>
    /// Listar PIX dinâmicos do usuário
    /// </summary>
    [HttpGet("listar")]
    public async Task<ActionResult<object>> ListarPixDinamicosAsync()
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());

            var pixDinamicos = await _pixService.ListarPixDinamicosAsync(userId);

            var items = pixDinamicos.Select(p => new PixDinamicoItemResponse
            {
                PixId = p.Id,
                Amount = p.Amount,
                Description = p.Description,
                Status = p.Status,
                CreatedAt = p.CreatedAt,
                PaidAt = p.PaidAt
            }).ToList();

            var response = new ListarPixDinamicosResponse
            {
                Items = items,
                Total = items.Count
            };

            return Ok(new
            {
                message = "PIX dinâmicos listados com sucesso",
                data = response
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao listar PIX dinâmicos: {ex.Message}");
            return StatusCode(500, new { message = "Erro ao listar PIX dinâmicos", error = ex.Message });
        }
    }

    /// <summary>
    /// Cancelar um PIX dinâmico
    /// </summary>
    [HttpPost("cancelar/{pixId}")]
    public async Task<ActionResult<object>> CancelarPixAsync(Guid pixId)
    {
        try
        {
            var pixDinamico = await _pixService.CancelarPixAsync(pixId);

            _logger.LogInformation($"PIX dinâmico cancelado: {pixId}");

            return Ok(new
            {
                message = "PIX dinâmico cancelado com sucesso",
                data = new { pixId = pixDinamico.Id, status = pixDinamico.Status }
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Erro ao cancelar PIX: {ex.Message}");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao cancelar PIX: {ex.Message}");
            return StatusCode(500, new { message = "Erro ao cancelar PIX", error = ex.Message });
        }
    }
}

