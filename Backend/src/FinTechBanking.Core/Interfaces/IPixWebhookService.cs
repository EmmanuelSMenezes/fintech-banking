using FinTechBanking.Core.Entities;

namespace FinTechBanking.Core.Interfaces;

/// <summary>
/// Interface para serviço de Webhook de PIX
/// </summary>
public interface IPixWebhookService
{
    /// <summary>
    /// Registrar um novo webhook
    /// </summary>
    Task<PixWebhook> RegistrarWebhookAsync(Guid userId, string eventType, string webhookUrl);
    
    /// <summary>
    /// Listar webhooks do usuário
    /// </summary>
    Task<List<PixWebhook>> ListarWebhooksAsync(Guid userId);
    
    /// <summary>
    /// Deletar um webhook
    /// </summary>
    Task<bool> DeletarWebhookAsync(Guid webhookId, Guid userId);
    
    /// <summary>
    /// Testar um webhook enviando uma notificação de teste
    /// </summary>
    Task<bool> TestarWebhookAsync(Guid webhookId, Guid userId);
    
    /// <summary>
    /// Enviar notificação de webhook para um evento
    /// </summary>
    Task<bool> EnviarNotificacaoAsync(Guid userId, string eventType, object payload);
    
    /// <summary>
    /// Ativar/Desativar um webhook
    /// </summary>
    Task<PixWebhook> AtivarDesativarWebhookAsync(Guid webhookId, Guid userId, bool isActive);
}

