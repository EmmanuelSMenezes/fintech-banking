# 🏗️ Arquitetura do FinTech Banking Gateway

## Visão Geral

O FinTech Banking Gateway é um sistema de pagamentos bancários construído com uma arquitetura em camadas, separando responsabilidades e permitindo escalabilidade.

```
┌─────────────────────────────────────────────────────────────┐
│                    Cliente / Frontend                        │
└────────────────────────┬────────────────────────────────────┘
                         │
┌────────────────────────▼────────────────────────────────────┐
│              FinTechBanking.API (REST)                       │
│  ├─ AuthController (Autenticação)                           │
│  ├─ TransactionsController (Operações)                      │
│  ├─ AccountsController (Contas)                             │
│  └─ WebhooksController (Notificações)                       │
└────────────────────────┬────────────────────────────────────┘
                         │
        ┌────────────────┼────────────────┐
        │                │                │
   ┌────▼────┐    ┌─────▼──────┐   ┌────▼────┐
   │ PIX QR  │    │ Withdrawal │   │ Webhooks│
   │ Requests│    │ Requests   │   │ Events  │
   └────┬────┘    └─────┬──────┘   └────┬────┘
        │                │               │
        └────────────────┼───────────────┘
                         │
        ┌────────────────▼────────────────┐
        │   FinTechBanking.Services       │
        │  ├─ RabbitMqBroker              │
        │  └─ AuthService                 │
        └────────────────┬────────────────┘
                         │
        ┌────────────────▼────────────────┐
        │   FinTechBanking.Banking        │
        │  ├─ BankingHub                  │
        │  └─ SicoobBankService           │
        └────────────────┬────────────────┘
                         │
        ┌────────────────▼────────────────┐
        │   FinTechBanking.Data           │
        │  ├─ UserRepository              │
        │  ├─ AccountRepository           │
        │  └─ TransactionRepository       │
        └────────────────┬────────────────┘
                         │
        ┌────────────────▼────────────────┐
        │   FinTechBanking.Core           │
        │  ├─ Entities                    │
        │  ├─ DTOs                        │
        │  └─ Interfaces                  │
        └─────────────────────────────────┘
```

## 📦 Estrutura de Projetos

### 1. **FinTechBanking.Core**
Contém a lógica de negócio e contratos.

**Pastas:**
- `Entities/` - Modelos de dados (User, Account, Transaction, WebhookLog)
- `DTOs/` - Data Transfer Objects (AuthDtos, TransactionDtos)
- `Interfaces/` - Contratos (IUserRepository, IAuthService, IBankingHub, etc)

### 2. **FinTechBanking.Data**
Camada de acesso a dados com Dapper ORM.

**Pastas:**
- `Repositories/` - Implementações de repositórios (UserRepository, AccountRepository, TransactionRepository)
- `Migrations/` - Scripts SQL para criar tabelas

**Tecnologias:**
- Dapper ORM
- Npgsql (PostgreSQL)

### 3. **FinTechBanking.Services**
Serviços de negócio e mensageria.

**Pastas:**
- `Auth/` - AuthService (autenticação, JWT, hash de senhas)
- `Messaging/` - RabbitMqBroker (integração com RabbitMQ)

### 4. **FinTechBanking.Banking**
Integrações bancárias.

**Pastas:**
- `Hub/` - BankingHub (abstração bancária, roteamento)
- `Services/` - SicoobBankService (integração específica com Sicoob)

### 5. **FinTechBanking.API**
API REST principal.

**Pastas:**
- `Controllers/` - Endpoints REST
  - `AuthController` - Autenticação
  - `TransactionsController` - Operações de pagamento
  - `AccountsController` - Gerenciamento de contas
  - `WebhooksController` - Recebimento de notificações

**Configuração:**
- `Program.cs` - Setup da aplicação, DI, autenticação
- `appsettings.json` - Configurações

## 🔄 Fluxos Principais

### 1. Autenticação (JWT)

```
POST /api/auth/register
  ↓
AuthService.RegisterAsync()
  ↓
UserRepository.CreateAsync()
  ↓
Usuário criado no banco
  ↓
Retorna RegisterResponse

POST /api/auth/login
  ↓
AuthService.LoginAsync()
  ↓
Valida credenciais
  ↓
Gera JWT Token
  ↓
Retorna LoginResponse com token
```

### 2. Geração de QR Code PIX

```
POST /api/transactions/pix-qrcode
  ↓
Valida autenticação (JWT)
  ↓
Obtém conta do usuário
  ↓
Cria Transaction (PENDING)
  ↓
Publica em "pix-requests" (RabbitMQ)
  ↓
Retorna QR Code ao cliente
```

### 3. Saque

```
POST /api/transactions/withdrawal
  ↓
Valida autenticação
  ↓
Verifica saldo
  ↓
Cria Transaction (PENDING)
  ↓
Publica em "withdrawal-requests" (RabbitMQ)
  ↓
Retorna status ao cliente
```

### 4. Webhook do Banco

```
POST /api/webhooks/sicoob
  ↓
Valida assinatura
  ↓
Publica em "webhook-events" (RabbitMQ)
  ↓
Consumer processa evento
  ↓
Atualiza Transaction
  ↓
Notifica cliente
```

## 🗄️ Banco de Dados

### Tabelas

- **users** - Usuários do sistema
- **accounts** - Contas bancárias
- **transactions** - Transações (PIX, saques, etc)
- **webhook_logs** - Log de webhooks recebidos

### Relacionamentos

```
users (1) ──── (N) accounts
accounts (1) ──── (N) transactions
transactions (1) ──── (N) webhook_logs
```

## 🔐 Segurança

- ✅ **Autenticação JWT** - Tokens com expiração
- ✅ **Hash de Senhas** - SHA256
- ✅ **CORS** - Configurado para aceitar requisições
- ✅ **Validação de Entrada** - DTOs com validação
- 🔄 **Assinatura de Webhooks** - TODO
- 🔄 **Rate Limiting** - TODO
- 🔄 **Antifraude** - TODO

## 📊 Padrões de Design

- **Repository Pattern** - Abstração de acesso a dados
- **Dependency Injection** - Injeção de dependências no Program.cs
- **DTO Pattern** - Separação entre modelos internos e externos
- **Service Layer** - Lógica de negócio centralizada
- **Hub Pattern** - Abstração de múltiplos bancos

## 🚀 Próximas Implementações

### Curto Prazo
- [ ] Implementar Consumer de Requisições (Worker Service)
- [ ] Implementar Consumer de Webhooks (Worker Service)
- [ ] Integração real com API Sicoob
- [ ] Validação de assinatura de webhooks

### Médio Prazo
- [ ] Frontend React
- [ ] Suporte a Boleto
- [ ] Suporte a TED
- [ ] Testes unitários e de integração

### Longo Prazo
- [ ] Suporte a Stark Bank
- [ ] Suporte a Efi Bank
- [ ] Dashboard de analytics
- [ ] Sistema de antifraude
- [ ] Rate limiting
- [ ] Cache distribuído (Redis)

## 📚 Referências

- [.NET 9 Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [Dapper ORM](https://github.com/DapperLib/Dapper)
- [RabbitMQ](https://www.rabbitmq.com/)
- [PostgreSQL](https://www.postgresql.org/)
- [JWT Authentication](https://jwt.io/)

