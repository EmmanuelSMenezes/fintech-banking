# 📮 Guia de Testes com Postman

## 🎯 Fluxo Completo de Testes

### 1️⃣ Criar Usuário Admin (Backoffice)

**Endpoint**: `POST http://localhost:5066/api/admin/users`

**Headers**:
```
Content-Type: application/json
Authorization: Bearer {admin-token}
```

**Body**:
```json
{
  "email": "admin@fintech.com",
  "fullName": "Administrador",
  "document": "12345678900",
  "phoneNumber": "11999999999"
}
```

**Response**:
```json
{
  "message": "Usuário criado com sucesso",
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "email": "admin@fintech.com",
    "fullName": "Administrador",
    "isActive": true,
    "emailSent": true
  }
}
```

---

### 2️⃣ Criar Usuário Cliente (Backoffice)

**Endpoint**: `POST http://localhost:5066/api/admin/users`

**Headers**:
```
Content-Type: application/json
Authorization: Bearer {admin-token}
```

**Body**:
```json
{
  "email": "cliente@example.com",
  "fullName": "João Silva",
  "document": "98765432100",
  "phoneNumber": "11988888888"
}
```

**Response**:
```json
{
  "message": "Usuário criado com sucesso",
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440001",
    "email": "cliente@example.com",
    "fullName": "João Silva",
    "isActive": true,
    "emailSent": true
  }
}
```

✉️ **Email recebido com**:
- Email: cliente@example.com
- Senha Temporária: (gerada aleatoriamente)
- Link para primeiro acesso

---

### 3️⃣ Cliente Faz Login (Internet Banking)

**Endpoint**: `POST http://localhost:5065/api/auth/login`

**Headers**:
```
Content-Type: application/json
```

**Body**:
```json
{
  "email": "cliente@example.com",
  "password": "senha-temporaria-recebida-no-email"
}
```

**Response**:
```json
{
  "message": "Login realizado com sucesso",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "userId": "550e8400-e29b-41d4-a716-446655440001",
    "email": "cliente@example.com",
    "expiresIn": 3600
  }
}
```

---

### 4️⃣ Cliente Consulta Saldo (Internet Banking)

**Endpoint**: `GET http://localhost:5065/api/transactions/balance`

**Headers**:
```
Authorization: Bearer {client-token}
```

**Response**:
```json
{
  "message": "Saldo obtido com sucesso",
  "data": {
    "balance": 1000.00,
    "currency": "BRL",
    "lastUpdate": "2025-10-21T10:30:00Z"
  }
}
```

---

### 5️⃣ Cliente Gera QR Code PIX (Internet Banking)

**Endpoint**: `POST http://localhost:5065/api/transactions/pix/qrcode`

**Headers**:
```
Authorization: Bearer {client-token}
Content-Type: application/json
```

**Body**:
```json
{
  "amount": 100.00,
  "description": "Pagamento de teste",
  "recipientKey": "chave-pix-destinatario@example.com"
}
```

**Response**:
```json
{
  "message": "QR Code gerado com sucesso",
  "data": {
    "transactionId": "550e8400-e29b-41d4-a716-446655440002",
    "qrCode": "00020126580014br.gov.bcb.pix...",
    "amount": 100.00,
    "status": "PENDING",
    "expiresIn": 3600
  }
}
```

---

### 6️⃣ Cliente Solicita Saque (Internet Banking)

**Endpoint**: `POST http://localhost:5065/api/transactions/withdrawal`

**Headers**:
```
Authorization: Bearer {client-token}
Content-Type: application/json
```

**Body**:
```json
{
  "amount": 500.00,
  "accountNumber": "123456",
  "bankCode": "001"
}
```

**Response**:
```json
{
  "message": "Saque solicitado com sucesso",
  "data": {
    "transactionId": "550e8400-e29b-41d4-a716-446655440003",
    "amount": 500.00,
    "status": "PENDING",
    "createdAt": "2025-10-21T10:35:00Z"
  }
}
```

---

### 7️⃣ Admin Consulta Dashboard (Backoffice)

**Endpoint**: `GET http://localhost:5066/api/admin/dashboard`

**Headers**:
```
Authorization: Bearer {admin-token}
```

**Response**:
```json
{
  "message": "Dashboard carregado com sucesso",
  "data": {
    "userId": "550e8400-e29b-41d4-a716-446655440000",
    "role": "admin",
    "stats": {
      "totalTransactions": 150,
      "totalAmount": 50000.00,
      "pendingTransactions": 10,
      "activeUsers": 500
    },
    "recentTransactions": [
      {
        "id": "550e8400-e29b-41d4-a716-446655440002",
        "type": "PIX",
        "amount": 100.00,
        "status": "COMPLETED",
        "date": "2025-10-21T10:30:00Z"
      }
    ]
  }
}
```

---

### 8️⃣ Admin Lista Usuários (Backoffice)

**Endpoint**: `GET http://localhost:5066/api/admin/users?page=1&pageSize=10`

**Headers**:
```
Authorization: Bearer {admin-token}
```

**Response**:
```json
{
  "message": "Usuários listados com sucesso",
  "data": {
    "users": [
      {
        "id": "550e8400-e29b-41d4-a716-446655440000",
        "email": "admin@fintech.com",
        "fullName": "Administrador",
        "status": "ACTIVE"
      },
      {
        "id": "550e8400-e29b-41d4-a716-446655440001",
        "email": "cliente@example.com",
        "fullName": "João Silva",
        "status": "ACTIVE"
      }
    ],
    "page": 1,
    "pageSize": 10,
    "total": 2
  }
}
```

---

### 9️⃣ Admin Consulta Transações (Backoffice)

**Endpoint**: `GET http://localhost:5066/api/admin/transactions?page=1&pageSize=10`

**Headers**:
```
Authorization: Bearer {admin-token}
```

**Response**:
```json
{
  "message": "Transações listadas com sucesso",
  "data": {
    "transactions": [
      {
        "id": "550e8400-e29b-41d4-a716-446655440002",
        "type": "PIX",
        "amount": 100.00,
        "status": "COMPLETED",
        "date": "2025-10-21T10:30:00Z"
      }
    ],
    "page": 1,
    "pageSize": 10,
    "total": 1
  }
}
```

---

### 🔟 Admin Gera Relatório (Backoffice)

**Endpoint**: `GET http://localhost:5066/api/admin/reports/transactions?startDate=2025-10-01&endDate=2025-10-31`

**Headers**:
```
Authorization: Bearer {admin-token}
```

**Response**:
```json
{
  "message": "Relatório gerado com sucesso",
  "data": {
    "period": {
      "start": "2025-10-01T00:00:00Z",
      "end": "2025-10-31T23:59:59Z"
    },
    "totalTransactions": 150,
    "totalAmount": 50000.00,
    "byType": {
      "pix": {
        "count": 100,
        "amount": 30000.00
      },
      "withdrawal": {
        "count": 50,
        "amount": 20000.00
      }
    },
    "byStatus": {
      "completed": {
        "count": 140,
        "amount": 48000.00
      },
      "pending": {
        "count": 10,
        "amount": 2000.00
      }
    }
  }
}
```

---

## 🔑 Variáveis Postman

Crie estas variáveis para facilitar os testes:

```
{{base_url_cliente}} = http://localhost:5065
{{base_url_interna}} = http://localhost:5066
{{admin_token}} = (copiar do login do admin)
{{client_token}} = (copiar do login do cliente)
{{admin_email}} = admin@fintech.com
{{client_email}} = cliente@example.com
```

---

## 📋 Checklist de Testes

- [ ] CORS funcionando (sem erros no console)
- [ ] Criar usuário admin
- [ ] Criar usuário cliente
- [ ] Email recebido com credenciais
- [ ] Cliente fazer login
- [ ] Cliente consultar saldo
- [ ] Cliente gerar QR Code
- [ ] Cliente solicitar saque
- [ ] Admin consultar dashboard
- [ ] Admin listar usuários
- [ ] Admin listar transações
- [ ] Admin gerar relatório

---

## 🐛 Erros Comuns

| Erro | Solução |
|------|---------|
| 401 Unauthorized | Token expirado ou inválido |
| 403 Forbidden | Usuário não tem permissão (role) |
| 400 Bad Request | Dados inválidos no body |
| 500 Internal Server Error | Erro no servidor (verificar logs) |
| CORS Error | Verificar configuração de CORS |


