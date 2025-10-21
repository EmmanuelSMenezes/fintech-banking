# 🚀 Fase 9: Implementação Real de RabbitMQ

## 📋 Objetivo

Implementar a integração real com RabbitMQ para processar mensagens de forma assíncrona e confiável.

---

## 🎯 Tarefas

### 1. Implementar RabbitMqBroker Real

**Arquivo:** `src/FinTechBanking.Services/Messaging/RabbitMqBroker.cs`

Atualmente é um placeholder. Precisa implementar:

```csharp
public class RabbitMqBroker : IMessageBroker
{
    private IConnection _connection;
    private IModel _channel;

    public async Task PublishAsync<T>(string queueName, T message) where T : class
    {
        // 1. Conectar ao RabbitMQ
        // 2. Declarar fila
        // 3. Serializar mensagem
        // 4. Publicar
    }

    public async Task SubscribeAsync<T>(string queueName, Func<T, Task> handler) where T : class
    {
        // 1. Conectar ao RabbitMQ
        // 2. Declarar fila
        // 3. Criar consumer
        // 4. Processar mensagens
    }
}
```

### 2. Implementar Subscribe em ConsumerHost

**Arquivo:** `src/FinTechBanking.Workers/ConsumerHost.cs`

Atualmente os métodos são placeholders. Precisa:

```csharp
private async Task StartPixRequestConsumerAsync(CancellationToken cancellationToken)
{
    await _messageBroker.SubscribeAsync<PixQrCodeRequestDto>(
        "pix-requests",
        async (request) => await _pixRequestConsumer.ProcessAsync(request)
    );
}

private async Task StartWithdrawalRequestConsumerAsync(CancellationToken cancellationToken)
{
    await _messageBroker.SubscribeAsync<WithdrawalRequestDto>(
        "withdrawal-requests",
        async (request) => await _withdrawalRequestConsumer.ProcessAsync(request)
    );
}

private async Task StartWebhookEventConsumerAsync(CancellationToken cancellationToken)
{
    await _messageBroker.SubscribeAsync<WebhookEventDto>(
        "webhook-events",
        async (webhookEvent) => await _webhookEventConsumer.ProcessAsync(webhookEvent)
    );
}
```

### 3. Testar Fluxo Completo

**Teste Manual:**

1. Iniciar Docker Compose
```bash
docker-compose up -d
```

2. Iniciar API
```bash
cd src/FinTechBanking.API
dotnet run
```

3. Iniciar ConsumerWorker (outro terminal)
```bash
cd src/FinTechBanking.ConsumerWorker
dotnet run
```

4. Registrar usuário
```bash
curl -X POST https://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email":"user@example.com",
    "password":"Pass123!",
    "fullName":"John Doe",
    "document":"12345678901",
    "phoneNumber":"11999999999"
  }'
```

5. Fazer login
```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email":"user@example.com",
    "password":"Pass123!"
  }'
```

6. Gerar QR Code PIX (usar token do login)
```bash
curl -X POST https://localhost:5001/api/transactions/pix-qrcode \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "amount":100.00,
    "description":"Pagamento teste",
    "recipientKey":"user@example.com"
  }'
```

7. Verificar logs do ConsumerWorker
- Deve processar a mensagem da fila
- Deve gerar QR Code
- Deve atualizar status da transação

---

## 📦 Dependências Necessárias

Já instaladas:
- ✅ RabbitMQ.Client 7.1.2

---

## 🔄 Fluxo de Mensagens

```
┌─────────────────────────────────────────────────────────────┐
│                    API REST                                 │
│  POST /api/transactions/pix-qrcode                          │
└────────────────────┬────────────────────────────────────────┘
                     │
                     ▼
        ┌────────────────────────────┐
        │  RabbitMqBroker.Publish    │
        │  Queue: pix-requests       │
        └────────────────┬───────────┘
                         │
                         ▼
        ┌────────────────────────────┐
        │  RabbitMQ Message Queue    │
        │  (pix-requests)            │
        └────────────────┬───────────┘
                         │
                         ▼
        ┌────────────────────────────┐
        │  ConsumerWorker            │
        │  PixRequestConsumer        │
        │  .ProcessAsync()           │
        └────────────────┬───────────┘
                         │
        ┌────────────────┼────────────────┐
        │                │                │
        ▼                ▼                ▼
    ┌────────┐      ┌──────────┐    ┌──────────┐
    │Banking │      │Database  │    │RabbitMQ  │
    │  Hub   │      │(Update)  │    │(Publish) │
    └────────┘      └──────────┘    └──────────┘
```

---

## 🧪 Testes Recomendados

### Teste 1: Publicar e Consumir Mensagem
```csharp
[Test]
public async Task PublishAndConsumePixRequest()
{
    // Arrange
    var request = new PixQrCodeRequestDto { ... };
    
    // Act
    await _messageBroker.PublishAsync("pix-requests", request);
    
    // Assert
    // Verificar se mensagem foi processada
}
```

### Teste 2: Validar Processamento
```csharp
[Test]
public async Task ProcessPixRequestSuccessfully()
{
    // Arrange
    var consumer = new PixRequestConsumer(...);
    var request = new PixQrCodeRequestDto { ... };
    
    // Act
    await consumer.ProcessAsync(request);
    
    // Assert
    // Verificar se transação foi atualizada
}
```

### Teste 3: Tratamento de Erros
```csharp
[Test]
public async Task HandlePixRequestError()
{
    // Arrange
    var consumer = new PixRequestConsumer(...);
    var invalidRequest = new PixQrCodeRequestDto { TransactionId = Guid.Empty };
    
    // Act & Assert
    // Verificar se erro foi tratado corretamente
}
```

---

## 📝 Checklist

- [ ] Implementar RabbitMqBroker.PublishAsync
- [ ] Implementar RabbitMqBroker.SubscribeAsync
- [ ] Implementar ConsumerHost.StartPixRequestConsumerAsync
- [ ] Implementar ConsumerHost.StartWithdrawalRequestConsumerAsync
- [ ] Implementar ConsumerHost.StartWebhookEventConsumerAsync
- [ ] Testar fluxo PIX completo
- [ ] Testar fluxo Withdrawal completo
- [ ] Testar fluxo Webhook completo
- [ ] Tratamento de erros e retry
- [ ] Logging estruturado
- [ ] Documentação de operação

---

## 🚀 Como Começar

1. Leia este documento
2. Implemente RabbitMqBroker
3. Implemente ConsumerHost
4. Execute testes manuais
5. Valide fluxo completo

---

## 📚 Referências

- [RabbitMQ.Client Documentation](https://www.rabbitmq.com/dotnet-api-guide.html)
- [RabbitMQ Tutorials](https://www.rabbitmq.com/getstarted.html)
- [Async/Await Best Practices](https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming)

---

*Próxima Fase: Fase 10 - Integração Sicoob*

