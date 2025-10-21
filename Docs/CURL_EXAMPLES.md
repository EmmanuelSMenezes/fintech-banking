# 🔧 Exemplos CURL - FinTech Banking API

## 📋 Índice

1. [Autenticação](#autenticação)
2. [Contas](#contas)
3. [Transações](#transações)
4. [Webhooks](#webhooks)

---

## 🔐 Autenticação

### Register - Registrar Novo Usuário

```bash
curl -X POST https://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "Pass123!",
    "fullName": "John Doe",
    "document": "12345678901",
    "phoneNumber": "11999999999"
  }'
```

**Resposta:**
```json
{
  "success": true,
  "message": "Usuário registrado com sucesso",
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "email": "user@example.com"
  }
}
```

### Login - Autenticar Usuário

```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "Pass123!"
  }'
```

**Resposta:**
```json
{
  "success": true,
  "message": "Login realizado com sucesso",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
      "id": "550e8400-e29b-41d4-a716-446655440000",
      "email": "user@example.com"
    }
  }
}
```

---

## 💰 Contas

### Get Balance - Obter Saldo

```bash
curl -X GET https://localhost:5001/api/accounts/balance \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

**Resposta:**
```json
{
  "success": true,
  "data": {
    "balance": 1000.00
  }
}
```

### Get Account Details - Obter Detalhes da Conta

```bash
curl -X GET https://localhost:5001/api/accounts/details \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

**Resposta:**
```json
{
  "success": true,
  "data": {
    "accountId": "550e8400-e29b-41d4-a716-446655440000",
    "accountNumber": "123456",
    "balance": 1000.00,
    "bankCode": "001",
    "status": "ACTIVE"
  }
}
```

---

## 💳 Transações

### PIX QR Code - Gerar QR Code PIX

```bash
curl -X POST https://localhost:5001/api/transactions/pix-qrcode \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." \
  -H "Content-Type: application/json" \
  -d '{
    "amount": 100.00,
    "description": "Pagamento de fatura",
    "recipientKey": "recipient@example.com"
  }'
```

**Resposta:**
```json
{
  "success": true,
  "data": {
    "transactionId": "550e8400-e29b-41d4-a716-446655440000",
    "qrCode": "00020126580014br.gov.bcb.pix0136550e8400-e29b-41d4-a716-446655440000520400005303986540510.005802BR5913FINTECH6009SAO PAULO62410503***63041D3D",
    "status": "PENDING"
  }
}
```

### Withdrawal - Solicitar Saque

```bash
curl -X POST https://localhost:5001/api/transactions/withdrawal \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." \
  -H "Content-Type: application/json" \
  -d '{
    "amount": 50.00,
    "accountNumber": "123456",
    "bankCode": "001"
  }'
```

**Resposta:**
```json
{
  "success": true,
  "data": {
    "transactionId": "550e8400-e29b-41d4-a716-446655440000",
    "status": "PENDING"
  }
}
```

### Get Transaction Status - Verificar Status

```bash
curl -X GET https://localhost:5001/api/transactions/550e8400-e29b-41d4-a716-446655440000/status \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

**Resposta:**
```json
{
  "success": true,
  "data": {
    "transactionId": "550e8400-e29b-41d4-a716-446655440000",
    "status": "COMPLETED",
    "amount": 100.00,
    "type": "PIX",
    "createdAt": "2025-10-21T10:30:00Z",
    "completedAt": "2025-10-21T10:31:00Z"
  }
}
```

### Get Transaction History - Obter Histórico

```bash
curl -X GET https://localhost:5001/api/transactions/history \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

**Resposta:**
```json
{
  "success": true,
  "data": [
    {
      "transactionId": "550e8400-e29b-41d4-a716-446655440000",
      "type": "PIX",
      "amount": 100.00,
      "status": "COMPLETED",
      "createdAt": "2025-10-21T10:30:00Z"
    },
    {
      "transactionId": "550e8400-e29b-41d4-a716-446655440001",
      "type": "WITHDRAWAL",
      "amount": 50.00,
      "status": "PENDING",
      "createdAt": "2025-10-21T10:35:00Z"
    }
  ]
}
```

---

## 🔗 Webhooks

### Sicoob Webhook - PIX Recebido

```bash
curl -X POST https://localhost:5001/api/webhooks/sicoob \
  -H "Content-Type: application/json" \
  -H "X-Sicoob-Signature: signature-hash" \
  -d '{
    "event": "pix.received",
    "transactionId": "550e8400-e29b-41d4-a716-446655440000",
    "amount": 100.00,
    "status": "COMPLETED",
    "timestamp": "2025-10-21T10:30:00Z"
  }'
```

**Resposta:**
```json
{
  "success": true,
  "message": "Webhook processado com sucesso"
}
```

### Sicoob Webhook - Saque Completado

```bash
curl -X POST https://localhost:5001/api/webhooks/sicoob \
  -H "Content-Type: application/json" \
  -H "X-Sicoob-Signature: signature-hash" \
  -d '{
    "event": "withdrawal.completed",
    "transactionId": "550e8400-e29b-41d4-a716-446655440001",
    "amount": 50.00,
    "status": "COMPLETED",
    "timestamp": "2025-10-21T10:35:00Z"
  }'
```

**Resposta:**
```json
{
  "success": true,
  "message": "Webhook processado com sucesso"
}
```

---

## 🔄 Fluxo Completo

### 1. Registrar
```bash
curl -X POST https://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "novo@example.com",
    "password": "Novo123!",
    "fullName": "Novo User",
    "document": "12345678901",
    "phoneNumber": "11999999999"
  }'
```

### 2. Login
```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "novo@example.com",
    "password": "Novo123!"
  }'
```

**Copie o token da resposta**

### 3. Obter Saldo
```bash
curl -X GET https://localhost:5001/api/accounts/balance \
  -H "Authorization: Bearer <TOKEN>"
```

### 4. Gerar PIX QR Code
```bash
curl -X POST https://localhost:5001/api/transactions/pix-qrcode \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "amount": 100.00,
    "description": "Pagamento",
    "recipientKey": "recipient@example.com"
  }'
```

### 5. Verificar Histórico
```bash
curl -X GET https://localhost:5001/api/transactions/history \
  -H "Authorization: Bearer <TOKEN>"
```

---

## 📝 Notas

- Substitua `<TOKEN>` pelo token JWT obtido no login
- Substitua `550e8400-e29b-41d4-a716-446655440000` pelo ID real
- Use `-k` ou `--insecure` se tiver problemas com SSL em desenvolvimento
- Todos os valores monetários estão em reais (R$)

---

**Status: ✅ Exemplos Prontos para Uso**

---

*Última atualização: 2025-10-21*

