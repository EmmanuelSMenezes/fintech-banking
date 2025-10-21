namespace FinTechBanking.Services.Email;

/// <summary>
/// Interface para serviço de envio de emails
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Enviar email com credenciais de primeiro acesso
    /// </summary>
    Task SendFirstAccessEmailAsync(string email, string fullName, string tempPassword);

    /// <summary>
    /// Enviar email genérico
    /// </summary>
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);

    /// <summary>
    /// Enviar email de confirmação
    /// </summary>
    Task SendConfirmationEmailAsync(string email, string confirmationLink);

    /// <summary>
    /// Enviar email de recuperação de senha
    /// </summary>
    Task SendPasswordResetEmailAsync(string email, string resetLink);
}

