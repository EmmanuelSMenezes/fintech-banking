# 🧪 Fase 12: Testes - Guia Completo

## 🎯 Objetivo

Implementar testes para garantir qualidade e confiabilidade do sistema.

---

## 📋 Tipos de Testes

### 1. Testes Unitários
- Testar métodos individuais
- Usar mocks para dependências
- Cobertura: >80%

### 2. Testes de Integração
- Testar fluxo completo
- Usar banco de dados real
- Testar com RabbitMQ

### 3. Testes E2E
- Testar frontend + backend
- Usar Cypress ou Playwright
- Simular usuário real

### 4. Testes de Carga
- Testar performance
- Usar k6 ou Apache JMeter
- Simular múltiplos usuários

---

## 🚀 Implementar Testes Unitários

### 1. Criar Projeto de Testes

```bash
dotnet new xunit -n FinTechBanking.Tests -o src/FinTechBanking.Tests
dotnet sln add src/FinTechBanking.Tests/FinTechBanking.Tests.csproj
cd src/FinTechBanking.Tests
dotnet add package Moq
dotnet add reference ../FinTechBanking.Core/FinTechBanking.Core.csproj
dotnet add reference ../FinTechBanking.Services/FinTechBanking.Services.csproj
```

### 2. Teste de Autenticação

```csharp
// AuthServiceTests.cs
using Xunit;
using Moq;
using FinTechBanking.Services;

public class AuthServiceTests
{
    [Fact]
    public async Task Login_WithValidCredentials_ReturnsToken()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        var service = new AuthService(mockUserRepository.Object);
        
        // Act
        var result = await service.LoginAsync("user@example.com", "Pass123!");
        
        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ThrowsException()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        var service = new AuthService(mockUserRepository.Object);
        
        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => service.LoginAsync("user@example.com", "WrongPassword")
        );
    }
}
```

### 3. Teste de Transações

```csharp
// TransactionServiceTests.cs
[Fact]
public async Task GeneratePixQrCode_WithValidData_ReturnsQrCode()
{
    // Arrange
    var mockBankingHub = new Mock<IBankingHub>();
    mockBankingHub
        .Setup(x => x.GeneratePixQrCodeAsync(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync("00020126580014br.gov.bcb.pix...");
    
    var service = new TransactionService(mockBankingHub.Object);
    
    // Act
    var result = await service.GeneratePixQrCodeAsync("SICOOB", 100, "user@example.com", "Test");
    
    // Assert
    Assert.NotNull(result);
    Assert.StartsWith("00020126", result);
}
```

---

## 🧪 Testes de Integração

### 1. Teste de Fluxo Completo

```csharp
// IntegrationTests.cs
[Fact]
public async Task CompletePixFlow_Success()
{
    // Arrange
    var user = await CreateTestUser();
    var account = await CreateTestAccount(user.Id);
    
    // Act
    var qrCode = await _transactionService.GeneratePixQrCodeAsync(
        "SICOOB", 100, "recipient@example.com", "Test"
    );
    
    // Assert
    Assert.NotNull(qrCode);
    var transaction = await _transactionRepository.GetByIdAsync(qrCode);
    Assert.Equal("COMPLETED", transaction.Status);
}
```

### 2. Teste de Webhook

```csharp
[Fact]
public async Task ProcessWebhook_UpdatesTransactionStatus()
{
    // Arrange
    var transaction = await CreateTestTransaction();
    var webhook = new WebhookEventDto
    {
        TransactionId = transaction.Id,
        Status = "COMPLETED",
        ExternalId = "ext-123"
    };
    
    // Act
    await _webhookConsumer.ProcessAsync(webhook);
    
    // Assert
    var updated = await _transactionRepository.GetByIdAsync(transaction.Id);
    Assert.Equal("COMPLETED", updated.Status);
}
```

---

## 🎯 Testes Frontend

### 1. Teste de Login

```javascript
// Login.test.jsx
import { render, screen, fireEvent } from '@testing-library/react';
import Login from '../components/Auth/Login';

test('Login form submits with email and password', async () => {
  render(<Login />);
  
  const emailInput = screen.getByLabelText(/email/i);
  const passwordInput = screen.getByLabelText(/senha/i);
  const submitButton = screen.getByRole('button', { name: /entrar/i });
  
  fireEvent.change(emailInput, { target: { value: 'user@example.com' } });
  fireEvent.change(passwordInput, { target: { value: 'Pass123!' } });
  fireEvent.click(submitButton);
  
  // Assert
  expect(submitButton).toBeDisabled();
});
```

### 2. Teste de Dashboard

```javascript
// Dashboard.test.jsx
test('Dashboard displays balance', async () => {
  render(<Dashboard />);
  
  const balance = await screen.findByText(/R\$ 1000\.00/);
  expect(balance).toBeInTheDocument();
});
```

---

## 📊 Cobertura de Testes

### Executar com Cobertura

```bash
# Backend
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover

# Frontend
npm test -- --coverage
```

### Metas

- Cobertura geral: >80%
- Cobertura de serviços: >90%
- Cobertura de controllers: >85%

---

## 🔄 Testes de Carga

### Usar k6

```bash
npm install -g k6
```

### Script k6

```javascript
// load-test.js
import http from 'k6/http';
import { check } from 'k6';

export let options = {
  vus: 100,
  duration: '30s',
};

export default function () {
  let response = http.post('https://localhost:5001/api/auth/login', {
    email: 'user@example.com',
    password: 'Pass123!'
  });
  
  check(response, {
    'status is 200': (r) => r.status === 200,
  });
}
```

### Executar

```bash
k6 run load-test.js
```

---

## ✅ Checklist de Testes

### Unitários
- [ ] AuthService
- [ ] TransactionService
- [ ] BankingHub
- [ ] Repositories
- [ ] Validators

### Integração
- [ ] Fluxo PIX completo
- [ ] Fluxo Saque completo
- [ ] Fluxo Webhook
- [ ] Autenticação
- [ ] Autorização

### Frontend
- [ ] Login
- [ ] Register
- [ ] Dashboard
- [ ] Transações
- [ ] Logout

### Carga
- [ ] 100 usuários simultâneos
- [ ] 1000 requisições/segundo
- [ ] Tempo de resposta <500ms

---

## 🚀 Executar Todos os Testes

```bash
# Backend
dotnet test

# Frontend
npm test

# Cobertura
dotnet test /p:CollectCoverage=true
npm test -- --coverage

# Carga
k6 run load-test.js
```

---

## 📈 Métricas de Sucesso

- ✅ Cobertura >80%
- ✅ Todos os testes passam
- ✅ Tempo de resposta <500ms
- ✅ 0 erros críticos
- ✅ 0 warnings de segurança

---

## 💡 Dicas

1. **Sempre escreva testes antes do código** (TDD)
2. **Use mocks para dependências externas**
3. **Teste casos de sucesso e erro**
4. **Mantenha testes simples e focados**
5. **Execute testes frequentemente**

---

**Próximo Passo:** Implementar testes!

*Última atualização: 2025-10-21*

