namespace FinTechBanking.Core.Entities;

/// <summary>
/// Entidade para PIX Dinâmico
/// Representa um QR Code dinâmico gerado para receber pagamentos
/// </summary>
public class PixDinamico
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid AccountId { get; set; }
    
    /// <summary>
    /// Valor do PIX dinâmico
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Descrição/Identificador do PIX
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Chave PIX do recebedor
    /// </summary>
    public string RecipientKey { get; set; }
    
    /// <summary>
    /// Dados do QR Code (string codificada)
    /// </summary>
    public string QrCodeData { get; set; }
    
    /// <summary>
    /// URL para acessar o QR Code
    /// </summary>
    public string QrCodeUrl { get; set; }
    
    /// <summary>
    /// Status do PIX: PENDING, PAID, EXPIRED, CANCELLED
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// ID externo do banco
    /// </summary>
    public string? ExternalId { get; set; }
    
    /// <summary>
    /// Código do banco (ex: "001" para Sicoob)
    /// </summary>
    public string BankCode { get; set; }
    
    /// <summary>
    /// Data de criação
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Data de expiração
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
    
    /// <summary>
    /// Data de pagamento
    /// </summary>
    public DateTime? PaidAt { get; set; }
    
    /// <summary>
    /// Última atualização
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}

