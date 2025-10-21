using System.Text.Json;
using FinTechBanking.Core.Interfaces;
using FinTechBanking.Core.DTOs;

namespace FinTechBanking.Workers.Consumers;

/// <summary>
/// Consumer para processar requisições de QR Code PIX da fila RabbitMQ
/// </summary>
public class PixRequestConsumer
{
    private readonly IBankingHub _bankingHub;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMessageBroker _messageBroker;

    public PixRequestConsumer(
        IBankingHub bankingHub,
        ITransactionRepository transactionRepository,
        IMessageBroker messageBroker)
    {
        _bankingHub = bankingHub;
        _transactionRepository = transactionRepository;
        _messageBroker = messageBroker;
    }

    /// <summary>
    /// Processa uma requisição de QR Code PIX
    /// </summary>
    public async Task ProcessAsync(PixQrCodeRequestDto request)
    {
        try
        {
            Console.WriteLine($"[PixRequestConsumer] Processando requisição: {request.TransactionId}");

            // 1. Obter transação
            var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId);
            if (transaction == null)
            {
                Console.WriteLine($"[PixRequestConsumer] Transação não encontrada: {request.TransactionId}");
                return;
            }

            // 2. Gerar QR Code via Banking Hub
            var qrCode = await _bankingHub.GeneratePixQrCodeAsync(
                transaction.BankCode,
                transaction.Amount,
                request.RecipientKey,
                request.Description
            );

            Console.WriteLine($"[PixRequestConsumer] QR Code gerado: {qrCode}");

            // 3. Atualizar transação com sucesso
            transaction.Status = "COMPLETED";
            transaction.ExternalId = qrCode;
            transaction.UpdatedAt = DateTime.UtcNow;
            await _transactionRepository.UpdateAsync(transaction);

            // 4. Publicar evento de sucesso
            var successEvent = new
            {
                TransactionId = request.TransactionId,
                Status = "COMPLETED",
                QrCode = qrCode,
                Timestamp = DateTime.UtcNow
            };

            await _messageBroker.PublishAsync("pix-completed", successEvent);
            Console.WriteLine($"[PixRequestConsumer] Evento de sucesso publicado: {request.TransactionId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[PixRequestConsumer] Erro ao processar: {ex.Message}");

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

                await _messageBroker.PublishAsync("pix-failed", errorEvent);
                Console.WriteLine($"[PixRequestConsumer] Evento de erro publicado: {request.TransactionId}");
            }
            catch (Exception innerEx)
            {
                Console.WriteLine($"[PixRequestConsumer] Erro ao tratar falha: {innerEx.Message}");
            }
        }
    }
}

