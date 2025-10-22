namespace FinTechBanking.Core.Entities;

/// <summary>
/// Entidade para Webhook de PIX
/// Representa um webhook registrado para receber notificações de eventos de PIX
/// </summary>
public class PixWebhook
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Tipo de evento: pix-dinamico-criado, pix-dinamico-pago, pix-dinamico-expirado, pix-dinamico-cancelado
    /// </summary>
    public string EventType { get; set; }
    
    /// <summary>
    /// URL do webhook para receber notificações
    /// </summary>
    public string WebhookUrl { get; set; }
    
    /// <summary>
    /// Indica se o webhook está ativo
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Número de tentativas de envio
    /// </summary>
    public int RetryCount { get; set; }
    
    /// <summary>
    /// Última tentativa de envio
    /// </summary>
    public DateTime? LastAttempt { get; set; }
    
    /// <summary>
    /// Data de criação
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Última atualização
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}

