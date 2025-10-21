# 🔧 Resumo Técnico - FinTech Banking

## Arquitetura

```
┌─────────────────────────────────────────────────────────────┐
│                    FRONTEND LAYER                           │
├─────────────────────────────────────────────────────────────┤
│  Internet Banking (3000)  │  Backoffice (3001)              │
│  Next.js 15 + React 18    │  Next.js 15 + React 18          │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│                    API LAYER                                │
├─────────────────────────────────────────────────────────────┤
│  API Cliente (5167)       │  API Interna (5036)             │
│  .NET 9 Web API           │  .NET 9 Web API                 │
│  JWT Auth                 │  JWT Auth + Admin               │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│                  BUSINESS LOGIC LAYER                       │
├─────────────────────────────────────────────────────────────┤
│  Services (Email, Auth, Banking)                            │
│  Repository Pattern with Dapper ORM                         │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│                  DATA & MESSAGE LAYER                       │
├─────────────────────────────────────────────────────────────┤
│  PostgreSQL (5432)        │  RabbitMQ (5672)                │
│  4 Tables + Indexes       │  Message Broker                 │
│  Transactions             │  Consumer Worker                │
└─────────────────────────────────────────────────────────────┘
```

## Stack Tecnológico

### Backend
- **Framework:** .NET 9
- **ORM:** Dapper 2.1.66
- **Database:** PostgreSQL 15
- **Authentication:** JWT Bearer
- **Password Hashing:** BCrypt.Net-Next 4.0.3
- **Message Broker:** RabbitMQ 3
- **Email:** SMTP (Gmail, Outlook, SendGrid)
- **API Documentation:** Swagger/OpenAPI

### Frontend
- **Framework:** Next.js 15.2.3
- **UI Library:** React 18
- **Styling:** Tailwind CSS
- **Language:** TypeScript
- **Package Manager:** npm

### Infrastructure
- **Containerization:** Docker & Docker Compose
- **Database:** PostgreSQL 15-alpine
- **Message Broker:** RabbitMQ 3-management-alpine

## Projetos .NET

```
FinTechBanking.sln
├── FinTechBanking.Core
│   ├── Entities (User, Account, Transaction, WebhookLog)
│   ├── DTOs (AuthDtos, TransactionDtos)
│   └── Interfaces (IUserRepository, ITransactionRepository, etc)
│
├── FinTechBanking.Data
│   ├── Repositories (UserRepository, TransactionRepository)
│   ├── Migrations (001_InitialSchema.sql)
│   └── DapperConfiguration.cs
│
├── FinTechBanking.Services
│   ├── Email (IEmailService, SmtpEmailService)
│   ├── Auth (AuthService)
│   └── Banking (BankingHub, SicoobService)
│
├── FinTechBanking.API.Cliente
│   ├── Controllers (AuthController, TransactionsController)
│   ├── Program.cs (DI, CORS, JWT)
│   └── appsettings.json
│
├── FinTechBanking.API.Interna
│   ├── Controllers (AdminController)
│   ├── Program.cs (DI, CORS, JWT, Email)
│   └── appsettings.json
│
├── FinTechBanking.ConsumerWorker
│   └── RabbitMQ Consumer
│
└── FinTechBanking.Workers
    └── Background Services
```

## Endpoints Implementados

### API Cliente (5167)

| Método | Endpoint | Autenticação | Descrição |
|--------|----------|--------------|-----------|
| POST | `/api/auth/register` | ❌ | Registrar cliente |
| POST | `/api/auth/login` | ❌ | Login com JWT |
| GET | `/api/transactions/balance` | ✅ | Consultar saldo |
| GET | `/api/transactions/history` | ✅ | Histórico |
| POST | `/api/transactions/pix/qrcode` | ✅ | Gerar QR Code |
| POST | `/api/transactions/withdrawal` | ✅ | Solicitar saque |
| GET | `/api/transactions/{id}/status` | ✅ | Status |

### API Interna (5036)

| Método | Endpoint | Autenticação | Descrição |
|--------|----------|--------------|-----------|
| POST | `/api/admin/users` | ✅ | Criar usuário |
| GET | `/api/admin/users` | ✅ | Listar usuários |
| GET | `/api/admin/users/{id}` | ✅ | Detalhes |
| GET | `/api/admin/transactions` | ✅ | Listar transações |
| GET | `/api/admin/reports/transactions` | ✅ | Relatório |
| GET | `/api/admin/dashboard` | ✅ | Dashboard |

## Banco de Dados

### Tabelas

```sql
users
├── id (UUID, PK)
├── email (VARCHAR, UNIQUE)
├── password_hash (VARCHAR)
├── full_name (VARCHAR)
├── document (VARCHAR, UNIQUE)
├── phone_number (VARCHAR)
├── is_active (BOOLEAN)
├── webhook_url (VARCHAR)
├── created_at (TIMESTAMP)
└── updated_at (TIMESTAMP)

accounts
├── id (UUID, PK)
├── user_id (UUID, FK → users)
├── account_number (VARCHAR)
├── bank_code (VARCHAR)
├── balance (DECIMAL)
├── is_active (BOOLEAN)
├── created_at (TIMESTAMP)
└── updated_at (TIMESTAMP)

transactions
├── id (UUID, PK)
├── account_id (UUID, FK → accounts)
├── transaction_type (VARCHAR)
├── amount (DECIMAL)
├── status (VARCHAR)
├── external_id (VARCHAR)
├── description (VARCHAR)
├── qr_code_data (TEXT)
├── recipient_key (VARCHAR)
├── created_at (TIMESTAMP)
├── updated_at (TIMESTAMP)
└── completed_at (TIMESTAMP)

webhook_logs
├── id (UUID, PK)
├── transaction_id (UUID, FK → transactions)
├── event_type (VARCHAR)
├── payload (TEXT)
├── status (VARCHAR)
├── error_message (TEXT)
├── received_at (TIMESTAMP)
└── processed_at (TIMESTAMP)
```

## Fluxo de Autenticação

```
1. Cliente registra-se
   ↓
2. Senha é hasheada com BCrypt
   ↓
3. Usuário salvo no banco
   ↓
4. Cliente faz login
   ↓
5. Senha verificada com BCrypt
   ↓
6. JWT Token gerado
   ↓
7. Token retornado ao cliente
   ↓
8. Cliente usa token em requisições autenticadas
```

## Fluxo de Criação de Usuário (Backoffice)

```
1. Admin acessa Backoffice
   ↓
2. Admin cria novo usuário
   ↓
3. Sistema gera senha temporária aleatória
   ↓
4. Senha é hasheada com BCrypt
   ↓
5. Usuário salvo no banco
   ↓
6. Email enviado via SMTP com credenciais
   ↓
7. Cliente recebe email
   ↓
8. Cliente faz login com senha temporária
   ↓
9. Cliente pode alterar senha
```

## Configurações de Segurança

### CORS
```csharp
policy.WithOrigins("http://localhost:3000", "http://localhost:3001", "http://localhost:5173")
      .AllowAnyMethod()
      .AllowAnyHeader()
      .AllowCredentials()
      .WithExposedHeaders("Content-Type", "Authorization");
```

### JWT
- **Issuer:** fintech-banking-cliente / fintech-banking-interna
- **Audience:** fintech-banking-cliente-api / fintech-banking-interna-api
- **Expiração:** 60-120 minutos
- **Algoritmo:** HS256

### Password Hashing
- **Algoritmo:** BCrypt
- **Cost Factor:** 11
- **Encoding:** UTF-8

## Mapeamento de Dados (Dapper)

```csharp
// Aliases SQL para mapear snake_case para PascalCase
SELECT 
    password_hash as PasswordHash,
    full_name as FullName,
    phone_number as PhoneNumber,
    is_active as IsActive,
    created_at as CreatedAt,
    updated_at as UpdatedAt,
    webhook_url as WebhookUrl
FROM users
```

## Variáveis de Ambiente

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=fintech_banking;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "SecretKey": "sua-chave-secreta",
    "Issuer": "fintech-banking",
    "Audience": "fintech-banking-api",
    "ExpirationMinutes": 60
  },
  "Email": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUsername": "seu-email@gmail.com",
    "SmtpPassword": "sua-senha-app",
    "FromEmail": "seu-email@gmail.com",
    "FromName": "FinTech Banking"
  }
}
```

## Performance

- **Índices:** 5 índices otimizados
- **Connection Pooling:** Habilitado
- **Async/Await:** Implementado em todas as operações
- **Dapper:** ORM leve e rápido
- **Caching:** Pronto para implementação

## Segurança

- ✅ JWT Authentication
- ✅ BCrypt Password Hashing
- ✅ CORS Protection
- ✅ Authorization Attributes
- ✅ SQL Injection Prevention (Dapper)
- ✅ HTTPS Ready (desabilitado em dev)

## Próximas Melhorias

1. **Caching:** Redis para cache de sessões
2. **Rate Limiting:** Proteção contra brute force
3. **Logging:** ELK Stack para logs centralizados
4. **Monitoring:** Application Insights
5. **Testing:** xUnit + Moq
6. **CI/CD:** GitHub Actions
7. **Containerization:** Kubernetes
8. **API Versioning:** v1, v2, etc

---

**Arquitetura moderna, escalável e segura! ✅**

