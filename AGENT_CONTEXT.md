# 🤖 Agent Context - FinTech Banking

## 📋 Resumo do Projeto

**FinTech Banking** é um sistema completo de gateway bancário com:
- **Backend:** 3 APIs .NET 9 (Principal, Cliente, Interna)
- **Frontend:** React 18 + Vite (fintech-frontend)
- **Banco de Dados:** PostgreSQL 15
- **Message Broker:** RabbitMQ 3
- **Integração:** Sicoob Banking Hub

## 🏗️ Arquitetura

```
Frontend (5173)
    ↓
API Principal (5064)
    ├─ PostgreSQL (5432)
    ├─ RabbitMQ (5672)
    └─ Sicoob Banking Hub
    
API Cliente (5167)
    ├─ PostgreSQL (5432)
    ├─ RabbitMQ (5672)
    └─ Sicoob Banking Hub
    
API Interna (5036)
    ├─ PostgreSQL (5432)
    ├─ RabbitMQ (5672)
    └─ Sicoob Banking Hub
```

## 🔑 Informações Críticas

### Portas
- Frontend: 5173
- API Principal: 5064
- API Cliente: 5167
- API Interna: 5036
- PostgreSQL: 5432
- RabbitMQ: 5672 (Management: 15672)

### Credenciais de Teste
```
Email: emmanuel@fintech.com
Senha: Senha123!
```

### URLs Importantes
- Frontend: http://localhost:5173
- Swagger Principal: http://localhost:5064/swagger
- Swagger Cliente: http://localhost:5167/swagger
- Swagger Interna: http://localhost:5036/swagger

## 🔄 Integração Frontend-Backend

### Resposta da API (Login)
```json
{
  "accessToken": "eyJhbGc...",
  "refreshToken": "T2z1J8...",
  "expiresIn": "2025-10-21T22:01:06..."
}
```

### Mapeamento no Frontend
```javascript
// api.js mapeia para:
{
  success: true,
  data: {
    token: data.accessToken,
    refreshToken: data.refreshToken,
    expiresIn: data.expiresIn,
    user: { email: email }
  }
}
```

### Armazenamento
```javascript
localStorage.setItem('token', response.data.token);
localStorage.setItem('refreshToken', response.data.refreshToken);
localStorage.setItem('user', JSON.stringify(response.data.user));
localStorage.setItem('expiresIn', response.data.expiresIn);
```

## 📁 Estrutura do Projeto

```
Backend/
├── src/
│   ├── FinTechBanking.API/              # API Principal (5064)
│   ├── FinTechBanking.API.Cliente/      # API Cliente (5167)
│   ├── FinTechBanking.API.Interna/      # API Interna (5036)
│   ├── FinTechBanking.Core/             # DTOs, Entities
│   ├── FinTechBanking.Data/             # Repositories
│   ├── FinTechBanking.Services/         # Business Logic
│   ├── FinTechBanking.Banking/          # Sicoob Integration
│   ├── FinTechBanking.Workers/          # Background Jobs
│   └── FinTechBanking.ConsumerWorker/   # RabbitMQ Consumer
├── docker-compose.yml
└── FinTechBanking.sln

FrontEnd/
├── fintech-frontend/                    # React + Vite (5173)
│   ├── src/
│   │   ├── components/
│   │   │   ├── Auth/
│   │   │   │   ├── Login.jsx
│   │   │   │   ├── Register.jsx
│   │   │   │   └── Auth.css
│   │   │   └── Dashboard/
│   │   │       ├── Dashboard.jsx
│   │   │       └── Dashboard.css
│   │   ├── services/
│   │   │   └── api.js                   # API Client
│   │   ├── App.jsx
│   │   ├── App.css
│   │   └── index.css
│   └── package.json

Collections_Postman/                     # Postman Collections

Docs/
├── QUICK_START.md                       # Guia rápido
├── FRONTEND_API_INTEGRATION.md          # Integração
├── PORTS_AND_CONNECTIVITY.md            # Portas
├── TEST_CREDENTIALS.md                  # Credenciais
├── SWAGGER_SETUP.md                     # Swagger
└── CSS_IMPROVEMENTS.md                  # CSS
```

## 🚀 Iniciar Projeto

### 1. Infraestrutura
```bash
cd Backend
docker-compose up -d
```

### 2. APIs (3 Terminais)
```bash
# Terminal 1
cd Backend/src/FinTechBanking.API
dotnet run

# Terminal 2
cd Backend/src/FinTechBanking.API.Cliente
dotnet run

# Terminal 3
cd Backend/src/FinTechBanking.API.Interna
dotnet run
```

### 3. Frontend
```bash
cd FrontEnd/fintech-frontend
npm run dev
```

## 🔐 Autenticação

### Login
```bash
POST http://localhost:5064/api/auth/login
{
  "email": "user@example.com",
  "password": "password123"
}
```

### Usar Token
```bash
Authorization: Bearer <token>
```

## 📊 Endpoints Principais

### Auth
- `POST /api/auth/register` - Registrar
- `POST /api/auth/login` - Login

### Accounts
- `GET /api/accounts/balance` - Saldo
- `GET /api/accounts/details` - Detalhes

### Transactions
- `POST /api/transactions/pix-qrcode` - QR Code PIX
- `POST /api/transactions/withdrawal` - Saque
- `GET /api/transactions/history` - Histórico

## 🛠️ Tecnologias

**Backend:**
- .NET 9
- Dapper ORM
- PostgreSQL 15
- RabbitMQ 3
- JWT Authentication
- Swagger/OpenAPI

**Frontend:**
- React 18/19
- Vite 7
- CSS3 (Variables, Animations)
- Fetch API

## 📝 Últimas Mudanças

### Correção Frontend-Backend (2025-10-21)
1. Mapeamento de resposta da API em `api.js`
2. Tratamento de erros padronizado
3. Armazenamento de tokens em localStorage
4. Logs detalhados para debug
5. Atualização de Login, Register e Dashboard

### CSS Melhorado
- Gradientes e animações
- Design responsivo
- Tipografia profissional
- Efeitos hover

### Swagger Instalado
- Swashbuckle.AspNetCore v9.0.6
- JWT Bearer support
- Documentação automática

## ⚠️ Problemas Conhecidos

Nenhum no momento. Sistema funcionando corretamente.

## 📚 Documentação

Veja `Docs/` para documentação completa:
- QUICK_START.md - Iniciar rápido
- FRONTEND_API_INTEGRATION.md - Integração
- PORTS_AND_CONNECTIVITY.md - Portas
- TEST_CREDENTIALS.md - Testes

## 🎯 Próximos Passos

1. Implementar refresh token
2. Adicionar expiração de token
3. Implementar 2FA
4. Adicionar mais endpoints
5. Melhorar tratamento de erros
6. Adicionar testes unitários

## 📞 Contato

Desenvolvedor: Emmanuel Menezes
Email: emmanuel.s.menezes@gmail.com

---

**Última atualização:** 2025-10-21
**Status:** ✅ Funcionando

