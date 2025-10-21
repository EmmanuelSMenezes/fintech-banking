namespace FinTechBanking.Core.Interfaces;

public interface IMessageBroker
{
    Task PublishAsync<T>(string queueName, T message) where T : class;
    Task SubscribeAsync<T>(string queueName, Func<T, Task> handler) where T : class;
}

