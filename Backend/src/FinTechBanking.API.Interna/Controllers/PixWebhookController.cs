using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinTechBanking.Core.DTOs;
using FinTechBanking.Core.Interfaces;
using FinTechBanking.API.Interna.Attributes;

namespace FinTechBanking.API.Interna.Controllers;

/// <summary>
/// Controller para gerenciar Webhooks de PIX
/// </summary>
[ApiController]
[Route("api/pix-webhooks")]
[Authorize]
[RateLimit(maxRequests: 100, windowSeconds: 60)]
public class PixWebhookController : ControllerBase
{
    private readonly IPixWebhookService _webhookService;
    private readonly ILogger<PixWebhookController> _logger;

    public PixWebhookController(
        IPixWebhookService webhookService,
        ILogger<PixWebhookController> logger)
    {
        _webhookService = webhookService;
        _logger = logger;
    }

    /// <summary>
    /// Registrar um novo webhook
    /// </summary>
    [HttpPost("registrar")]
    public async Task<ActionResult<object>> RegistrarWebhookAsync([FromBody] RegistrarWebhookRequest request)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());

            // Validações
            if (string.IsNullOrWhiteSpace(request.EventType))
                return BadRequest(new { message = "Tipo de evento é obrigatório" });

            if (string.IsNullOrWhiteSpace(request.WebhookUrl))
                return BadRequest(new { message = "URL do webhook é obrigatória" });

            var webhook = await _webhookService.RegistrarWebhookAsync(userId, request.EventType, request.WebhookUrl);

            _logger.LogInformation($"Webhook registrado: {webhook.Id} para usuário {userId}");

            var response = new PixWebhookResponse
            {
                Id = webhook.Id,
                EventType = webhook.EventType,
                WebhookUrl = webhook.WebhookUrl,
                IsActive = webhook.IsActive,
                RetryCount = webhook.RetryCount,
                LastAttempt = webhook.LastAttempt,
                CreatedAt = webhook.CreatedAt,
                UpdatedAt = webhook.UpdatedAt
            };

            return Ok(new { message = "Webhook registrado com sucesso", data = response });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning($"Erro ao registrar webhook: {ex.Message}");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao registrar webhook: {ex.Message}");
            return StatusCode(500, new { message = "Erro ao registrar webhook" });
        }
    }

    /// <summary>
    /// Listar webhooks do usuário
    /// </summary>
    [HttpGet("listar")]
    public async Task<ActionResult<object>> ListarWebhooksAsync()
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());

            var webhooks = await _webhookService.ListarWebhooksAsync(userId);

            var response = new ListarWebhooksResponse
            {
                Webhooks = webhooks.Select(w => new PixWebhookResponse
                {
                    Id = w.Id,
                    EventType = w.EventType,
                    WebhookUrl = w.WebhookUrl,
                    IsActive = w.IsActive,
                    RetryCount = w.RetryCount,
                    LastAttempt = w.LastAttempt,
                    CreatedAt = w.CreatedAt,
                    UpdatedAt = w.UpdatedAt
                }).ToList(),
                Total = webhooks.Count
            };

            return Ok(new { message = "Webhooks listados com sucesso", data = response });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao listar webhooks: {ex.Message}");
            return StatusCode(500, new { message = "Erro ao listar webhooks" });
        }
    }

    /// <summary>
    /// Deletar um webhook
    /// </summary>
    [HttpDelete("deletar/{webhookId}")]
    public async Task<ActionResult<object>> DeletarWebhookAsync(Guid webhookId)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());

            var result = await _webhookService.DeletarWebhookAsync(webhookId, userId);

            if (!result)
                return NotFound(new { message = "Webhook não encontrado" });

            _logger.LogInformation($"Webhook deletado: {webhookId} para usuário {userId}");

            return Ok(new { message = "Webhook deletado com sucesso" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Erro ao deletar webhook: {ex.Message}");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao deletar webhook: {ex.Message}");
            return StatusCode(500, new { message = "Erro ao deletar webhook" });
        }
    }

    /// <summary>
    /// Testar um webhook
    /// </summary>
    [HttpPost("testar/{webhookId}")]
    public async Task<ActionResult<object>> TestarWebhookAsync(Guid webhookId)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());

            var result = await _webhookService.TestarWebhookAsync(webhookId, userId);

            _logger.LogInformation($"Webhook testado: {webhookId} para usuário {userId}");

            return Ok(new { message = "Webhook testado com sucesso", data = new { success = result } });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Erro ao testar webhook: {ex.Message}");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao testar webhook: {ex.Message}");
            return StatusCode(500, new { message = "Erro ao testar webhook" });
        }
    }

    /// <summary>
    /// Ativar/Desativar um webhook
    /// </summary>
    [HttpPut("ativar-desativar/{webhookId}")]
    public async Task<ActionResult<object>> AtivarDesativarWebhookAsync(Guid webhookId, [FromBody] AtivarDesativarWebhookRequest request)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());

            var webhook = await _webhookService.AtivarDesativarWebhookAsync(webhookId, userId, request.IsActive);

            _logger.LogInformation($"Webhook {(request.IsActive ? "ativado" : "desativado")}: {webhookId} para usuário {userId}");

            var response = new PixWebhookResponse
            {
                Id = webhook.Id,
                EventType = webhook.EventType,
                WebhookUrl = webhook.WebhookUrl,
                IsActive = webhook.IsActive,
                RetryCount = webhook.RetryCount,
                LastAttempt = webhook.LastAttempt,
                CreatedAt = webhook.CreatedAt,
                UpdatedAt = webhook.UpdatedAt
            };

            return Ok(new { message = $"Webhook {(request.IsActive ? "ativado" : "desativado")} com sucesso", data = response });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Erro ao ativar/desativar webhook: {ex.Message}");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao ativar/desativar webhook: {ex.Message}");
            return StatusCode(500, new { message = "Erro ao ativar/desativar webhook" });
        }
    }
}

