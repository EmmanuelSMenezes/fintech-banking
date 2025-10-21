namespace FinTechBanking.Core.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string FullName { get; set; }
    public string Document { get; set; } // CPF/CNPJ
    public string PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public string? Role { get; set; } // "admin" ou "user"
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? WebhookUrl { get; set; } // URL para notificações do cliente
}

