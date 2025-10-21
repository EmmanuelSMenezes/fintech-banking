using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FinTechBanking.Services.Email;

/// <summary>
/// Serviço de envio de emails via SMTP
/// </summary>
public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SmtpEmailService> _logger;
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly string _fromEmail;
    private readonly string _fromName;

    public SmtpEmailService(IConfiguration configuration, ILogger<SmtpEmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;

        var emailSettings = _configuration.GetSection("Email");
        _smtpServer = emailSettings["SmtpServer"] ?? throw new InvalidOperationException("Email:SmtpServer não configurado");
        _smtpPort = int.Parse(emailSettings["SmtpPort"] ?? "587");
        _smtpUsername = emailSettings["SmtpUsername"] ?? throw new InvalidOperationException("Email:SmtpUsername não configurado");
        _smtpPassword = emailSettings["SmtpPassword"] ?? throw new InvalidOperationException("Email:SmtpPassword não configurado");
        _fromEmail = emailSettings["FromEmail"] ?? throw new InvalidOperationException("Email:FromEmail não configurado");
        _fromName = emailSettings["FromName"] ?? "FinTech Banking";
    }

    public async Task SendFirstAccessEmailAsync(string email, string fullName, string tempPassword)
    {
        var subject = "Bem-vindo ao FinTech Banking - Suas Credenciais de Acesso";
        var body = GenerateFirstAccessEmailBody(fullName, email, tempPassword);
        await SendEmailAsync(email, subject, body, isHtml: true);
    }

    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
    {
        try
        {
            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail, _fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml
                };

                mailMessage.To.Add(to);

                await client.SendMailAsync(mailMessage);
                _logger.LogInformation("Email enviado com sucesso para {Email}", to);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar email para {Email}", to);
            throw;
        }
    }

    public async Task SendConfirmationEmailAsync(string email, string confirmationLink)
    {
        var subject = "Confirme seu email - FinTech Banking";
        var body = $@"
            <h2>Confirmação de Email</h2>
            <p>Clique no link abaixo para confirmar seu email:</p>
            <a href='{confirmationLink}'>Confirmar Email</a>
            <p>Se você não solicitou isso, ignore este email.</p>
        ";
        await SendEmailAsync(email, subject, body, isHtml: true);
    }

    public async Task SendPasswordResetEmailAsync(string email, string resetLink)
    {
        var subject = "Recuperação de Senha - FinTech Banking";
        var body = $@"
            <h2>Recuperação de Senha</h2>
            <p>Clique no link abaixo para redefinir sua senha:</p>
            <a href='{resetLink}'>Redefinir Senha</a>
            <p>Este link expira em 24 horas.</p>
            <p>Se você não solicitou isso, ignore este email.</p>
        ";
        await SendEmailAsync(email, subject, body, isHtml: true);
    }

    private string GenerateFirstAccessEmailBody(string fullName, string email, string tempPassword)
    {
        return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background-color: #007bff; color: white; padding: 20px; text-align: center; }}
                    .content {{ padding: 20px; background-color: #f9f9f9; }}
                    .credentials {{ background-color: #fff; padding: 15px; border: 1px solid #ddd; margin: 20px 0; }}
                    .footer {{ text-align: center; padding: 20px; color: #666; font-size: 12px; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>Bem-vindo ao FinTech Banking!</h1>
                    </div>
                    <div class='content'>
                        <p>Olá <strong>{fullName}</strong>,</p>
                        <p>Sua conta foi criada com sucesso! Aqui estão suas credenciais de primeiro acesso:</p>
                        
                        <div class='credentials'>
                            <p><strong>Email:</strong> {email}</p>
                            <p><strong>Senha Temporária:</strong> {tempPassword}</p>
                        </div>
                        
                        <p><strong>⚠️ IMPORTANTE:</strong></p>
                        <ul>
                            <li>Acesse o sistema em: <a href='http://localhost:3000'>http://localhost:3000</a></li>
                            <li>Faça login com as credenciais acima</li>
                            <li>Altere sua senha imediatamente após o primeiro acesso</li>
                            <li>Nunca compartilhe suas credenciais</li>
                        </ul>
                        
                        <p>Se você não solicitou esta conta, entre em contato conosco imediatamente.</p>
                    </div>
                    <div class='footer'>
                        <p>© 2025 FinTech Banking. Todos os direitos reservados.</p>
                        <p>Este é um email automático, não responda.</p>
                    </div>
                </div>
            </body>
            </html>
        ";
    }
}

