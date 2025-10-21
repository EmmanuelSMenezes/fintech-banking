# 🧪 Exemplos de Uso da API

## 📌 Base URL

```
https://localhost:5001
```

## 🔐 Autenticação

Todos os endpoints (exceto `/auth/*`) requerem um token JWT no header:

```
Authorization: Bearer <seu-token-jwt>
```

---

## 1️⃣ Autenticação

### Registrar Novo Usuário

**Endpoint**: `POST /api/auth/register`

**Request**:
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

**Response** (201 Created):
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "email": "user@example.com",
  "fullName": "John Doe",
  "document": "12345678901",
  "phoneNumber": "11999999999",
  "createdAt": "2025-10-21T10:30:00Z"
}
```

### Fazer Login

**Endpoint**: `POST /api/auth/login`

**Request**:
```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "Password123!"
  }'
```

**Response** (200 OK):
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600,
  "user": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "email": "user@example.com",
    "fullName": "John Doe"
  }
}
```

---

## 2️⃣ Contas

### Obter Saldo

**Endpoint**: `GET /api/accounts/balance`

**Request**:
```bash
curl -X GET https://localhost:5001/api/accounts/balance \
  -H "Authorization: Bearer <seu-token-jwt>"
```

**Response** (200 OK):
```json
{
  "accountId": "550e8400-e29b-41d4-a716-446655440001",
  "balance": 1000.50,
  "bankCode": "001",
  "accountNumber": "123456789",
  "lastUpdated": "2025-10-21T10:30:00Z"
}
```

---

## 3️⃣ Transações

### Gerar QR Code PIX

**Endpoint**: `POST /api/transactions/pix-qrcode`

**Request**:
```bash
curl -X POST https://localhost:5001/api/transactions/pix-qrcode \
  -H "Authorization: Bearer <seu-token-jwt>" \
  -H "Content-Type: application/json" \
  -d '{
    "amount": 100.00,
    "recipientKey": "user@example.com",
    "description": "Pagamento de teste"
  }'
```

**Response** (201 Created):
```json
{
  "transactionId": "550e8400-e29b-41d4-a716-446655440002",
  "qrCode": "00020126580014br.gov.bcb.pix...",
  "amount": 100.00,
  "status": "PENDING",
  "createdAt": "2025-10-21T10:30:00Z"
}
```

### Solicitar Saque

**Endpoint**: `POST /api/transactions/withdrawal`

**Request**:
```bash
curl -X POST https://localhost:5001/api/transactions/withdrawal \
  -H "Authorization: Bearer <seu-token-jwt>" \
  -H "Content-Type: application/json" \
  -d '{
    "amount": 50.00,
    "accountNumber": "123456789",
    "bankCode": "001"
  }'
```

**Response** (201 Created):
```json
{
  "transactionId": "550e8400-e29b-41d4-a716-446655440003",
  "amount": 50.00,
  "status": "PENDING",
  "createdAt": "2025-10-21T10:30:00Z"
}
```

### Obter Status da Transação

**Endpoint**: `GET /api/transactions/{transactionId}`

**Request**:
```bash
curl -X GET https://localhost:5001/api/transactions/550e8400-e29b-41d4-a716-446655440002 \
  -H "Authorization: Bearer <seu-token-jwt>"
```

**Response** (200 OK):
```json
{
  "transactionId": "550e8400-e29b-41d4-a716-446655440002",
  "amount": 100.00,
  "status": "COMPLETED",
  "type": "PIX_QRCODE",
  "createdAt": "2025-10-21T10:30:00Z",
  "updatedAt": "2025-10-21T10:35:00Z"
}
```

---

## 4️⃣ Webhooks

### Receber Notificação do Sicoob

**Endpoint**: `POST /api/webhooks/sicoob`

**Request** (enviado pelo Sicoob):
```bash
curl -X POST https://localhost:5001/api/webhooks/sicoob \
  -H "Content-Type: application/json" \
  -d '{
    "transactionId": "550e8400-e29b-41d4-a716-446655440002",
    "status": "COMPLETED",
    "externalId": "sicoob-123456",
    "timestamp": "2025-10-21T10:35:00Z"
  }'
```

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Webhook processado com sucesso"
}
```

---

## 🧪 Testando com Postman

### 1. Importar Collection

Crie uma nova collection no Postman com as requisições acima.

### 2. Configurar Variáveis

```
{{base_url}} = https://localhost:5001
{{token}} = <seu-token-jwt>
```

### 3. Usar em Requisições

```
Authorization: Bearer {{token}}
```

---

## 🔄 Fluxo Completo de Teste

### 1. Registrar Usuário
```bash
POST /api/auth/register
```

### 2. Fazer Login
```bash
POST /api/auth/login
# Salvar o token retornado
```

### 3. Obter Saldo
```bash
GET /api/accounts/balance
# Usar o token do passo 2
```

### 4. Gerar QR Code PIX
```bash
POST /api/transactions/pix-qrcode
# Usar o token do passo 2
```

### 5. Verificar Status
```bash
GET /api/transactions/{transactionId}
# Usar o token do passo 2
```

---

## ⚠️ Códigos de Erro

| Código | Descrição |
|--------|-----------|
| 200 | OK - Requisição bem-sucedida |
| 201 | Created - Recurso criado |
| 400 | Bad Request - Dados inválidos |
| 401 | Unauthorized - Token inválido/expirado |
| 403 | Forbidden - Sem permissão |
| 404 | Not Found - Recurso não encontrado |
| 500 | Internal Server Error - Erro no servidor |

---

## 💡 Dicas

1. **Sempre use HTTPS** em produção
2. **Guarde o token JWT** com segurança
3. **Tokens expiram em 1 hora** (configurável)
4. **Valide os dados** antes de enviar
5. **Trate os erros** apropriadamente

---

## 🔗 Ferramentas Recomendadas

- **Postman** - Testar APIs
- **Thunder Client** - Extensão VS Code
- **Insomnia** - Cliente REST
- **curl** - Linha de comando

---

**Última atualização**: 2025-10-21

