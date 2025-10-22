namespace FinTechBanking.Core.DTOs;

/// <summary>
/// DTO para requisição de registro de webhook
/// </summary>
public class RegistrarWebhookRequest
{
    public string EventType { get; set; }
    public string WebhookUrl { get; set; }
}

/// <summary>
/// DTO para resposta de webhook
/// </summary>
public class PixWebhookResponse
{
    public Guid Id { get; set; }
    public string EventType { get; set; }
    public string WebhookUrl { get; set; }
    public bool IsActive { get; set; }
    public int RetryCount { get; set; }
    public DateTime? LastAttempt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// DTO para resposta de listagem de webhooks
/// </summary>
public class ListarWebhooksResponse
{
    public List<PixWebhookResponse> Webhooks { get; set; }
    public int Total { get; set; }
}

/// <summary>
/// DTO para resposta de teste de webhook
/// </summary>
public class TestarWebhookResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public int StatusCode { get; set; }
}

/// <summary>
/// DTO para evento de webhook
/// </summary>
public class PixWebhookEventDto
{
    public string EventType { get; set; }
    public DateTime Timestamp { get; set; }
    public object Data { get; set; }
}

/// <summary>
/// DTO para ativar/desativar webhook
/// </summary>
public class AtivarDesativarWebhookRequest
{
    public bool IsActive { get; set; }
}

