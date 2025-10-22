using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using FinTechBanking.Core.Interfaces;

namespace FinTechBanking.Services.RateLimiting;

public class RateLimitService : IRateLimitService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<RateLimitService> _logger;

    public RateLimitService(IMemoryCache cache, ILogger<RateLimitService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<bool> IsRateLimitExceededAsync(string userId, string endpoint, int maxRequests, int windowSeconds)
    {
        try
        {
            var key = GenerateKey(userId, endpoint);
            var currentCount = _cache.GetOrCreate(key, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(windowSeconds);
                return 0;
            });

            if (currentCount >= maxRequests)
            {
                _logger.LogWarning($"Rate limit exceeded for user {userId} on endpoint {endpoint}");
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao verificar rate limit para usuário {userId}");
            return false;
        }
    }

    public async Task IncrementRequestCountAsync(string userId, string endpoint)
    {
        try
        {
            var key = GenerateKey(userId, endpoint);
            
            if (_cache.TryGetValue(key, out int currentCount))
            {
                _cache.Set(key, currentCount + 1, TimeSpan.FromSeconds(60)); // Default 60 segundos
            }
            else
            {
                _cache.Set(key, 1, TimeSpan.FromSeconds(60));
            }

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao incrementar contador para usuário {userId}");
        }
    }

    public async Task<int> GetRemainingRequestsAsync(string userId, string endpoint, int maxRequests)
    {
        try
        {
            var key = GenerateKey(userId, endpoint);
            var currentCount = _cache.GetOrCreate(key, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60);
                return 0;
            });

            var remaining = Math.Max(0, maxRequests - currentCount);
            return remaining;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao obter requisições restantes para usuário {userId}");
            return maxRequests;
        }
    }

    public async Task ResetCounterAsync(string userId, string endpoint)
    {
        try
        {
            var key = GenerateKey(userId, endpoint);
            _cache.Remove(key);
            _logger.LogInformation($"Contador resetado para usuário {userId} no endpoint {endpoint}");
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao resetar contador para usuário {userId}");
        }
    }

    private string GenerateKey(string userId, string endpoint)
    {
        return $"ratelimit:{userId}:{endpoint}";
    }
}

