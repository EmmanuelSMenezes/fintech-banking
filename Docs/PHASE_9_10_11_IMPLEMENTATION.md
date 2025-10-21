# 🚀 Implementação Rápida - Fases 9, 10, 11

## ⚡ Plano de Ação (Tokens Limitados)

Vamos implementar de forma **rápida e direta** as próximas 3 fases.

---

## 🎯 Fase 9: RabbitMQ Real (AGORA)

### Status Atual
- ✅ Consumers implementados
- ✅ ConsumerHost pronto
- ⏳ RabbitMqBroker é placeholder

### O Que Fazer
1. Implementar `RabbitMqBroker` com RabbitMQ.Client real
2. Testar com Docker
3. Validar fluxo completo

### Arquivo
```
src/FinTechBanking.Services/Messaging/RabbitMqBroker.cs
```

### Código Necessário
```csharp
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMqBroker : IMessageBroker
{
    private IConnection? _connection;
    private IModel? _channel;

    private void EnsureConnection()
    {
        if (_connection == null || !_connection.IsOpen)
        {
            var factory = new ConnectionFactory() { Uri = new Uri(_connectionString) };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }
    }

    public async Task PublishAsync<T>(string queueName, T message) where T : class
    {
        EnsureConnection();
        _channel!.QueueDeclare(queue: queueName, durable: true);
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);
        _channel.BasicPublish(exchange: "", routingKey: queueName, body: body);
        await Task.CompletedTask;
    }

    public async Task SubscribeAsync<T>(string queueName, Func<T, Task> handler) where T : class
    {
        EnsureConnection();
        _channel!.QueueDeclare(queue: queueName, durable: true);
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            var message = JsonSerializer.Deserialize<T>(json);
            if (message != null) await handler(message);
            _channel.BasicAck(ea.DeliveryTag, false);
        };
        _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        await Task.CompletedTask;
    }
}
```

---

## 🎯 Fase 10: Sicoob Real (DEPOIS)

### Status Atual
- ✅ SicoobBankService criado (mock)
- ⏳ Precisa integração real

### O Que Fazer
1. Obter credenciais Sicoob (você precisa fazer isso)
2. Implementar autenticação OAuth2
3. Implementar métodos reais

### Arquivo
```
src/FinTechBanking.Banking/Services/SicoobBankService.cs
```

### Estrutura
```csharp
public class SicoobBankService : ISicoobBankService
{
    private readonly HttpClient _httpClient;
    private string? _accessToken;

    public async Task<string> GeneratePixQrCodeAsync(decimal amount, string recipientKey, string description)
    {
        // 1. Obter token OAuth2
        // 2. Chamar API Sicoob
        // 3. Retornar QR Code
    }

    public async Task<bool> ProcessWithdrawalAsync(decimal amount, string accountNumber)
    {
        // 1. Validar conta
        // 2. Processar saque
        // 3. Retornar sucesso
    }
}
```

---

## 🎯 Fase 11: Frontend React (DEPOIS)

### Inicializar
```bash
npm create vite@latest fintech-frontend -- --template react
cd fintech-frontend
npm install
npm install react-router-dom axios
```

### Estrutura
```
fintech-frontend/
├── src/
│   ├── components/
│   │   ├── Auth/
│   │   │   ├── Login.jsx
│   │   │   └── Register.jsx
│   │   ├── Dashboard/
│   │   │   ├── Balance.jsx
│   │   │   └── History.jsx
│   │   └── Transactions/
│   │       ├── PixQrCode.jsx
│   │       └── Withdrawal.jsx
│   ├── pages/
│   ├── services/
│   │   └── api.js
│   ├── App.jsx
│   └── main.jsx
```

### API Service
```javascript
// src/services/api.js
const API_URL = 'https://localhost:5001/api';

export const login = async (email, password) => {
  const response = await fetch(`${API_URL}/auth/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password })
  });
  return response.json();
};

export const getBalance = async (token) => {
  const response = await fetch(`${API_URL}/accounts/balance`, {
    headers: { 'Authorization': `Bearer ${token}` }
  });
  return response.json();
};
```

---

## 📋 Checklist Rápido

### Fase 9
- [ ] Adicionar RabbitMQ.Client ao Services
- [ ] Implementar RabbitMqBroker real
- [ ] Compilar: `dotnet build`
- [ ] Testar: `docker-compose up -d`
- [ ] Testar fluxo PIX

### Fase 10
- [ ] Obter credenciais Sicoob
- [ ] Implementar autenticação
- [ ] Implementar GeneratePixQrCodeAsync
- [ ] Testar com sandbox

### Fase 11
- [ ] Criar projeto React
- [ ] Criar páginas de auth
- [ ] Criar dashboard
- [ ] Integrar com API

---

## 🚀 Comandos Rápidos

```bash
# Fase 9: Compilar
dotnet build

# Fase 9: Testar
docker-compose up -d
cd src/FinTechBanking.API && dotnet run
cd src/FinTechBanking.ConsumerWorker && dotnet run

# Fase 11: Criar React
npm create vite@latest fintech-frontend -- --template react
cd fintech-frontend
npm install
npm run dev
```

---

## 📊 Timeline

| Fase | Tempo | Status |
|------|-------|--------|
| 9 | 30 min | ⏳ Agora |
| 10 | 1-2 h | ⏳ Depois |
| 11 | 2-3 h | ⏳ Depois |

---

## 💡 Dicas

1. **Fase 9:** Copie o código acima e substitua em RabbitMqBroker.cs
2. **Fase 10:** Você precisa das credenciais Sicoob (contate o banco)
3. **Fase 11:** Use Vite para criar React rápido

---

**Próximo Passo:** Implementar Fase 9 agora!

*Última atualização: 2025-10-21*

