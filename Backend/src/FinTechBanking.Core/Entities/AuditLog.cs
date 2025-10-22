namespace FinTechBanking.Core.Entities;

/// <summary>
/// Entidade para rastrear todas as operações do sistema
/// </summary>
public class AuditLog
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// ID do usuário que realizou a ação
    /// </summary>
    public Guid? UserId { get; set; }
    
    /// <summary>
    /// Email do usuário (para referência)
    /// </summary>
    public string? UserEmail { get; set; }
    
    /// <summary>
    /// Tipo de ação (CREATE, READ, UPDATE, DELETE, LOGIN, LOGOUT, etc)
    /// </summary>
    public string Action { get; set; } = string.Empty;
    
    /// <summary>
    /// Entidade afetada (User, Account, Transaction, etc)
    /// </summary>
    public string Entity { get; set; } = string.Empty;
    
    /// <summary>
    /// ID da entidade afetada
    /// </summary>
    public string? EntityId { get; set; }
    
    /// <summary>
    /// Descrição da ação
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Dados antigos (antes da mudança) - JSON
    /// </summary>
    public string? OldValues { get; set; }
    
    /// <summary>
    /// Dados novos (depois da mudança) - JSON
    /// </summary>
    public string? NewValues { get; set; }
    
    /// <summary>
    /// Método HTTP (GET, POST, PUT, DELETE, etc)
    /// </summary>
    public string HttpMethod { get; set; } = string.Empty;
    
    /// <summary>
    /// Endpoint acessado
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;
    
    /// <summary>
    /// Endereço IP do cliente
    /// </summary>
    public string? IpAddress { get; set; }
    
    /// <summary>
    /// User Agent do cliente
    /// </summary>
    public string? UserAgent { get; set; }
    
    /// <summary>
    /// Status HTTP da resposta
    /// </summary>
    public int? StatusCode { get; set; }
    
    /// <summary>
    /// Tempo de execução em milissegundos
    /// </summary>
    public long? ExecutionTimeMs { get; set; }
    
    /// <summary>
    /// Resultado da ação (Success, Error, etc)
    /// </summary>
    public string Result { get; set; } = "Success";
    
    /// <summary>
    /// Mensagem de erro (se houver)
    /// </summary>
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// Data e hora da ação
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

