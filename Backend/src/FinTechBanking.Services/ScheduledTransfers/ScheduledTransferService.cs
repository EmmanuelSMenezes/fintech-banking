using FinTechBanking.Core.DTOs;
using FinTechBanking.Core.Entities;
using FinTechBanking.Core.Interfaces;

namespace FinTechBanking.Services.ScheduledTransfers;

public class ScheduledTransferService : IScheduledTransferService
{
    private readonly IScheduledTransferRepository _repository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;

    public ScheduledTransferService(
        IScheduledTransferRepository repository,
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository)
    {
        _repository = repository;
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
    }

    public async Task<ScheduledTransferResponse> AgendarTransferenciaAsync(Guid userId, AgendarTransferenciaRequest request)
    {
        // Validações
        if (request.Amount <= 0)
            throw new ArgumentException("Valor deve ser maior que zero");

        if (request.ScheduledDate <= DateTime.UtcNow)
            throw new ArgumentException("Data agendada deve ser no futuro");

        if (string.IsNullOrWhiteSpace(request.RecipientKey))
            throw new ArgumentException("Chave PIX do destinatário é obrigatória");

        if (request.AccountId == Guid.Empty)
            throw new ArgumentException("Conta é obrigatória");

        var transfer = new ScheduledTransfer
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            AccountId = request.AccountId,
            RecipientKey = request.RecipientKey,
            Amount = request.Amount,
            Description = request.Description,
            ScheduledDate = request.ScheduledDate,
            Status = "PENDING",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateAsync(transfer);
        return MapToResponse(created);
    }

    public async Task<List<ScheduledTransferResponse>> ListarTransferenciasAgendadasAsync(Guid userId)
    {
        var transfers = await _repository.GetByUserIdAsync(userId);
        return transfers.Select(MapToResponse).ToList();
    }

    public async Task<ScheduledTransferResponse> ObterDetalhesAsync(Guid transferId, Guid userId)
    {
        var transfer = await _repository.GetByIdAsync(transferId);
        
        if (transfer == null)
            throw new KeyNotFoundException("Transferência não encontrada");

        if (transfer.UserId != userId)
            throw new UnauthorizedAccessException("Acesso negado");

        return MapToResponse(transfer);
    }

    public async Task<bool> CancelarTransferenciaAsync(Guid transferId, Guid userId)
    {
        var transfer = await _repository.GetByIdAsync(transferId);
        
        if (transfer == null)
            throw new KeyNotFoundException("Transferência não encontrada");

        if (transfer.UserId != userId)
            throw new UnauthorizedAccessException("Acesso negado");

        if (transfer.Status != "PENDING")
            throw new InvalidOperationException("Apenas transferências pendentes podem ser canceladas");

        transfer.Status = "CANCELLED";
        transfer.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(transfer);
        return true;
    }

    public async Task<bool> ExecutarTransferenciasAgendadasAsync()
    {
        var pendingTransfers = await _repository.GetPendingTransfersAsync();

        foreach (var transfer in pendingTransfers)
        {
            try
            {
                // Validar conta
                var account = await _accountRepository.GetByIdAsync(transfer.AccountId);
                if (account == null)
                    throw new KeyNotFoundException("Conta não encontrada");

                if (account.Balance < transfer.Amount)
                    throw new InvalidOperationException("Saldo insuficiente");

                // Criar transação PIX
                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    AccountId = transfer.AccountId,
                    UserId = transfer.UserId,
                    TransactionType = "PIX_TRANSFER",
                    Amount = transfer.Amount,
                    Status = "COMPLETED",
                    Description = transfer.Description ?? "Transferência agendada",
                    RecipientKey = transfer.RecipientKey,
                    BankCode = "001",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _transactionRepository.CreateAsync(transaction);

                // Atualizar status
                transfer.Status = "EXECUTED";
                transfer.ExecutedAt = DateTime.UtcNow;
                transfer.UpdatedAt = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                transfer.Status = "FAILED";
                transfer.ErrorMessage = ex.Message;
                transfer.UpdatedAt = DateTime.UtcNow;
            }

            await _repository.UpdateAsync(transfer);
        }

        return true;
    }

    private static ScheduledTransferResponse MapToResponse(ScheduledTransfer transfer)
    {
        return new ScheduledTransferResponse
        {
            Id = transfer.Id,
            UserId = transfer.UserId,
            AccountId = transfer.AccountId,
            RecipientKey = transfer.RecipientKey,
            Amount = transfer.Amount,
            Description = transfer.Description,
            ScheduledDate = transfer.ScheduledDate,
            Status = transfer.Status,
            CreatedAt = transfer.CreatedAt,
            UpdatedAt = transfer.UpdatedAt,
            ExecutedAt = transfer.ExecutedAt,
            ErrorMessage = transfer.ErrorMessage
        };
    }
}

