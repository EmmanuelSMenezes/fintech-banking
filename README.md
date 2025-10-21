# FinTech Banking - Sistema Completo

Sistema de gateway de pagamentos fintech com integração bancária (Sicoob), PIX QR Code e saques.

## 📁 Estrutura do Projeto

```
FinTech-banking/
├── Backend/                          # Aplicações .NET 9
│   ├── src/
│   │   ├── FinTechBanking.API/              # API Principal (Port 5064)
│   │   ├── FinTechBanking.API.Cliente/      # API Cliente (Port 5167)
│   │   ├── FinTechBanking.API.Interna/      # API Interna (Port 5036)
│   │   ├── FinTechBanking.Core/             # Entities, DTOs, Interfaces
│   │   ├── FinTechBanking.Data/             # Repositories, Dapper
│   │   ├── FinTechBanking.Services/         # Business Logic
│   │   ├── FinTechBanking.Banking/          # Banking Hub, Sicoob
│   │   ├── FinTechBanking.Workers/          # Background Jobs
│   │   └── FinTechBanking.ConsumerWorker/   # RabbitMQ Consumers
│   ├── FinTechBanking.sln
│   └── docker-compose.yml
│
├── FrontEnd/                         # Aplicações React/Vite
│   ├── fintech-frontend/             # Frontend Cliente (Port 5173)
│   └── fintech-internet-banking/     # Internet Banking (futuro)
│
├── Collections_Postman/              # Postman Collections
│
├── Docs/                             # Documentação Completa
│   ├── QUICK_START.md
│   ├── FRONTEND_API_INTEGRATION.md
│   ├── PORTS_AND_CONNECTIVITY.md
│   ├── TEST_CREDENTIALS.md
│   ├── SWAGGER_SETUP.md
│   └── CSS_IMPROVEMENTS.md
│
├── AGENT_CONTEXT.md                  # Contexto para próximo agente
└── .gitignore
```

## 🚀 Quick Start

### 1. Infraestrutura (Docker)
```bash
cd Backend
docker-compose up -d
```

### 2. Backend (.NET 9)

#### Build
```bash
cd Backend
dotnet build FinTechBanking.sln --configuration Release
```

#### Executar APIs (3 Terminais)
```bash
# Terminal 1 - API Principal (5064)
cd Backend/src/FinTechBanking.API
dotnet run

# Terminal 2 - API Cliente (5167)
cd Backend/src/FinTechBanking.API.Cliente
dotnet run

# Terminal 3 - API Interna (5036)
cd Backend/src/FinTechBanking.API.Interna
dotnet run
```

### 3. Frontend (React + Vite)
```bash
cd FrontEnd/fintech-frontend
npm install
npm run dev
# Acesse: http://localhost:5173
```

### 4. Acessar Aplicações

| Aplicação | URL | Descrição |
|-----------|-----|-----------|
| Frontend | http://localhost:5173 | React App |
| API Principal | http://localhost:5064 | Backend Principal |
| API Cliente | http://localhost:5167 | API Pública |
| API Interna | http://localhost:5036 | API Privada |
| Swagger Principal | http://localhost:5064/swagger | Documentação |
| Swagger Cliente | http://localhost:5167/swagger | Documentação |
| Swagger Interna | http://localhost:5036/swagger | Documentação |

## 🔌 Endpoints Principais

### Auth
- `POST /api/auth/register` - Registrar usuário
- `POST /api/auth/login` - Fazer login

### Accounts
- `GET /api/accounts/balance` - Obter saldo
- `GET /api/accounts/details` - Obter detalhes da conta

### Transactions
- `POST /api/transactions/pix-qrcode` - Gerar QR Code PIX
- `POST /api/transactions/withdrawal` - Solicitar saque
- `GET /api/transactions/{id}` - Status da transação
- `GET /api/transactions/history` - Histórico de transações

### Admin (API Interna)
- `POST /api/admin/users` - Criar usuário
- `GET /api/admin/users` - Listar usuários
- `GET /api/admin/transactions` - Listar transações
- `GET /api/admin/dashboard` - Dashboard

## 🗄️ Banco de Dados

PostgreSQL 15 com Dapper ORM

## 📨 Message Broker

RabbitMQ 3 para processamento assíncrono

## 🏦 Integração Bancária

Sicoob Bank Service para operações PIX

## 🔐 Autenticação

JWT com issuers separados para cada API

## 📊 Tecnologias

### Backend
- .NET 9
- Dapper ORM
- PostgreSQL 15
- RabbitMQ 3
- JWT Authentication

### Frontend
- React 18 / Next.js 14
- TypeScript
- Tailwind CSS

## 🔐 Credenciais de Teste

```
Email: emmanuel@fintech.com
Senha: Senha123!
```

## 📚 Documentação

- **AGENT_CONTEXT.md** - Contexto completo para próximo agente
- **Docs/QUICK_START.md** - Guia rápido de inicialização
- **Docs/FRONTEND_API_INTEGRATION.md** - Integração Frontend-Backend
- **Docs/PORTS_AND_CONNECTIVITY.md** - Mapeamento de portas
- **Docs/TEST_CREDENTIALS.md** - Credenciais e testes
- **Docs/SWAGGER_SETUP.md** - Configuração Swagger
- **Docs/CSS_IMPROVEMENTS.md** - Melhorias CSS

## ✅ Status

- ✅ Backend: 3 APIs compiladas e rodando
- ✅ APIs: Funcionando com dados reais
- ✅ Banco de Dados: PostgreSQL integrado
- ✅ Message Broker: RabbitMQ configurado
- ✅ Banking Hub: Integrado com Sicoob
- ✅ Frontend: React + Vite funcionando
- ✅ Autenticação: JWT implementado
- ✅ Swagger: Documentação automática
- ✅ CORS: Habilitado em todas as APIs
- ✅ Repositório: Sincronizado no GitHub

## 🎯 Próximos Passos

1. Implementar refresh token
2. Adicionar expiração de token
3. Implementar 2FA
4. Adicionar mais endpoints
5. Melhorar tratamento de erros
6. Adicionar testes unitários

---

**Desenvolvido com ❤️ para FinTech Banking**

**Última atualização:** 2025-10-21

