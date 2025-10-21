# 🚀 Roadmap - Próximas Fases

## Fase 9: RabbitMQ Real (1-2 semanas)

### Objetivo
Conectar com RabbitMQ real e processar mensagens assincronamente

### Tarefas
1. **Implementar RabbitMqBroker Real**
   - Arquivo: `src/FinTechBanking.Services/Messaging/RabbitMqBroker.cs`
   - Usar: `RabbitMQ.Client` (já adicionado)
   - Implementar: `PublishAsync` e `SubscribeAsync` com conexão real

2. **Testar Fluxo Completo**
   - Iniciar Docker: `docker-compose up -d`
   - Iniciar API: `dotnet run` (API)
   - Iniciar Consumer: `dotnet run` (ConsumerWorker)
   - Testar com curl

### Código Exemplo
```csharp
// RabbitMqBroker.cs - PublishAsync
public async Task PublishAsync<T>(string queueName, T message) where T : class
{
    var factory = new ConnectionFactory() { Uri = new Uri(_connectionString) };
    using var connection = factory.CreateConnection();
    using var channel = connection.CreateModel();
    
    channel.QueueDeclare(queue: queueName, durable: true);
    var json = JsonSerializer.Serialize(message);
    var body = Encoding.UTF8.GetBytes(json);
    
    channel.BasicPublish(exchange: "", routingKey: queueName, body: body);
}
```

---

## Fase 10: Integração Sicoob (2-3 semanas)

### Objetivo
Integrar com API real do Sicoob

### Tarefas
1. **Obter Credenciais**
   - Contatar Sicoob
   - Obter: API Key, API URL, Client ID, Client Secret

2. **Implementar Autenticação**
   - Arquivo: `src/FinTechBanking.Banking/Services/SicoobBankService.cs`
   - Implementar: OAuth2 com Sicoob

3. **Implementar Métodos Reais**
   - `GeneratePixQrCodeAsync()` - Chamar API Sicoob
   - `ProcessWithdrawalAsync()` - Chamar API Sicoob
   - `GetBalanceAsync()` - Chamar API Sicoob

4. **Testar com Sandbox**
   - Usar ambiente de sandbox Sicoob
   - Validar assinatura de webhooks

### Configuração
```json
// appsettings.json
{
  "Banking": {
    "Sicoob": {
      "ApiKey": "seu-api-key",
      "ApiUrl": "https://api.sicoob.com.br",
      "ClientId": "seu-client-id",
      "ClientSecret": "seu-client-secret"
    }
  }
}
```

---

## Fase 11: Frontend React (2-3 semanas)

### Objetivo
Criar interface web para usuários

### Tarefas
1. **Inicializar Projeto React**
   ```bash
   npm create vite@latest fintech-frontend -- --template react
   cd fintech-frontend
   npm install
   ```

2. **Estrutura de Pastas**
   ```
   src/
   ├── components/
   │   ├── Auth/
   │   │   ├── Login.jsx
   │   │   └── Register.jsx
   │   ├── Dashboard/
   │   │   ├── Balance.jsx
   │   │   └── History.jsx
   │   └── Transactions/
   │       ├── PixQrCode.jsx
   │       └── Withdrawal.jsx
   ├── pages/
   ├── services/
   │   └── api.js
   ├── App.jsx
   └── main.jsx
   ```

3. **Páginas Principais**
   - Login/Register
   - Dashboard (Saldo + Histórico)
   - PIX QR Code
   - Saque
   - Perfil

4. **Integração com API**
   ```javascript
   // services/api.js
   const API_URL = 'https://localhost:5001/api';
   
   export const login = async (email, password) => {
     const response = await fetch(`${API_URL}/auth/login`, {
       method: 'POST',
       headers: { 'Content-Type': 'application/json' },
       body: JSON.stringify({ email, password })
     });
     return response.json();
   };
   ```

---

## Fase 12: Testes (1-2 semanas)

### Objetivo
Garantir qualidade e confiabilidade

### Tarefas
1. **Testes Unitários**
   - Usar: xUnit
   - Cobertura: >80%
   - Arquivos: `*.Tests.cs`

2. **Testes de Integração**
   - Testar fluxo completo
   - Testar com banco de dados real
   - Testar com RabbitMQ real

3. **Testes de Carga**
   - Usar: k6 ou Apache JMeter
   - Simular: 1000+ requisições/segundo

4. **Testes de Segurança**
   - Validar JWT
   - Testar CORS
   - Testar SQL Injection

### Exemplo Teste Unitário
```csharp
[Fact]
public async Task GeneratePixQrCode_WithValidData_ReturnsQrCode()
{
    // Arrange
    var service = new SicoobBankService(_apiKey, _apiUrl);
    
    // Act
    var result = await service.GeneratePixQrCodeAsync(100, "user@example.com", "Test");
    
    // Assert
    Assert.NotNull(result);
    Assert.NotEmpty(result);
}
```

---

## Timeline Estimado

| Fase | Duração | Início | Fim |
|------|---------|--------|-----|
| 9 | 1-2 sem | Agora | +2 sem |
| 10 | 2-3 sem | +2 sem | +5 sem |
| 11 | 2-3 sem | +5 sem | +8 sem |
| 12 | 1-2 sem | +8 sem | +10 sem |
| **Total** | **~10 semanas** | | |

---

## 📋 Checklist por Fase

### Fase 9
- [ ] Implementar RabbitMqBroker real
- [ ] Testar PublishAsync
- [ ] Testar SubscribeAsync
- [ ] Testar fluxo PIX completo
- [ ] Testar fluxo Saque completo
- [ ] Documentar

### Fase 10
- [ ] Obter credenciais Sicoob
- [ ] Implementar autenticação
- [ ] Implementar GeneratePixQrCodeAsync
- [ ] Implementar ProcessWithdrawalAsync
- [ ] Testar com sandbox
- [ ] Validar webhooks

### Fase 11
- [ ] Inicializar React
- [ ] Criar páginas de auth
- [ ] Criar dashboard
- [ ] Criar transações
- [ ] Integrar com API
- [ ] Testar fluxo completo

### Fase 12
- [ ] Escrever testes unitários
- [ ] Escrever testes integração
- [ ] Testes de carga
- [ ] Testes de segurança
- [ ] Cobertura >80%
- [ ] Documentar testes

---

## 🎯 Dicas Importantes

1. **Sempre compile antes de começar**
   ```bash
   dotnet build
   ```

2. **Use logging para debug**
   ```csharp
   _logger.LogInformation("Debug message");
   ```

3. **Teste com curl antes de integrar**
   ```bash
   curl -X POST https://localhost:5001/api/...
   ```

4. **Mantenha documentação atualizada**
   - Atualize README.md
   - Atualize API_EXAMPLES.md

5. **Use versionamento de API**
   - `/api/v1/...`
   - `/api/v2/...`

---

## 📞 Referências

- [RabbitMQ .NET Client](https://www.rabbitmq.com/dotnet-api-guide.html)
- [Sicoob API Docs](https://www.sicoob.com.br/api)
- [React Documentation](https://react.dev)
- [xUnit Documentation](https://xunit.net)

---

*Última atualização: 2025-10-21*

