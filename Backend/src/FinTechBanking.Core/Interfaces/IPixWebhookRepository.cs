using FinTechBanking.Core.Entities;

namespace FinTechBanking.Core.Interfaces;

/// <summary>
/// Interface para repositório de Webhook de PIX
/// </summary>
public interface IPixWebhookRepository
{
    /// <summary>
    /// Criar um novo webhook
    /// </summary>
    Task<PixWebhook> CreateAsync(PixWebhook webhook);
    
    /// <summary>
    /// Obter webhook por ID
    /// </summary>
    Task<PixWebhook?> GetByIdAsync(Guid webhookId);
    
    /// <summary>
    /// Listar webhooks do usuário
    /// </summary>
    Task<List<PixWebhook>> GetByUserIdAsync(Guid userId);
    
    /// <summary>
    /// Listar webhooks ativos do usuário para um tipo de evento
    /// </summary>
    Task<List<PixWebhook>> GetActiveByUserAndEventAsync(Guid userId, string eventType);
    
    /// <summary>
    /// Atualizar webhook
    /// </summary>
    Task<bool> UpdateAsync(PixWebhook webhook);
    
    /// <summary>
    /// Deletar webhook
    /// </summary>
    Task<bool> DeleteAsync(Guid webhookId);
}

