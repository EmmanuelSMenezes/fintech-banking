# 🔗 Portas e Conectividade - FinTech Banking

## 📊 Mapeamento de Portas

### Backend APIs

| Serviço | Porta | URL | Swagger |
|---------|-------|-----|---------|
| **FinTechBanking.API** (Principal) | 5064 | http://localhost:5064 | http://localhost:5064/swagger |
| **FinTechBanking.API.Cliente** | 5167 | http://localhost:5167 | http://localhost:5167/swagger |
| **FinTechBanking.API.Interna** | 5036 | http://localhost:5036 | http://localhost:5036/swagger |

### Frontend

| Aplicação | Porta | URL |
|-----------|-------|-----|
| **fintech-frontend** (Vite + React) | 5173 | http://localhost:5173 |

### Infraestrutura

| Serviço | Porta | URL |
|---------|-------|-----|
| **PostgreSQL** | 5432 | localhost:5432 |
| **RabbitMQ** | 5672 | localhost:5672 |
| **RabbitMQ Management** | 15672 | http://localhost:15672 |

## 🔄 Fluxo de Comunicação

```
Frontend (5173)
    ↓
    └─→ API Principal (5064)
            ├─→ PostgreSQL (5432)
            ├─→ RabbitMQ (5672)
            └─→ Sicoob Banking Hub
    
    └─→ API Cliente (5167)
            ├─→ PostgreSQL (5432)
            ├─→ RabbitMQ (5672)
            └─→ Sicoob Banking Hub
    
    └─→ API Interna (5036)
            ├─→ PostgreSQL (5432)
            ├─→ RabbitMQ (5672)
            └─→ Sicoob Banking Hub
```

## 🔐 CORS Configuration

Todas as APIs têm CORS habilitado para aceitar requisições de qualquer origem:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
```

## 📱 Frontend Configuration

### API URL

**Arquivo:** `FrontEnd/fintech-frontend/src/services/api.js`

```javascript
const API_URL = 'http://localhost:5064/api';
```

### Endpoints Utilizados

- `POST /api/auth/login` - Login
- `POST /api/auth/register` - Registro
- `GET /api/accounts/balance` - Saldo da conta
- `GET /api/accounts/details` - Detalhes da conta
- `POST /api/transactions/pix-qrcode` - Gerar QR Code PIX
- `POST /api/transactions/withdrawal` - Solicitar saque
- `GET /api/transactions/{id}` - Status da transação
- `GET /api/transactions/history` - Histórico de transações

## 🚀 Iniciando os Serviços

### 1. Infraestrutura (Docker)

```bash
cd Backend
docker-compose up -d
```

### 2. APIs Backend

**Terminal 1 - API Principal:**
```bash
cd Backend/src/FinTechBanking.API
dotnet run
```

**Terminal 2 - API Cliente:**
```bash
cd Backend/src/FinTechBanking.API.Cliente
dotnet run
```

**Terminal 3 - API Interna:**
```bash
cd Backend/src/FinTechBanking.API.Interna
dotnet run
```

### 3. Frontend

```bash
cd FrontEnd/fintech-frontend
npm install
npm run dev
```

## 🧪 Testando Conectividade

### Verificar se API está respondendo

```bash
# API Principal
curl http://localhost:5064/swagger

# API Cliente
curl http://localhost:5167/swagger

# API Interna
curl http://localhost:5036/swagger
```

### Testar Login

```bash
curl -X POST http://localhost:5064/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "password123"
  }'
```

### Testar com Token

```bash
curl -X GET http://localhost:5064/api/accounts/balance \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

## 📋 Checklist de Verificação

- [ ] PostgreSQL rodando na porta 5432
- [ ] RabbitMQ rodando na porta 5672
- [ ] API Principal respondendo em http://localhost:5064
- [ ] API Cliente respondendo em http://localhost:5167
- [ ] API Interna respondendo em http://localhost:5036
- [ ] Swagger acessível em todas as APIs
- [ ] Frontend conectando em http://localhost:5064/api
- [ ] CORS funcionando (requisições do frontend passam)
- [ ] JWT tokens sendo gerados corretamente
- [ ] Banco de dados com dados de teste

## 🔧 Troubleshooting

### Porta já em uso

```bash
# Encontrar processo usando a porta
netstat -ano | findstr :5064

# Matar processo (Windows)
taskkill /PID <PID> /F
```

### CORS Error

- Verificar se CORS está habilitado em Program.cs
- Confirmar que `app.UseCors("AllowAll")` está antes de `app.UseAuthentication()`

### Frontend não conecta ao Backend

- Verificar se API está rodando: `curl http://localhost:5064`
- Verificar URL em `api.js`: deve ser `http://localhost:5064/api`
- Verificar console do navegador para erros

### Token inválido

- Fazer login novamente
- Copiar token completo (sem espaços)
- Usar formato: `Bearer <token>`

---

**Última atualização:** 2025-10-21

