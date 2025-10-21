# 🧪 Teste com Sandbox Sicoob

## 🎯 Objetivo

Testar a integração com Sicoob usando o ambiente de sandbox.

---

## 🚀 Passo 1: Iniciar Serviços

```bash
# Terminal 1: Docker
docker-compose up -d

# Verificar se está rodando
docker-compose ps
```

---

## 🚀 Passo 2: Compilar Backend

```bash
dotnet build
```

Deve retornar: `Compilação com êxito`

---

## 🚀 Passo 3: Executar Backend

```bash
# Terminal 2: API
cd src/FinTechBanking.API
dotnet run

# Deve exibir:
# info: Microsoft.Hosting.Lifetime[14]
#       Now listening on: https://localhost:5001
```

---

## 🚀 Passo 4: Testar Endpoints

### 1. Registrar Usuário

```bash
curl -X POST https://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email":"test@example.com",
    "password":"Pass123!",
    "fullName":"Test User",
    "document":"12345678901",
    "phoneNumber":"11999999999"
  }'
```

**Resposta esperada:**
```json
{
  "success": true,
  "message": "Usuário registrado com sucesso",
  "data": {
    "id": "uuid",
    "email": "test@example.com"
  }
}
```

### 2. Fazer Login

```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email":"test@example.com",
    "password":"Pass123!"
  }'
```

**Resposta esperada:**
```json
{
  "success": true,
  "message": "Login realizado com sucesso",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIs...",
    "user": {
      "id": "uuid",
      "email": "test@example.com"
    }
  }
}
```

**Copie o token para os próximos testes!**

### 3. Obter Saldo

```bash
curl -X GET https://localhost:5001/api/accounts/balance \
  -H "Authorization: Bearer <TOKEN>"
```

**Resposta esperada:**
```json
{
  "success": true,
  "data": {
    "balance": 1000.00
  }
}
```

### 4. Gerar PIX QR Code

```bash
curl -X POST https://localhost:5001/api/transactions/pix-qrcode \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "amount":100.00,
    "description":"Pagamento de teste",
    "recipientKey":"recipient@example.com"
  }'
```

**Resposta esperada:**
```json
{
  "success": true,
  "data": {
    "transactionId": "uuid",
    "qrCode": "00020126580014br.gov.bcb.pix...",
    "status": "PENDING"
  }
}
```

### 5. Solicitar Saque

```bash
curl -X POST https://localhost:5001/api/transactions/withdrawal \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "amount":50.00,
    "accountNumber":"123456",
    "bankCode":"001"
  }'
```

**Resposta esperada:**
```json
{
  "success": true,
  "data": {
    "transactionId": "uuid",
    "status": "PENDING"
  }
}
```

---

## 🔍 Verificar Logs

### Logs da API

Procure por mensagens como:
```
[Sicoob] Gerando QR Code PIX: 100 para recipient@example.com
[Sicoob] QR Code gerado com sucesso: 00020126580014br.gov.bcb.pix...
```

### Logs do Consumer Worker

```bash
# Terminal 3: Consumer Worker
cd src/FinTechBanking.ConsumerWorker
dotnet run

# Deve exibir:
# [ConsumerHost] Iniciando PixRequestConsumer...
# [ConsumerHost] PixRequestConsumer iniciado
```

---

## 📊 Fluxo Completo de Teste

```
1. Registrar usuário
   ↓
2. Fazer login (obter token)
   ↓
3. Obter saldo
   ↓
4. Gerar PIX QR Code
   ↓
5. Verificar logs da API
   ↓
6. Solicitar saque
   ↓
7. Verificar logs do Consumer
```

---

## ✅ Checklist de Teste

- [ ] Docker rodando
- [ ] Backend compilado
- [ ] API iniciada
- [ ] Usuário registrado
- [ ] Login realizado
- [ ] Saldo obtido
- [ ] PIX QR Code gerado
- [ ] Logs da Sicoob aparecem
- [ ] Saque solicitado
- [ ] Consumer processa requisição

---

## 🐛 Troubleshooting

### Erro: "Connection refused"
- Verifique se Docker está rodando: `docker-compose ps`
- Verifique se API está rodando na porta 5001

### Erro: "Unauthorized"
- Verifique se o token está correto
- Verifique se o token não expirou
- Tente fazer login novamente

### Erro: "Sicoob API error"
- Verifique as credenciais em `appsettings.json`
- Verifique se está usando sandbox: `https://api.sicoob.com.br/sandbox`
- Verifique os logs da API

### Erro: "Database connection failed"
- Verifique se PostgreSQL está rodando: `docker-compose ps`
- Verifique a connection string em `appsettings.json`

---

## 📝 Notas Importantes

1. **Sandbox vs Produção**
   - Sandbox: https://api.sicoob.com.br/sandbox
   - Produção: https://api.sicoob.com.br

2. **Valores em Centavos**
   - API Sicoob usa centavos
   - 100.00 = 10000 centavos

3. **Credenciais**
   - Client ID: 9b5e603e428cc477a2841e2683c92d21
   - Access Token: 1301865f-c6bc-38f3-9f49-666dbcfc59c3

4. **Tokens JWT**
   - Expiram em 60 minutos
   - Faça login novamente se expirar

---

## 🎯 Próximos Passos

1. ✅ Testar endpoints
2. ✅ Validar respostas
3. ⏳ Implementar Fase 12 (Testes)
4. ⏳ Deploy em staging
5. ⏳ Deploy em produção

---

**Status: ✅ PRONTO PARA TESTES**

Comece testando agora!

---

*Última atualização: 2025-10-21*

