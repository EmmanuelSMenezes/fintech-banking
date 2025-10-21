using FinTechBanking.Core.Interfaces;
using FinTechBanking.Core.DTOs;
using FinTechBanking.Workers.Consumers;

namespace FinTechBanking.Workers;

/// <summary>
/// Host para gerenciar todos os consumers
/// </summary>
public class ConsumerHost
{
    private readonly PixRequestConsumer _pixRequestConsumer;
    private readonly WithdrawalRequestConsumer _withdrawalRequestConsumer;
    private readonly WebhookEventConsumer _webhookEventConsumer;
    private readonly IMessageBroker _messageBroker;

    public ConsumerHost(
        PixRequestConsumer pixRequestConsumer,
        WithdrawalRequestConsumer withdrawalRequestConsumer,
        WebhookEventConsumer webhookEventConsumer,
        IMessageBroker messageBroker)
    {
        _pixRequestConsumer = pixRequestConsumer;
        _withdrawalRequestConsumer = withdrawalRequestConsumer;
        _webhookEventConsumer = webhookEventConsumer;
        _messageBroker = messageBroker;
    }

    /// <summary>
    /// Inicia todos os consumers
    /// </summary>
    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("[ConsumerHost] Iniciando consumers...");

        try
        {
            // Iniciar consumers em paralelo
            var tasks = new List<Task>
            {
                StartPixRequestConsumerAsync(cancellationToken),
                StartWithdrawalRequestConsumerAsync(cancellationToken),
                StartWebhookEventConsumerAsync(cancellationToken)
            };

            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ConsumerHost] Erro ao iniciar consumers: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Inicia o consumer de requisições PIX
    /// </summary>
    private async Task StartPixRequestConsumerAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("[ConsumerHost] Iniciando PixRequestConsumer...");

        try
        {
            await _messageBroker.SubscribeAsync<PixQrCodeRequestDto>(
                "pix-requests",
                async (request) => await _pixRequestConsumer.ProcessAsync(request)
            );
            Console.WriteLine("[ConsumerHost] PixRequestConsumer iniciado");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ConsumerHost] Erro ao iniciar PixRequestConsumer: {ex.Message}");
        }
    }

    /// <summary>
    /// Inicia o consumer de requisições de saque
    /// </summary>
    private async Task StartWithdrawalRequestConsumerAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("[ConsumerHost] Iniciando WithdrawalRequestConsumer...");

        try
        {
            await _messageBroker.SubscribeAsync<WithdrawalRequestDto>(
                "withdrawal-requests",
                async (request) => await _withdrawalRequestConsumer.ProcessAsync(request)
            );
            Console.WriteLine("[ConsumerHost] WithdrawalRequestConsumer iniciado");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ConsumerHost] Erro ao iniciar WithdrawalRequestConsumer: {ex.Message}");
        }
    }

    /// <summary>
    /// Inicia o consumer de eventos de webhook
    /// </summary>
    private async Task StartWebhookEventConsumerAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("[ConsumerHost] Iniciando WebhookEventConsumer...");

        try
        {
            await _messageBroker.SubscribeAsync<WebhookEventDto>(
                "webhook-events",
                async (webhookEvent) => await _webhookEventConsumer.ProcessAsync(webhookEvent)
            );
            Console.WriteLine("[ConsumerHost] WebhookEventConsumer iniciado");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ConsumerHost] Erro ao iniciar WebhookEventConsumer: {ex.Message}");
        }
    }

    /// <summary>
    /// Para todos os consumers
    /// </summary>
    public async Task StopAsync()
    {
        Console.WriteLine("[ConsumerHost] Parando consumers...");

        try
        {
            // TODO: Implementar stop real
            await Task.CompletedTask;
            Console.WriteLine("[ConsumerHost] Consumers parados");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ConsumerHost] Erro ao parar consumers: {ex.Message}");
        }
    }
}

