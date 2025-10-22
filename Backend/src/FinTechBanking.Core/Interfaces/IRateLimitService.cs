namespace FinTechBanking.Core.Interfaces;

public interface IRateLimitService
{
    /// <summary>
    /// Verifica se o usuário excedeu o limite de requisições
    /// </summary>
    Task<bool> IsRateLimitExceededAsync(string userId, string endpoint, int maxRequests, int windowSeconds);

    /// <summary>
    /// Incrementa o contador de requisições
    /// </summary>
    Task IncrementRequestCountAsync(string userId, string endpoint);

    /// <summary>
    /// Obtém o número de requisições restantes
    /// </summary>
    Task<int> GetRemainingRequestsAsync(string userId, string endpoint, int maxRequests);

    /// <summary>
    /// Reseta o contador de requisições
    /// </summary>
    Task ResetCounterAsync(string userId, string endpoint);
}

