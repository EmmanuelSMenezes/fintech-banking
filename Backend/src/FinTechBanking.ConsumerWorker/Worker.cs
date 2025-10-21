using FinTechBanking.Workers;

namespace FinTechBanking.ConsumerWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Consumer Worker iniciando...");

        try
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var consumerHost = scope.ServiceProvider.GetRequiredService<ConsumerHost>();
                await consumerHost.StartAsync(stoppingToken);
                _logger.LogInformation("Consumer Worker iniciado com sucesso");

                // Manter o worker rodando
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);
                }

                await consumerHost.StopAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao iniciar Consumer Worker");
            throw;
        }
        finally
        {
            _logger.LogInformation("Consumer Worker parado");
        }
    }
}
