using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinTechBanking.Core.Interfaces;
using FinTechBanking.API.Interna.Attributes;

namespace FinTechBanking.API.Interna.Controllers;

/// <summary>
/// Controller para consultar logs de auditoria
/// </summary>
[ApiController]
[Route("api/audit")]
[Authorize]
[RateLimit(maxRequests: 100, windowSeconds: 60)]
public class AuditController : ControllerBase
{
    private readonly IAuditService _auditService;
    private readonly ILogger<AuditController> _logger;

    public AuditController(IAuditService auditService, ILogger<AuditController> logger)
    {
        _auditService = auditService;
        _logger = logger;
    }

    /// <summary>
    /// Obtém logs de auditoria do usuário autenticado
    /// </summary>
    [HttpGet("my-logs")]
    public async Task<ActionResult<object>> GetMyLogsAsync([FromQuery] int limit = 50)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var logs = await _auditService.GetUserLogsAsync(userId, limit);

            return Ok(new
            {
                message = "Logs de auditoria obtidos com sucesso",
                data = logs,
                count = logs.Count()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter logs do usuário");
            return StatusCode(500, new { message = "Erro ao obter logs", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtém logs de auditoria de um usuário específico (admin only)
    /// </summary>
    [HttpGet("user/{userId}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<object>> GetUserLogsAsync(Guid userId, [FromQuery] int limit = 50)
    {
        try
        {
            var logs = await _auditService.GetUserLogsAsync(userId, limit);

            return Ok(new
            {
                message = "Logs de auditoria do usuário obtidos com sucesso",
                data = logs,
                count = logs.Count()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter logs do usuário");
            return StatusCode(500, new { message = "Erro ao obter logs", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtém logs de auditoria de uma entidade (admin only)
    /// </summary>
    [HttpGet("entity/{entity}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<object>> GetEntityLogsAsync(string entity, [FromQuery] int limit = 50)
    {
        try
        {
            var logs = await _auditService.GetEntityLogsAsync(entity, limit);

            return Ok(new
            {
                message = $"Logs de auditoria da entidade {entity} obtidos com sucesso",
                data = logs,
                count = logs.Count()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter logs da entidade");
            return StatusCode(500, new { message = "Erro ao obter logs", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtém logs de auditoria com filtros (admin only)
    /// </summary>
    [HttpGet("search")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<object>> SearchLogsAsync(
        [FromQuery] string? userId = null,
        [FromQuery] string? action = null,
        [FromQuery] string? entity = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] int page = 1,
        [FromQuery] int limit = 50)
    {
        try
        {
            var logs = await _auditService.GetLogsAsync(userId, action, entity, startDate, endDate, page, limit);

            return Ok(new
            {
                message = "Logs de auditoria encontrados com sucesso",
                data = logs,
                count = logs.Count(),
                filters = new { userId, action, entity, startDate, endDate, page, limit }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar logs");
            return StatusCode(500, new { message = "Erro ao buscar logs", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtém estatísticas de auditoria (admin only)
    /// </summary>
    [HttpGet("stats")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<object>> GetStatsAsync()
    {
        try
        {
            // Obter logs dos últimos 7 dias
            var startDate = DateTime.UtcNow.AddDays(-7);
            var endDate = DateTime.UtcNow;
            var logs = await _auditService.GetLogsAsync(startDate: startDate, endDate: endDate, limit: 10000);

            var stats = new
            {
                totalLogs = logs.Count(),
                byAction = logs.GroupBy(l => l.Action).Select(g => new { action = g.Key, count = g.Count() }),
                byEntity = logs.GroupBy(l => l.Entity).Select(g => new { entity = g.Key, count = g.Count() }),
                byResult = logs.GroupBy(l => l.Result).Select(g => new { result = g.Key, count = g.Count() }),
                averageExecutionTime = logs.Where(l => l.ExecutionTimeMs.HasValue).Average(l => l.ExecutionTimeMs),
                period = new { startDate, endDate }
            };

            return Ok(new
            {
                message = "Estatísticas de auditoria obtidas com sucesso",
                data = stats
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter estatísticas");
            return StatusCode(500, new { message = "Erro ao obter estatísticas", error = ex.Message });
        }
    }
}

