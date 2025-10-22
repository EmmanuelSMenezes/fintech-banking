using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinTechBanking.Core.Interfaces;
using FinTechBanking.API.Interna.Attributes;

namespace FinTechBanking.API.Interna.Controllers;

[ApiController]
[Route("api/webhooks")]
[Authorize]
[RateLimit(maxRequests: 100, windowSeconds: 60)]
public class WebhooksController : ControllerBase
{
    private readonly IWebhookService _webhookService;
    private readonly IWebhookLogRepository _webhookLogRepository;
    private readonly ILogger<WebhooksController> _logger;

    public WebhooksController(
        IWebhookService webhookService,
        IWebhookLogRepository webhookLogRepository,
        ILogger<WebhooksController> logger)
    {
        _webhookService = webhookService;
        _webhookLogRepository = webhookLogRepository;
        _logger = logger;
    }

    /// <summary>
    /// Registra uma URL de webhook para o usuário
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<object>> RegisterWebhookAsync([FromBody] RegisterWebhookRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.WebhookUrl))
                return BadRequest(new { message = "URL do webhook é obrigatória" });

            if (!Uri.TryCreate(request.WebhookUrl, UriKind.Absolute, out _))
                return BadRequest(new { message = "URL do webhook inválida" });

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var success = await _webhookService.RegisterWebhookAsync(userId, request.WebhookUrl);

            if (!success)
                return StatusCode(500, new { message = "Erro ao registrar webhook" });

            return Ok(new { message = "Webhook registrado com sucesso", webhookUrl = request.WebhookUrl });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar webhook");
            return StatusCode(500, new { message = "Erro ao registrar webhook", error = ex.Message });
        }
    }

    /// <summary>
    /// Remove o webhook do usuário
    /// </summary>
    [HttpPost("unregister")]
    public async Task<ActionResult<object>> UnregisterWebhookAsync()
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var success = await _webhookService.UnregisterWebhookAsync(userId);

            if (!success)
                return StatusCode(500, new { message = "Erro ao remover webhook" });

            return Ok(new { message = "Webhook removido com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover webhook");
            return StatusCode(500, new { message = "Erro ao remover webhook", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtém a URL de webhook registrada
    /// </summary>
    [HttpGet("url")]
    public async Task<ActionResult<object>> GetWebhookUrlAsync()
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var webhookUrl = await _webhookService.GetWebhookUrlAsync(userId);

            return Ok(new { webhookUrl = webhookUrl ?? "Nenhum webhook registrado" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter URL do webhook");
            return StatusCode(500, new { message = "Erro ao obter URL do webhook", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtém o histórico de webhooks enviados
    /// </summary>
    [HttpGet("history")]
    public async Task<ActionResult<object>> GetWebhookHistoryAsync([FromQuery] int limit = 50)
    {
        try
        {
            var webhooks = await _webhookLogRepository.GetAllAsync();
            var history = webhooks.OrderByDescending(w => w.ReceivedAt).Take(limit);

            return Ok(new
            {
                message = "Histórico de webhooks",
                total = webhooks.Count(),
                data = history.Select(w => new
                {
                    id = w.Id,
                    eventType = w.EventType,
                    status = w.Status,
                    receivedAt = w.ReceivedAt,
                    processedAt = w.ProcessedAt,
                    errorMessage = w.ErrorMessage
                })
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter histórico de webhooks");
            return StatusCode(500, new { message = "Erro ao obter histórico", error = ex.Message });
        }
    }

    /// <summary>
    /// Reprocessa webhooks que falharam
    /// </summary>
    [HttpPost("retry-failed")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<object>> RetryFailedWebhooksAsync()
    {
        try
        {
            await _webhookService.RetryFailedWebhooksAsync();
            return Ok(new { message = "Reprocessamento de webhooks iniciado" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao reprocessar webhooks");
            return StatusCode(500, new { message = "Erro ao reprocessar webhooks", error = ex.Message });
        }
    }
}

public class RegisterWebhookRequest
{
    public string WebhookUrl { get; set; }
}

