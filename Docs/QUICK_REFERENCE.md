# ⚡ Quick Reference - Comandos Úteis

## 🚀 Iniciar Projeto

```bash
# 1. Iniciar serviços (PostgreSQL + RabbitMQ)
docker-compose up -d

# 2. Compilar
dotnet build

# 3. Executar API
cd src/FinTechBanking.API
dotnet run

# 4. API disponível em
https://localhost:5001
```

---

## 🔐 Autenticação

### Registrar Usuário
```bash
curl -X POST https://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "Password123!",
    "fullName": "John Doe",
    "document": "12345678901",
    "phoneNumber": "11999999999"
  }'
```

### Fazer Login
```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "Password123!"
  }'
```

**Salvar o token retornado:**
```bash
TOKEN="seu-token-aqui"
```

---

## 💰 Transações

### Gerar QR Code PIX
```bash
curl -X POST https://localhost:5001/api/transactions/pix-qrcode \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "amount": 100.00,
    "recipientKey": "user@example.com",
    "description": "Pagamento de teste"
  }'
```

### Solicitar Saque
```bash
curl -X POST https://localhost:5001/api/transactions/withdrawal \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "amount": 50.00,
    "accountNumber": "123456789",
    "bankCode": "001"
  }'
```

### Obter Status da Transação
```bash
curl -X GET https://localhost:5001/api/transactions/{transactionId} \
  -H "Authorization: Bearer $TOKEN"
```

### Obter Saldo
```bash
curl -X GET https://localhost:5001/api/accounts/balance \
  -H "Authorization: Bearer $TOKEN"
```

---

## 🗄️ Banco de Dados

### Conectar ao PostgreSQL
```bash
psql -U postgres -h localhost -d fintech_banking
```

### Queries Úteis
```sql
-- Ver usuários
SELECT * FROM users;

-- Ver contas
SELECT * FROM accounts;

-- Ver transações
SELECT * FROM transactions;

-- Ver logs de webhooks
SELECT * FROM webhook_logs;
```

---

## 🐰 RabbitMQ

### Acessar Management UI
```
http://localhost:15672
Usuário: guest
Senha: guest
```

### Filas Esperadas
- `pix-requests` - Requisições de QR Code
- `withdrawal-requests` - Requisições de saque
- `webhook-events` - Eventos de webhooks

---

## 🛠️ Desenvolvimento

### Compilar
```bash
dotnet build
```

### Limpar Build
```bash
dotnet clean
```

### Restaurar Dependências
```bash
dotnet restore
```

### Executar Testes
```bash
dotnet test
```

### Adicionar Pacote NuGet
```bash
dotnet add package NomeDoPacote
```

### Remover Pacote NuGet
```bash
dotnet remove package NomeDoPacote
```

---

## 📁 Estrutura de Pastas

```
src/
├── FinTechBanking.API/          # API REST
│   ├── Controllers/
│   ├── Program.cs
│   └── appsettings.json
├── FinTechBanking.Core/         # Lógica de negócio
│   ├── Entities/
│   ├── DTOs/
│   └── Interfaces/
├── FinTechBanking.Data/         # Acesso a dados
│   ├── Repositories/
│   └── Migrations/
├── FinTechBanking.Services/     # Serviços
│   ├── Auth/
│   └── Messaging/
└── FinTechBanking.Banking/      # Integrações bancárias
    ├── Hub/
    └── Services/
```

---

## 📝 Arquivos Importantes

| Arquivo | Descrição |
|---------|-----------|
| `README.md` | Visão geral do projeto |
| `SETUP.md` | Guia de setup |
| `ARCHITECTURE.md` | Arquitetura detalhada |
| `DEVELOPMENT.md` | Guia de desenvolvimento |
| `API_EXAMPLES.md` | Exemplos de uso da API |
| `NEXT_STEPS.md` | Próximos passos |
| `PROJECT_STATUS.md` | Status do projeto |
| `docker-compose.yml` | Configuração Docker |
| `appsettings.json` | Configurações da API |

---

## 🔍 Troubleshooting

### Erro: "Connection refused"
```bash
# Verificar se containers estão rodando
docker-compose ps

# Reiniciar containers
docker-compose restart
```

### Erro: "Database does not exist"
```bash
# Executar migrations
psql -U postgres -h localhost -d fintech_banking -f src/FinTechBanking.Data/Migrations/001_InitialSchema.sql
```

### Erro: "Port already in use"
```bash
# Mudar portas no docker-compose.yml
# Ou matar processo na porta
lsof -i :5432
kill -9 <PID>
```

### Erro de Compilação
```bash
# Limpar e restaurar
dotnet clean
dotnet restore
dotnet build
```

---

## 📊 Endpoints Rápidos

| Método | Endpoint | Autenticação |
|--------|----------|--------------|
| POST | `/api/auth/register` | ❌ Não |
| POST | `/api/auth/login` | ❌ Não |
| GET | `/api/accounts/balance` | ✅ Sim |
| POST | `/api/transactions/pix-qrcode` | ✅ Sim |
| POST | `/api/transactions/withdrawal` | ✅ Sim |
| GET | `/api/transactions/{id}` | ✅ Sim |
| POST | `/api/webhooks/sicoob` | ❌ Não |

---

## 🎯 Próximos Comandos

### Criar Projeto Workers
```bash
dotnet new classlib -n FinTechBanking.Workers -o src/FinTechBanking.Workers
cd src/FinTechBanking.Workers
dotnet add reference ../FinTechBanking.Core
dotnet add reference ../FinTechBanking.Data
dotnet add reference ../FinTechBanking.Services
dotnet add reference ../FinTechBanking.Banking
cd ../..
dotnet sln add src/FinTechBanking.Workers/FinTechBanking.Workers.csproj
```

### Criar Projeto de Testes
```bash
dotnet new mstest -n FinTechBanking.Tests -o tests/FinTechBanking.Tests
cd tests/FinTechBanking.Tests
dotnet add reference ../../src/FinTechBanking.Core
dotnet add reference ../../src/FinTechBanking.Data
dotnet add reference ../../src/FinTechBanking.Services
```

### Inicializar Frontend React
```bash
npm create vite@latest fintech-frontend -- --template react
cd fintech-frontend
npm install
npm run dev
```

---

## 💡 Dicas

1. **Sempre use `async/await`** para operações I/O
2. **Valide entrada** em todos os endpoints
3. **Trate exceções** apropriadamente
4. **Use DTOs** para comunicação externa
5. **Mantenha secrets** em `appsettings.json` (não commitar)
6. **Teste localmente** antes de fazer push
7. **Documente** mudanças significativas

---

## 📞 Referências Rápidas

- [.NET Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [Dapper GitHub](https://github.com/DapperLib/Dapper)
- [PostgreSQL Docs](https://www.postgresql.org/docs/)
- [RabbitMQ Tutorials](https://www.rabbitmq.com/getstarted.html)
- [JWT.io](https://jwt.io/)

---

**Última atualização:** 2025-10-21
**Versão:** 1.0

