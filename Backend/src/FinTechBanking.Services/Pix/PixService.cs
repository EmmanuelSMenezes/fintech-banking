using FinTechBanking.Core.Entities;
using FinTechBanking.Core.Interfaces;

namespace FinTechBanking.Services.Pix;

/// <summary>
/// Serviço para gerenciar PIX Dinâmico
/// </summary>
public class PixService : IPixService
{
    private readonly IPixRepository _pixRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IBankingHub _bankingHub;
    private readonly IMessageBroker _messageBroker;

    public PixService(
        IPixRepository pixRepository,
        IAccountRepository accountRepository,
        IBankingHub bankingHub,
        IMessageBroker messageBroker)
    {
        _pixRepository = pixRepository;
        _accountRepository = accountRepository;
        _bankingHub = bankingHub;
        _messageBroker = messageBroker;
    }

    public async Task<PixDinamico> CriarPixDinamicoAsync(
        Guid userId,
        decimal amount,
        string description,
        string recipientKey)
    {
        // Validações
        if (amount <= 0)
            throw new ArgumentException("Valor deve ser maior que zero");

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Descrição é obrigatória");

        if (string.IsNullOrWhiteSpace(recipientKey))
            throw new ArgumentException("Chave PIX é obrigatória");

        // Obter conta do usuário
        var account = await _accountRepository.GetByUserIdAsync(userId);
        if (account == null)
            throw new InvalidOperationException("Conta não encontrada");

        if (!account.IsActive)
            throw new InvalidOperationException("Conta inativa");

        // Gerar QR Code via Banking Hub
        var qrCodeData = await _bankingHub.GeneratePixQrCodeAsync(
            account.BankCode,
            amount,
            recipientKey,
            description);

        // Criar entidade PIX Dinâmico
        var pixDinamico = new PixDinamico
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            AccountId = account.Id,
            Amount = amount,
            Description = description,
            RecipientKey = recipientKey,
            QrCodeData = qrCodeData,
            QrCodeUrl = $"https://api.example.com/qrcode/{Guid.NewGuid()}",
            Status = "PENDING",
            BankCode = account.BankCode,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddHours(24) // PIX dinâmico expira em 24h
        };

        // Salvar no banco
        var created = await _pixRepository.CreateAsync(pixDinamico);

        // Publicar evento
        await _messageBroker.PublishAsync("pix-dinamico-criado", new
        {
            PixId = created.Id,
            UserId = userId,
            Amount = amount,
            RecipientKey = recipientKey,
            BankCode = account.BankCode,
            CreatedAt = DateTime.UtcNow
        });

        return created;
    }

    public async Task<PixDinamico?> ObterStatusAsync(Guid pixId)
    {
        return await _pixRepository.GetByIdAsync(pixId);
    }

    public async Task<PixDinamico> ConfirmarPagamentoAsync(Guid pixId)
    {
        var pixDinamico = await _pixRepository.GetByIdAsync(pixId);
        if (pixDinamico == null)
            throw new InvalidOperationException("PIX dinâmico não encontrado");

        if (pixDinamico.Status == "PAID")
            throw new InvalidOperationException("PIX já foi pago");

        if (pixDinamico.Status == "EXPIRED")
            throw new InvalidOperationException("PIX expirou");

        if (pixDinamico.Status == "CANCELLED")
            throw new InvalidOperationException("PIX foi cancelado");

        // Atualizar status
        pixDinamico.Status = "PAID";
        pixDinamico.PaidAt = DateTime.UtcNow;
        pixDinamico.UpdatedAt = DateTime.UtcNow;

        var updated = await _pixRepository.UpdateAsync(pixDinamico);

        // Publicar evento
        await _messageBroker.PublishAsync("pix-dinamico-pago", new
        {
            PixId = updated.Id,
            UserId = updated.UserId,
            Amount = updated.Amount,
            PaidAt = DateTime.UtcNow
        });

        return updated;
    }

    public async Task<List<PixDinamico>> ListarPixDinamicosAsync(Guid userId)
    {
        return await _pixRepository.GetByUserIdAsync(userId);
    }

    public async Task<PixDinamico> CancelarPixAsync(Guid pixId)
    {
        var pixDinamico = await _pixRepository.GetByIdAsync(pixId);
        if (pixDinamico == null)
            throw new InvalidOperationException("PIX dinâmico não encontrado");

        if (pixDinamico.Status == "PAID")
            throw new InvalidOperationException("Não é possível cancelar PIX já pago");

        pixDinamico.Status = "CANCELLED";
        pixDinamico.UpdatedAt = DateTime.UtcNow;

        return await _pixRepository.UpdateAsync(pixDinamico);
    }
}

