using FinTechBanking.Core.DTOs;
using FinTechBanking.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinTechBanking.API.Interna.Controllers;

[ApiController]
[Route("api/transferencias")]
[Authorize]
public class ScheduledTransfersController : ControllerBase
{
    private readonly IScheduledTransferService _service;

    public ScheduledTransfersController(IScheduledTransferService service)
    {
        _service = service;
    }

    [HttpPost("agendar")]
    public async Task<IActionResult> AgendarTransferencia([FromBody] AgendarTransferenciaRequest request)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
            var result = await _service.AgendarTransferenciaAsync(userId, request);
            return Ok(new { message = "Transferência agendada com sucesso", data = result });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao agendar transferência", error = ex.Message });
        }
    }

    [HttpGet("agendadas")]
    public async Task<IActionResult> ListarTransferenciasAgendadas()
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
            var result = await _service.ListarTransferenciasAgendadasAsync(userId);
            return Ok(new { message = "Transferências listadas com sucesso", data = new { transferencias = result, total = result.Count } });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao listar transferências", error = ex.Message });
        }
    }

    [HttpGet("agendadas/{transferId}")]
    public async Task<IActionResult> ObterDetalhes(Guid transferId)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
            var result = await _service.ObterDetalhesAsync(transferId, userId);
            return Ok(new { message = "Detalhes obtidos com sucesso", data = result });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao obter detalhes", error = ex.Message });
        }
    }

    [HttpDelete("agendadas/{transferId}")]
    public async Task<IActionResult> CancelarTransferencia(Guid transferId)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
            await _service.CancelarTransferenciaAsync(transferId, userId);
            return Ok(new { message = "Transferência cancelada com sucesso" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao cancelar transferência", error = ex.Message });
        }
    }
}

