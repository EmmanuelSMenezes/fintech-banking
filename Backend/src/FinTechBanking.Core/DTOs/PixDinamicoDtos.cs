namespace FinTechBanking.Core.DTOs;

/// <summary>
/// Request para criar PIX dinâmico
/// </summary>
public class CriarPixDinamicoRequest
{
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string RecipientKey { get; set; }
}

/// <summary>
/// Response para criação de PIX dinâmico
/// </summary>
public class CriarPixDinamicoResponse
{
    public Guid PixId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string QrCode { get; set; }
    public string QrCodeUrl { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

/// <summary>
/// Response para status de PIX dinâmico
/// </summary>
public class PixDinamicoStatusResponse
{
    public Guid PixId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime? PaidAt { get; set; }
}

/// <summary>
/// Response para confirmação de pagamento
/// </summary>
public class ConfirmarPagamentoResponse
{
    public Guid PixId { get; set; }
    public string Status { get; set; }
    public DateTime PaidAt { get; set; }
    public decimal Amount { get; set; }
}

/// <summary>
/// Response para listar PIX dinâmicos
/// </summary>
public class ListarPixDinamicosResponse
{
    public List<PixDinamicoItemResponse> Items { get; set; } = new();
    public int Total { get; set; }
}

/// <summary>
/// Item de PIX dinâmico na listagem
/// </summary>
public class PixDinamicoItemResponse
{
    public Guid PixId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? PaidAt { get; set; }
}

/// <summary>
/// DTO para evento de PIX dinâmico criado
/// </summary>
public class PixDinamicoCriadoEventDto
{
    public Guid PixId { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public string RecipientKey { get; set; }
    public string BankCode { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO para evento de PIX dinâmico pago
/// </summary>
public class PixDinamicoPagoEventDto
{
    public Guid PixId { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; }
}

