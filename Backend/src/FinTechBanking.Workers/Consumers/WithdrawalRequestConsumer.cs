using System.Text.Json;
using FinTechBanking.Core.Interfaces;
using FinTechBanking.Core.DTOs;

namespace FinTechBanking.Workers.Consumers;

/// <summary>
/// Consumer para processar requisições de saque da fila RabbitMQ
/// </summary>
public class WithdrawalRequestConsumer
{
    private readonly IBankingHub _bankingHub;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IMessageBroker _messageBroker;

    public WithdrawalRequestConsumer(
        IBankingHub bankingHub,
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository,
        IMessageBroker messageBroker)
    {
        _bankingHub = bankingHub;
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _messageBroker = messageBroker;
    }

    /// <summary>
    /// Processa uma requisição de saque
    /// </summary>
    public async Task ProcessAsync(WithdrawalRequestDto request)
    {
        try
        {
            Console.WriteLine($"[WithdrawalRequestConsumer] Processando requisição: {request.TransactionId}");

            // 1. Obter transação
            var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId);
            if (transaction == null)
            {
                Console.WriteLine($"[WithdrawalRequestConsumer] Transação não encontrada: {request.TransactionId}");
                return;
            }

            // 2. Obter conta
            var account = await _accountRepository.GetByIdAsync(transaction.AccountId);
            if (account == null)
            {
                Console.WriteLine($"[WithdrawalRequestConsumer] Conta não encontrada: {transaction.AccountId}");
                throw new InvalidOperationException("Conta não encontrada");
            }

            // 3. Validar saldo
            if (account.Balance < transaction.Amount)
            {
                Console.WriteLine($"[WithdrawalRequestConsumer] Saldo insuficiente. Saldo: {account.Balance}, Solicitado: {transaction.Amount}");
                throw new InvalidOperationException("Saldo insuficiente");
            }

            // 4. Processar saque via Banking Hub
            var success = await _bankingHub.ProcessWithdrawalAsync(
                account.BankCode,
                transaction.Amount,
                account.AccountNumber
            );

            if (!success)
            {
                throw new InvalidOperationException("Falha ao processar saque no banco");
            }

            Console.WriteLine($"[WithdrawalRequestConsumer] Saque processado com sucesso: {request.TransactionId}");

            // 5. Atualizar saldo da conta
            account.Balance -= transaction.Amount;
            account.UpdatedAt = DateTime.UtcNow;
            await _accountRepository.UpdateAsync(account);

            // 6. Atualizar transação com sucesso
            transaction.Status = "COMPLETED";
            transaction.UpdatedAt = DateTime.UtcNow;
            await _transactionRepository.UpdateAsync(transaction);

            // 7. Publicar evento de sucesso
            var successEvent = new
            {
                TransactionId = request.TransactionId,
                Status = "COMPLETED",
                Amount = transaction.Amount,
                NewBalance = account.Balance,
                Timestamp = DateTime.UtcNow
            };

            await _messageBroker.PublishAsync("withdrawal-completed", successEvent);
            Console.WriteLine($"[WithdrawalRequestConsumer] Evento de sucesso publicado: {request.TransactionId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WithdrawalRequestConsumer] Erro ao processar: {ex.Message}");

            try
            {
                // Atualizar transação como FAILED
                var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId);
                if (transaction != null)
                {
                    transaction.Status = "FAILED";
                    transaction.UpdatedAt = DateTime.UtcNow;
                    await _transactionRepository.UpdateAsync(transaction);
                }

                // Publicar evento de erro
                var errorEvent = new
                {
                    TransactionId = request.TransactionId,
                    Status = "FAILED",
                    Error = ex.Message,
                    Timestamp = DateTime.UtcNow
                };

                await _messageBroker.PublishAsync("withdrawal-failed", errorEvent);
                Console.WriteLine($"[WithdrawalRequestConsumer] Evento de erro publicado: {request.TransactionId}");
            }
            catch (Exception innerEx)
            {
                Console.WriteLine($"[WithdrawalRequestConsumer] Erro ao tratar falha: {innerEx.Message}");
            }
        }
    }
}

