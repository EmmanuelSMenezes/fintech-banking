# 🚀 Getting Started - FinTech Banking

## ✅ Pré-requisitos

- ✅ .NET 9 SDK
- ✅ Node.js 18+
- ✅ Docker Desktop
- ✅ PostgreSQL 15 (via Docker)
- ✅ RabbitMQ 3 (via Docker)

---

## 🎯 Opção 1: Rodar Tudo Automaticamente (Recomendado)

### Windows PowerShell

```powershell
# Abra PowerShell como Administrador e execute:
.\RUN_ALL_SERVICES.ps1
```

Isso vai iniciar:
- ✅ Docker (PostgreSQL + RabbitMQ)
- ✅ API Cliente (Porta 5065)
- ✅ API Interna (Porta 5066)
- ✅ Consumer Worker
- ✅ Internet Banking (Porta 3000)
- ✅ Backoffice (Porta 3001)

---

## 🎯 Opção 2: Rodar Manualmente

### 1️⃣ Iniciar Docker

```bash
docker-compose up -d
```

Aguarde 10 segundos para os serviços ficarem prontos.

### 2️⃣ Compilar Projeto

```bash
dotnet build
```

### 3️⃣ Iniciar API Cliente (Terminal 1)

```bash
cd src/FinTechBanking.API.Cliente
$env:ASPNETCORE_URLS='http://localhost:5065'
dotnet run
```

### 4️⃣ Iniciar API Interna (Terminal 2)

```bash
cd src/FinTechBanking.API.Interna
$env:ASPNETCORE_URLS='http://localhost:5066'
dotnet run
```

### 5️⃣ Iniciar Consumer Worker (Terminal 3)

```bash
cd src/FinTechBanking.ConsumerWorker
dotnet run
```

### 6️⃣ Iniciar Internet Banking (Terminal 4)

```bash
cd fintech-internet-banking
npm install  # Primeira vez apenas
npm run dev
```

### 7️⃣ Iniciar Backoffice (Terminal 5)

```bash
cd fintech-backoffice
npm install  # Primeira vez apenas
npm run dev
```

---

## 📍 URLs de Acesso

| Serviço | URL | Descrição |
|---------|-----|-----------|
| **Internet Banking** | http://localhost:3000 | Frontend para clientes |
| **Backoffice** | http://localhost:3001 | Frontend para administradores |
| **API Cliente** | http://localhost:5065 | API pública (transações) |
| **API Interna** | http://localhost:5066 | API privada (administração) |
| **Swagger Cliente** | http://localhost:5065/swagger | Documentação API Cliente |
| **Swagger Interna** | http://localhost:5066/swagger | Documentação API Interna |
| **RabbitMQ** | http://localhost:15672 | Gerenciador de filas |
| **PostgreSQL** | localhost:5432 | Banco de dados |

---

## 🧪 Testando a API

### Com Postman

1. Importe as collections:
   - `Postman_API_Cliente.json`
   - `Postman_API_Interna.json`

2. Siga o fluxo recomendado nas collections

### Com cURL

```bash
# Registrar cliente
curl -X POST http://localhost:5065/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "cliente@example.com",
    "password": "Senha123!",
    "fullName": "João Silva",
    "document": "12345678900",
    "phoneNumber": "11999999999"
  }'

# Login
curl -X POST http://localhost:5065/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "cliente@example.com",
    "password": "Senha123!"
  }'

# Obter saldo (com token)
curl -X GET http://localhost:5065/api/transactions/balance \
  -H "Authorization: Bearer SEU_TOKEN_AQUI"
```

---

## 🔧 Configuração

### Variáveis de Ambiente

**API Cliente** (`src/FinTechBanking.API.Cliente/appsettings.json`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=fintech_banking;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "SecretKey": "your-super-secret-key-change-this-in-production-cliente",
    "Issuer": "fintech-banking-cliente",
    "Audience": "fintech-banking-cliente-api",
    "ExpirationMinutes": 60
  }
}
```

**API Interna** (`src/FinTechBanking.API.Interna/appsettings.json`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=fintech_banking;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "SecretKey": "your-super-secret-key-change-this-in-production-interna",
    "Issuer": "fintech-banking-interna",
    "Audience": "fintech-banking-interna-api",
    "ExpirationMinutes": 120
  }
}
```

---

## 🐛 Troubleshooting

### Porta já em uso

```bash
# Encontrar processo usando a porta
netstat -ano | findstr :5065

# Matar processo
taskkill /PID <PID> /F
```

### Docker não inicia

```bash
# Iniciar Docker Desktop manualmente
# Ou executar:
docker-compose down
docker-compose up -d
```

### Erro de conexão com banco

```bash
# Verificar se PostgreSQL está rodando
docker ps | findstr postgres

# Verificar logs
docker logs fintech_postgres
```

---

## 📚 Documentação

- **ARCHITECTURE_UPDATED.md** - Arquitetura completa do projeto
- **POSTMAN_GUIDE.md** - Guia das collections Postman
- **CURL_EXAMPLES.md** - Exemplos com curl
- **TEST_SICOOB_SANDBOX.md** - Testes com Sicoob

---

## ✨ Próximos Passos

1. ✅ Rodar o projeto
2. ✅ Testar endpoints com Postman
3. ✅ Explorar Internet Banking
4. ✅ Explorar Backoffice
5. ✅ Implementar testes E2E
6. ✅ Deploy em produção

---

**Pronto para começar? Execute o script e divirta-se! 🎉**

