# 🔔 GUIA COMPLETO DE WEBHOOKS - FinTech Banking

## 📋 Visão Geral

O sistema possui **2 tipos de webhooks**:

### 1️⃣ **Webhooks de PIX** (PixWebhookController)
- **Propósito**: Notificar o cliente sobre eventos de PIX Dinâmico
- **Quem registra**: O cliente (usuário final)
- **Quem envia**: O sistema FinTech Banking
- **Eventos**: PIX_RECEIVED, PIX_SENT, PIX_FAILED, etc.

### 2️⃣ **Webhooks do Banco** (WebhooksController)
- **Propósito**: Receber notificações do banco (Sicoob) sobre confirmação de pagamentos
- **Quem registra**: O cliente (usuário final)
- **Quem envia**: O banco (Sicoob)
- **Eventos**: Confirmação de PIX, Saque confirmado, Falha em transação, etc.

---

## 🔌 ENDPOINTS DE WEBHOOKS PIX

### 1. Registrar Webhook
```
POST /api/pix-webhooks/registrar
Authorization: Bearer {{user_token}}
Content-Type: application/json

{
  "eventType": "PIX_RECEIVED",
  "webhookUrl": "https://seu-dominio.com/webhook/pix"
}
```

**Tipos de Eventos Suportados:**
- `PIX_RECEIVED` - PIX recebido
- `PIX_SENT` - PIX enviado
- `PIX_FAILED` - PIX falhou
- `PIX_CONFIRMED` - PIX confirmado
- `PIX_CANCELLED` - PIX cancelado

**Resposta (200 OK):**
```json
{
  "message": "Webhook registrado com sucesso",
  "data": {
    "id": "uuid",
    "eventType": "PIX_RECEIVED",
    "webhookUrl": "https://seu-dominio.com/webhook/pix",
    "isActive": true,
    "retryCount": 0,
    "lastAttempt": null,
    "createdAt": "2025-10-23T10:00:00Z",
    "updatedAt": "2025-10-23T10:00:00Z"
  }
}
```

---

### 2. Listar Webhooks
```
GET /api/pix-webhooks/listar
Authorization: Bearer {{user_token}}
```

**Resposta (200 OK):**
```json
{
  "message": "Webhooks listados com sucesso",
  "data": {
    "webhooks": [
      {
        "id": "uuid",
        "eventType": "PIX_RECEIVED",
        "webhookUrl": "https://seu-dominio.com/webhook/pix",
        "isActive": true,
        "retryCount": 0,
        "lastAttempt": null,
        "createdAt": "2025-10-23T10:00:00Z",
        "updatedAt": "2025-10-23T10:00:00Z"
      }
    ],
    "total": 1
  }
}
```

---

### 3. Testar Webhook
```
POST /api/pix-webhooks/testar/{{webhook_id}}
Authorization: Bearer {{user_token}}
```

**O que faz:**
- Envia uma requisição POST para a URL registrada
- Payload de teste com dados fictícios
- Registra o resultado (sucesso/falha)

**Resposta (200 OK):**
```json
{
  "message": "Webhook testado com sucesso",
  "data": {
    "success": true
  }
}
```

---

### 4. Ativar/Desativar Webhook
```
PUT /api/pix-webhooks/ativar-desativar/{{webhook_id}}
Authorization: Bearer {{user_token}}
Content-Type: application/json

{
  "isActive": true
}
```

**Resposta (200 OK):**
```json
{
  "message": "Webhook ativado com sucesso",
  "data": {
    "id": "uuid",
    "eventType": "PIX_RECEIVED",
    "webhookUrl": "https://seu-dominio.com/webhook/pix",
    "isActive": true,
    "retryCount": 0,
    "lastAttempt": "2025-10-23T10:05:00Z",
    "createdAt": "2025-10-23T10:00:00Z",
    "updatedAt": "2025-10-23T10:05:00Z"
  }
}
```

---

### 5. Deletar Webhook
```
DELETE /api/pix-webhooks/deletar/{{webhook_id}}
Authorization: Bearer {{user_token}}
```

**Resposta (200 OK):**
```json
{
  "message": "Webhook deletado com sucesso"
}
```

---

## 🏦 ENDPOINTS DE WEBHOOKS DO BANCO

### 1. Registrar Webhook do Banco
```
POST /api/webhooks/register
Authorization: Bearer {{user_token}}
Content-Type: application/json

{
  "webhookUrl": "https://seu-dominio.com/webhook/banco"
}
```

**Resposta (200 OK):**
```json
{
  "message": "Webhook registrado com sucesso",
  "webhookUrl": "https://seu-dominio.com/webhook/banco"
}
```

---

### 2. Obter URL do Webhook
```
GET /api/webhooks/url
Authorization: Bearer {{user_token}}
```

**Resposta (200 OK):**
```json
{
  "webhookUrl": "https://seu-dominio.com/webhook/banco"
}
```

---

### 3. Obter Histórico de Webhooks
```
GET /api/webhooks/history?limit=50
Authorization: Bearer {{user_token}}
```

**Resposta (200 OK):**
```json
{
  "message": "Histórico de webhooks",
  "total": 5,
  "data": [
    {
      "id": "uuid",
      "eventType": "PIX_CONFIRMED",
      "status": "SUCCESS",
      "receivedAt": "2025-10-23T10:00:00Z",
      "processedAt": "2025-10-23T10:00:05Z",
      "errorMessage": null
    }
  ]
}
```

---

### 4. Remover Webhook do Banco
```
POST /api/webhooks/unregister
Authorization: Bearer {{user_token}}
```

**Resposta (200 OK):**
```json
{
  "message": "Webhook removido com sucesso"
}
```

---

## 🔄 FLUXO DE FUNCIONAMENTO

### Cenário 1: PIX Dinâmico Recebido

```
1. Cliente cria PIX Dinâmico
   POST /api/pix-dinamico/criar
   
2. Sistema gera QR Code
   
3. Outro usuário escaneia e paga
   
4. Banco confirma pagamento
   
5. Sistema envia webhook para cliente
   POST https://seu-dominio.com/webhook/pix
   {
     "eventType": "PIX_RECEIVED",
     "pixId": "uuid",
     "amount": 250.00,
     "status": "CONFIRMED",
     "timestamp": "2025-10-23T10:00:00Z"
   }
   
6. Cliente recebe notificação em tempo real
```

---

### Cenário 2: Transferência Agendada Executada

```
1. Cliente agenda transferência
   POST /api/transferencias/agendar
   
2. Sistema armazena com status PENDING
   
3. Job executa na data/hora agendada
   
4. Banco confirma transferência
   
5. Sistema envia webhook para cliente
   POST https://seu-dominio.com/webhook/pix
   {
     "eventType": "PIX_SENT",
     "transferId": "uuid",
     "amount": 150.00,
     "status": "EXECUTED",
     "timestamp": "2025-10-23T14:30:00Z"
   }
```

---

## 🧪 COMO TESTAR LOCALMENTE

### Opção 1: Usar ngrok para expor localhost

```bash
# 1. Instalar ngrok
# https://ngrok.com/download

# 2. Expor porta local
ngrok http 3000

# 3. Usar URL gerada
https://abc123.ngrok.io/webhook/pix
```

### Opção 2: Usar webhook.site

```bash
# 1. Acessar https://webhook.site
# 2. Copiar URL gerada
# 3. Registrar no sistema
# 4. Ver requisições em tempo real
```

### Opção 3: Criar endpoint local de teste

```csharp
// Program.cs
app.MapPost("/webhook/pix", (HttpContext context) =>
{
    var body = context.Request.Body;
    Console.WriteLine("Webhook recebido!");
    return Results.Ok();
});
```

---

## 📊 RETRY LOGIC

Se o webhook falhar:
- **Tentativa 1**: Imediato
- **Tentativa 2**: 5 minutos depois
- **Tentativa 3**: 15 minutos depois

Se todas falharem: Registra erro e notifica admin

---

## 🔐 SEGURANÇA

✅ Autenticação JWT obrigatória  
✅ Validação de URL (deve ser HTTPS em produção)  
✅ Rate limiting (100 requisições/60 segundos)  
✅ Isolamento de dados por usuário  
✅ Logging de todas as tentativas  

---

## 📝 POSTMAN COLLECTION

Use a collection: `POSTMAN_API_INTERNA_FASE_4_COMPLETA.json`

Seção: **🔔 Webhooks PIX (Fase 3)**

---

**Status**: 🟢 Pronto para Produção

