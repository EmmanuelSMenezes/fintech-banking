# FinTech Banking - Sistema Completo

Sistema de gateway de pagamentos fintech com integração bancária (Sicoob), PIX QR Code e saques.

## 📁 Estrutura do Projeto

```
FinTech-banking/
├── Backend/                          # Aplicações .NET 9
│   ├── src/
│   │   ├── FinTechBanking.API/              # API Principal (Port 5000)
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
├── FrontEnd/                         # Aplicações React/Next.js
│   ├── fintech-frontend/             # Frontend Cliente (Port 3000)
│   ├── fintech-internet-banking/     # Internet Banking (Port 3002)
│   └── fintech-backoffice/           # Backoffice Admin (Port 3003)
│
├── Collections_Postman/              # Postman Collections
│   ├── POSTMAN_API_CLIENTE_UPDATED.json
│   └── POSTMAN_API_INTERNA_UPDATED.json
│
└── Docs/                             # Documentação
    └── ... (documentos)
```

## 🚀 Quick Start

### Backend (.NET 9)

#### Build
```bash
cd Backend
dotnet build FinTechBanking.sln --configuration Release
```

#### Executar APIs Localmente
```bash
# Terminal 1 - API Principal
cd Backend/src/FinTechBanking.API
dotnet run

# Terminal 2 - API Cliente
cd Backend/src/FinTechBanking.API.Cliente
dotnet run

# Terminal 3 - API Interna
cd Backend/src/FinTechBanking.API.Interna
dotnet run
```

### Frontend

#### Cliente
```bash
cd FrontEnd/fintech-frontend
npm install
npm run dev
```

#### Internet Banking
```bash
cd FrontEnd/fintech-internet-banking
npm install
npm run dev
```

#### Backoffice
```bash
cd FrontEnd/fintech-backoffice
npm install
npm run dev
```

## 🔌 Endpoints Principais

### API.Cliente (5167)
- `POST /api/auth/register` - Registrar usuário
- `POST /api/auth/login` - Login
- `POST /api/transactions/pix-qrcode` - Gerar QR Code PIX
- `POST /api/transactions/withdrawal` - Solicitar saque
- `GET /api/transactions/{id}` - Status da transação

### API.Interna (5036)
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

## ✅ Status

- ✅ Backend: Compilado e rodando
- ✅ APIs: Funcionando com dados reais
- ✅ Banco de Dados: Integrado
- ✅ Message Broker: Configurado
- ✅ Banking Hub: Integrado com Sicoob
- ✅ Frontend: Pronto para desenvolvimento

---

**Desenvolvido com ❤️ para FinTech Banking**

