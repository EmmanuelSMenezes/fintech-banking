using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FinTechBanking.Core.Interfaces;

namespace FinTechBanking.API.Interna.Attributes;

/// <summary>
/// Atributo para aplicar rate limiting a endpoints
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RateLimitAttribute : Attribute, IAsyncActionFilter
{
    private readonly int _maxRequests;
    private readonly int _windowSeconds;

    /// <summary>
    /// Cria um novo atributo de rate limiting
    /// </summary>
    /// <param name="maxRequests">Número máximo de requisições permitidas</param>
    /// <param name="windowSeconds">Janela de tempo em segundos</param>
    public RateLimitAttribute(int maxRequests = 100, int windowSeconds = 60)
    {
        _maxRequests = maxRequests;
        _windowSeconds = windowSeconds;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        try
        {
            var rateLimitService = context.HttpContext.RequestServices.GetService<IRateLimitService>();
            if (rateLimitService == null)
            {
                await next();
                return;
            }

            var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                // Se não autenticado, usar IP como identificador
                userId = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            }

            var endpoint = context.HttpContext.Request.Path.ToString();

            // Verificar se excedeu o limite
            var isExceeded = await rateLimitService.IsRateLimitExceededAsync(userId, endpoint, _maxRequests, _windowSeconds);
            
            if (isExceeded)
            {
                var remaining = await rateLimitService.GetRemainingRequestsAsync(userId, endpoint, _maxRequests);
                
                context.Result = new ObjectResult(new
                {
                    message = "Rate limit exceeded",
                    error = $"Você excedeu o limite de {_maxRequests} requisições por {_windowSeconds} segundos",
                    retryAfter = _windowSeconds
                })
                {
                    StatusCode = 429
                };

                context.HttpContext.Response.Headers.Add("Retry-After", _windowSeconds.ToString());
                context.HttpContext.Response.Headers.Add("X-RateLimit-Limit", _maxRequests.ToString());
                context.HttpContext.Response.Headers.Add("X-RateLimit-Remaining", remaining.ToString());
                
                return;
            }

            // Incrementar contador
            await rateLimitService.IncrementRequestCountAsync(userId, endpoint);

            // Adicionar headers de rate limit
            var remainingRequests = await rateLimitService.GetRemainingRequestsAsync(userId, endpoint, _maxRequests);
            context.HttpContext.Response.Headers.Add("X-RateLimit-Limit", _maxRequests.ToString());
            context.HttpContext.Response.Headers.Add("X-RateLimit-Remaining", remainingRequests.ToString());
            context.HttpContext.Response.Headers.Add("X-RateLimit-Reset", DateTime.UtcNow.AddSeconds(_windowSeconds).ToString("O"));

            await next();
        }
        catch (Exception ex)
        {
            // Em caso de erro, permitir a requisição passar
            await next();
        }
    }
}

