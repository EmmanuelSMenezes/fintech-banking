# 🚀 GUIA PASSO A PASSO - Testar Fluxo Real

## ✅ Pré-requisitos

- Docker rodando
- PostgreSQL 15 rodando em container
- Git Bash ou PowerShell
- Postman ou curl

---

## 📋 PASSO 1: Compilar a API

```bash
cd c:\Users\Emmanuel1\Documents\Fintech-banking

# Compilar API Cliente
dotnet build src/FinTechBanking.API.Cliente/FinTechBanking.API.Cliente.csproj

# Verificar se compilou sem erros
# Deve aparecer: "Build succeeded"
```

---

## 📋 PASSO 2: Popular o Banco de Dados

### Opção A: Via Docker (Recomendado)

```bash
# Conectar ao PostgreSQL
docker exec -it fintech_postgres psql -U postgres -d fintech_banking

# Dentro do psql, executar:
\i SEED_DATABASE.sql

# Verificar dados inseridos:
SELECT * FROM users;
SELECT * FROM accounts;
SELECT * FROM transactions;

# Sair
\q
```

### Opção B: Via DBeaver

1. Abrir DBeaver
2. Conectar ao PostgreSQL (localhost:5432)
3. Abrir nova Query
4. Copiar conteúdo de SEED_DATABASE.sql
5. Executar (Ctrl+Enter)

---

## 📋 PASSO 3: Iniciar Docker Compose

```bash
cd c:\Users\Emmanuel1\Documents\Fintech-banking

# Subir todos os serviços
docker-compose up -d

# Verificar se está rodando
docker-compose ps

# Deve aparecer:
# fintech_postgres    - Up
# fintech_rabbitmq    - Up
# fintech_api_cliente - Up
# fintech_api_interna - Up
```

---

## 📋 PASSO 4: Testar Endpoints

### 4.1 - Login (Obter Token)

```bash
curl -X POST http://localhost:5167/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "cliente@fintech.com",
    "password": "Test@123"
  }'
```

**Resposta esperada:**
```json
{
  "message": "Login realizado com sucesso",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
      "id": "550e8400-e29b-41d4-a716-446655440001",
      "email": "cliente@fintech.com",
      "fullName": "João Silva"
    }
  }
}
```

**⚠️ IMPORTANTE:** Copie o token! Você vai usar em todos os próximos requests.

```bash
# Salvar token em variável (PowerShell)
$TOKEN = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."

# Ou (Bash)
TOKEN="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

---

### 4.2 - Consultar Saldo (REAL)

```bash
curl -X GET http://localhost:5167/api/transactions/balance \
  -H "Authorization: Bearer $TOKEN"
```

**Resposta esperada:**
```json
{
  "message": "Saldo obtido com sucesso",
  "data": {
    "accountId": "660e8400-e29b-41d4-a716-446655440001",
    "accountNumber": "0001234567",
    "balance": 5000.00,
    "currency": "BRL",
    "bankCode": "001",
    "lastUpdated": "2025-10-21T10:30:00Z"
  }
}
```

✅ **Saldo real do banco!**

---

### 4.3 - Gerar PIX QR Code (REAL)

```bash
curl -X POST http://localhost:5167/api/transactions/pix/qrcode \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "amount": 150.00,
    "description": "Pagamento de teste"
  }'
```

**Resposta esperada:**
```json
{
  "message": "QR Code gerado com sucesso",
  "data": {
    "transactionId": "770e8400-e29b-41d4-a716-446655440004",
    "amount": 150.00,
    "description": "Pagamento de teste",
    "qrCodeData": "00020126580014br.gov.bcb.pix0136...",
    "pixKey": "550e8400-e29b-41d4-a716-446655440004",
    "expiresAt": "2025-10-21T11:00:00Z",
    "status": "PENDING",
    "createdAt": "2025-10-21T10:30:00Z"
  }
}
```

✅ **Transação salva no banco!**

---

### 4.4 - Solicitar Saque (REAL - Valida Saldo)

```bash
curl -X POST http://localhost:5167/api/transactions/withdrawal \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "amount": 1000.00,
    "bankAccount": "0001234567"
  }'
```

**Resposta esperada:**
```json
{
  "message": "Saque solicitado com sucesso",
  "data": {
    "transactionId": "770e8400-e29b-41d4-a716-446655440005",
    "amount": 1000.00,
    "bankAccount": "0001234567",
    "status": "PENDING",
    "newBalance": 4000.00,
    "createdAt": "2025-10-21T10:30:00Z"
  }
}
```

✅ **Saldo deduzido! Novo saldo: 4000.00**

---

### 4.5 - Consultar Histórico (REAL)

```bash
curl -X GET "http://localhost:5167/api/transactions/history?page=1&pageSize=10" \
  -H "Authorization: Bearer $TOKEN"
```

**Resposta esperada:**
```json
{
  "message": "Histórico obtido com sucesso",
  "data": {
    "transactions": [
      {
        "id": "770e8400-e29b-41d4-a716-446655440005",
        "type": "WITHDRAWAL",
        "amount": 1000.00,
        "status": "PENDING",
        "description": "Saque para conta 0001234567",
        "date": "2025-10-21T10:30:00Z"
      },
      {
        "id": "770e8400-e29b-41d4-a716-446655440004",
        "type": "PIX_QR_CODE",
        "amount": 150.00,
        "status": "PENDING",
        "description": "Pagamento de teste",
        "date": "2025-10-21T10:30:00Z"
      }
    ],
    "page": 1,
    "pageSize": 10,
    "total": 2
  }
}
```

✅ **Histórico real do banco!**

---

### 4.6 - Verificar Status de Transação

```bash
# Use o transactionId da resposta anterior
curl -X GET "http://localhost:5167/api/transactions/770e8400-e29b-41d4-a716-446655440005/status" \
  -H "Authorization: Bearer $TOKEN"
```

**Resposta esperada:**
```json
{
  "message": "Status obtido com sucesso",
  "data": {
    "transactionId": "770e8400-e29b-41d4-a716-446655440005",
    "status": "PENDING",
    "amount": 1000.00,
    "type": "WITHDRAWAL",
    "description": "Saque para conta 0001234567",
    "createdAt": "2025-10-21T10:30:00Z"
  }
}
```

✅ **Status real do banco!**

---

## 📋 PASSO 5: Verificar Dados no Banco

```bash
# Conectar ao PostgreSQL
docker exec -it fintech_postgres psql -U postgres -d fintech_banking

# Verificar saldo atualizado
SELECT account_number, balance FROM accounts;

# Verificar transações criadas
SELECT id, transaction_type, amount, status, created_at FROM transactions ORDER BY created_at DESC;

# Sair
\q
```

---

## ✅ Checklist de Testes

- [ ] Login retorna token válido
- [ ] Saldo consultado do banco (5000.00)
- [ ] PIX QR Code salvo no banco
- [ ] Saque deduz saldo (novo saldo: 4000.00)
- [ ] Histórico retorna 2 transações
- [ ] Status retorna dados corretos
- [ ] Dados persistem no banco

---

## 🎉 Resultado

**Todos os endpoints funcionam com dados REAIS!**

Não há mais simulações. Tudo é persistido no PostgreSQL.

**Fluxo 100% funcional! 🚀**

