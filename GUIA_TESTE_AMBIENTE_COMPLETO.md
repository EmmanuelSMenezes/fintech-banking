# 🧪 GUIA DE TESTE - AMBIENTE COMPLETO (Fase 4)

## 🚀 PASSO 1: PREPARAR O AMBIENTE

### 1.1 Iniciar os Serviços

```powershell
# Terminal 1: Backend API Interna
cd Backend
dotnet run --project src/FinTechBanking.API.Interna/FinTechBanking.API.Interna.csproj
# Rodando em: http://localhost:5036

# Terminal 2: Backend API Cliente
cd Backend
dotnet run --project src/FinTechBanking.API.Cliente/FinTechBanking.API.Cliente.csproj
# Rodando em: http://localhost:5167

# Terminal 3: RabbitMQ (se usando Docker)
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management

# Terminal 4: PostgreSQL (se usando Docker)
docker run -d --name postgres -p 5432:5432 -e POSTGRES_PASSWORD=postgres postgres:15
```

### 1.2 Verificar Conectividade

```bash
# Testar API Interna
curl http://localhost:5036/health

# Testar API Cliente
curl http://localhost:5167/health

# Testar RabbitMQ
curl http://localhost:15672
```

---

## 🔐 PASSO 2: AUTENTICAÇÃO

### 2.1 Login Admin

**Postman:**
```
POST http://localhost:5036/api/auth/login
Content-Type: application/json

{
  "email": "admin@fintech.com",
  "password": "Admin@123"
}
```

**Resposta:**
```json
{
  "message": "Login realizado com sucesso",
  "data": {
    "userId": "uuid-admin",
    "email": "admin@fintech.com",
    "fullName": "Admin",
    "role": "admin"
  },
  "accessToken": "eyJhbGciOiJIUzI1NiIs..."
}
```

**Salvar em Postman:**
- Copiar o `accessToken`
- Ir em: Collections → Variables
- Colar em `admin_token`

### 2.2 Login Cliente

```
POST http://localhost:5036/api/auth/login
Content-Type: application/json

{
  "email": "cliente@example.com",
  "password": "Senha@123"
}
```

**Salvar em Postman:**
- Copiar o `accessToken`
- Ir em: Collections → Variables
- Colar em `user_token`

---

## 💰 PASSO 3: TESTAR TRANSAÇÕES PIX

### 3.1 Criar Transação

```
POST http://localhost:5036/api/transferencias
Authorization: Bearer {{user_token}}
Content-Type: application/json

{
  "accountId": "{{account_id}}",
  "recipientKey": "12345678901234567890123456789012",
  "amount": 100.00,
  "description": "Pagamento de teste"
}
```

**Resposta esperada:** 200 OK

### 3.2 Listar Transações

```
GET http://localhost:5036/api/transferencias?page=1&pageSize=10
Authorization: Bearer {{user_token}}
```

**Resposta esperada:** 200 OK com lista de transações

---

## 🎯 PASSO 4: TESTAR PIX DINÂMICO (Fase 2)

### 4.1 Criar PIX Dinâmico

```
POST http://localhost:5036/api/pix-dinamico/criar
Authorization: Bearer {{user_token}}
Content-Type: application/json

{
  "accountId": "{{account_id}}",
  "amount": 250.00,
  "description": "Cobrança PIX Dinâmico",
  "expirationMinutes": 30
}
```

**Resposta esperada:** 200 OK
```json
{
  "message": "PIX Dinâmico criado com sucesso",
  "data": {
    "id": "uuid-pix",
    "qrCode": "00020126580014br.gov.bcb.pix...",
    "amount": 250.00,
    "status": "PENDING",
    "expiresAt": "2025-10-23T11:00:00Z"
  }
}
```

**Salvar em Postman:**
- Copiar o `id`
- Ir em: Collections → Variables
- Colar em `pix_id`

### 4.2 Listar PIX Dinâmicos

```
GET http://localhost:5036/api/pix-dinamico/listar
Authorization: Bearer {{user_token}}
```

---

## 🔔 PASSO 5: TESTAR WEBHOOKS PIX (Fase 3)

### 5.1 Registrar Webhook

```
POST http://localhost:5036/api/pix-webhooks/registrar
Authorization: Bearer {{user_token}}
Content-Type: application/json

{
  "eventType": "PIX_RECEIVED",
  "webhookUrl": "https://webhook.site/seu-uuid-aqui"
}
```

**Resposta esperada:** 200 OK
```json
{
  "message": "Webhook registrado com sucesso",
  "data": {
    "id": "uuid-webhook",
    "eventType": "PIX_RECEIVED",
    "webhookUrl": "https://webhook.site/seu-uuid-aqui",
    "isActive": true,
    "retryCount": 0,
    "lastAttempt": null,
    "createdAt": "2025-10-23T10:00:00Z"
  }
}
```

**Salvar em Postman:**
- Copiar o `id`
- Ir em: Collections → Variables
- Colar em `webhook_id`

### 5.2 Testar Webhook

```
POST http://localhost:5036/api/pix-webhooks/testar/{{webhook_id}}
Authorization: Bearer {{user_token}}
```

**O que acontece:**
- Sistema envia POST para a URL registrada
- Você verá a requisição em webhook.site
- Resposta: 200 OK com `"success": true`

### 5.3 Listar Webhooks

```
GET http://localhost:5036/api/pix-webhooks/listar
Authorization: Bearer {{user_token}}
```

### 5.4 Ativar/Desativar Webhook

```
PUT http://localhost:5036/api/pix-webhooks/ativar-desativar/{{webhook_id}}
Authorization: Bearer {{user_token}}
Content-Type: application/json

{
  "isActive": false
}
```

---

## 📅 PASSO 6: TESTAR TRANSFERÊNCIAS AGENDADAS (Fase 4)

### 6.1 Agendar Transferência

```
POST http://localhost:5036/api/transferencias/agendar
Authorization: Bearer {{user_token}}
Content-Type: application/json

{
  "accountId": "{{account_id}}",
  "recipientKey": "12345678901234567890123456789012",
  "amount": 150.00,
  "description": "Transferência agendada",
  "scheduledDate": "2025-10-30T14:30:00Z"
}
```

**Resposta esperada:** 200 OK
```json
{
  "message": "Transferência agendada com sucesso",
  "data": {
    "id": "uuid-scheduled",
    "status": "PENDING",
    "amount": 150.00,
    "scheduledDate": "2025-10-30T14:30:00Z",
    "createdAt": "2025-10-23T10:00:00Z"
  }
}
```

**Salvar em Postman:**
- Copiar o `id`
- Ir em: Collections → Variables
- Colar em `scheduled_transfer_id`

### 6.2 Listar Transferências Agendadas

```
GET http://localhost:5036/api/transferencias/agendadas
Authorization: Bearer {{user_token}}
```

### 6.3 Obter Detalhes

```
GET http://localhost:5036/api/transferencias/agendadas/{{scheduled_transfer_id}}
Authorization: Bearer {{user_token}}
```

### 6.4 Cancelar Transferência

```
DELETE http://localhost:5036/api/transferencias/agendadas/{{scheduled_transfer_id}}
Authorization: Bearer {{user_token}}
```

---

## ✅ CHECKLIST DE TESTE

- [ ] API Interna rodando (5036)
- [ ] API Cliente rodando (5167)
- [ ] RabbitMQ rodando (5672)
- [ ] PostgreSQL rodando (5432)
- [ ] Login Admin funcionando
- [ ] Login Cliente funcionando
- [ ] Criar transação PIX
- [ ] Listar transações
- [ ] Criar PIX Dinâmico
- [ ] Registrar webhook
- [ ] Testar webhook
- [ ] Agendar transferência
- [ ] Listar transferências agendadas
- [ ] Cancelar transferência agendada

---

## 🐛 TROUBLESHOOTING

### Erro 401 Unauthorized
- Verificar se o token está correto
- Verificar se o token não expirou
- Fazer login novamente

### Erro 404 Not Found
- Verificar se a porta está correta (5036 para API Interna)
- Verificar se o endpoint está correto
- Verificar se o serviço está rodando

### Erro 500 Internal Server Error
- Verificar logs do backend
- Verificar se PostgreSQL está rodando
- Verificar se RabbitMQ está rodando

### Webhook não recebe notificação
- Verificar se a URL é válida
- Usar webhook.site para testar
- Verificar logs do RabbitMQ

---

## 📊 POSTMAN COLLECTION

**Arquivo:** `Collections_Postman/POSTMAN_API_INTERNA_FASE_4_COMPLETA.json`

**Como importar:**
1. Abrir Postman
2. Clicar em "Import"
3. Selecionar o arquivo JSON
4. Clicar em "Import"

---

**Status**: 🟢 Pronto para Teste

