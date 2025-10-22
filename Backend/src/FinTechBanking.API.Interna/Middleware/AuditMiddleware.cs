using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using FinTechBanking.Core.Interfaces;

namespace FinTechBanking.API.Interna.Middleware;

/// <summary>
/// Middleware para capturar automaticamente todas as requisições e respostas
/// </summary>
public class AuditMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuditMiddleware> _logger;

    public AuditMiddleware(RequestDelegate next, ILogger<AuditMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IAuditService auditService)
    {
        var stopwatch = Stopwatch.StartNew();
        var originalBodyStream = context.Response.Body;

        try
        {
            // Capturar informações da requisição
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmail = context.User.FindFirst(ClaimTypes.Email)?.Value;
            var httpMethod = context.Request.Method;
            var endpoint = context.Request.Path.ToString();
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();
            var userAgent = context.Request.Headers["User-Agent"].ToString();

            // Pular endpoints de health check e swagger
            if (ShouldSkipAudit(endpoint))
            {
                await _next(context);
                return;
            }

            // Executar a requisição
            await _next(context);

            stopwatch.Stop();

            // Registrar auditoria
            var statusCode = context.Response.StatusCode;
            var result = statusCode >= 200 && statusCode < 300 ? "Success" : "Error";

            await auditService.LogActionAsync(
                userId: string.IsNullOrEmpty(userId) ? null : Guid.Parse(userId),
                userEmail: userEmail,
                action: httpMethod,
                entity: ExtractEntity(endpoint),
                entityId: null,
                description: $"{httpMethod} {endpoint}",
                httpMethod: httpMethod,
                endpoint: endpoint,
                ipAddress: ipAddress,
                userAgent: userAgent,
                statusCode: statusCode,
                executionTimeMs: stopwatch.ElapsedMilliseconds,
                result: result);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Erro no middleware de auditoria");

            // Continuar mesmo com erro
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = 500;
            }
        }
    }

    private bool ShouldSkipAudit(string endpoint)
    {
        // Pular endpoints que não precisam de auditoria
        var skipPatterns = new[]
        {
            "/health",
            "/swagger",
            "/api-docs",
            "/.well-known",
            "/metrics"
        };

        return skipPatterns.Any(pattern => endpoint.StartsWith(pattern, StringComparison.OrdinalIgnoreCase));
    }

    private string ExtractEntity(string endpoint)
    {
        // Extrair entidade do endpoint
        // Ex: /api/users/123 -> users
        var parts = endpoint.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2)
        {
            return parts[1]; // Retorna "users", "transactions", etc
        }
        return "Unknown";
    }
}

/// <summary>
/// Extensão para registrar o middleware
/// </summary>
public static class AuditMiddlewareExtensions
{
    public static IApplicationBuilder UseAuditMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuditMiddleware>();
    }
}

