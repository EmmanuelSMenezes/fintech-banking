# 🚀 Quick Start - Testando o Projeto

## ✅ Todos os Serviços Estão Rodando!

```
✅ Frontend:        http://localhost:5173
✅ API Backend:     http://localhost:5064
✅ Swagger:         http://localhost:5064/swagger
✅ Consumer Worker: Rodando em background
✅ PostgreSQL:      Rodando em Docker
✅ RabbitMQ:        Rodando em Docker
```

---

## 🧪 Teste 1: Frontend Web (Mais Fácil)

### Passo 1: Abra o Frontend
```
http://localhost:5173
```

### Passo 2: Registre uma Conta
- Email: `test@example.com`
- Senha: `Test@123`
- Clique em "Register"

### Passo 3: Faça Login
- Email: `test@example.com`
- Senha: `Test@123`
- Clique em "Login"

### Passo 4: Teste as Funcionalidades
- Veja seu saldo
- Gere um QR Code PIX
- Solicite um saque

---

## 🧪 Teste 2: Postman (Mais Completo)

### Passo 1: Importe a Collection
1. Abra Postman
2. Clique em "Import"
3. Selecione `Postman_API_Cliente.json`

### Passo 2: Registre um Usuário
```
POST http://localhost:5064/api/auth/register
Content-Type: application/json

{
  "email": "postman@example.com",
  "password": "Test@123"
}
```

### Passo 3: Faça Login
```
POST http://localhost:5064/api/auth/login
Content-Type: application/json

{
  "email": "postman@example.com",
  "password": "Test@123"
}
```

**Copie o token da resposta!**

### Passo 4: Configure o Bearer Token
1. Na collection, vá para "Authorization"
2. Selecione "Bearer Token"
3. Cole o token que você copiou

### Passo 5: Teste os Endpoints
- `GET /api/accounts/{accountNumber}/balance` - Obter saldo
- `POST /api/transactions/pix/qrcode` - Gerar QR Code
- `POST /api/transactions/withdrawal` - Solicitar saque

---

## 🧪 Teste 3: Curl (Linha de Comando)

### Registrar Usuário
```bash
curl -X POST http://localhost:5064/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email":"curl@example.com",
    "password":"Test@123"
  }'
```

### Fazer Login
```bash
curl -X POST http://localhost:5064/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email":"curl@example.com",
    "password":"Test@123"
  }'
```

**Copie o token da resposta!**

### Obter Saldo
```bash
curl -X GET http://localhost:5064/api/accounts/001/balance \
  -H "Authorization: Bearer SEU_TOKEN_AQUI"
```

### Gerar QR Code PIX
```bash
curl -X POST http://localhost:5064/api/transactions/pix/qrcode \
  -H "Authorization: Bearer SEU_TOKEN_AQUI" \
  -H "Content-Type: application/json" \
  -d '{
    "amount": 100.00,
    "description": "Pagamento teste"
  }'
```

---

## 📊 Swagger UI

Acesse http://localhost:5064/swagger para:
- Ver todos os endpoints
- Testar diretamente no navegador
- Ver documentação completa

---

## 🔍 Verificar Status dos Serviços

### Verificar API
```bash
curl http://localhost:5064/health
```

### Verificar Frontend
```bash
curl http://localhost:5173
```

### Verificar PostgreSQL
```bash
docker ps | findstr postgres
```

### Verificar RabbitMQ
```bash
docker ps | findstr rabbitmq
```

---

## 🐛 Troubleshooting

### Erro: "Connection refused"
- Verifique se todos os serviços estão rodando
- Execute: `docker-compose ps`

### Erro: "Invalid token"
- Faça login novamente
- Copie o novo token
- Use o novo token nos requests

### Erro: "Database connection failed"
- Verifique se PostgreSQL está rodando
- Execute: `docker-compose logs postgres`

### Erro: "RabbitMQ connection failed"
- Verifique se RabbitMQ está rodando
- Execute: `docker-compose logs rabbitmq`

---

## 📝 Dados de Teste

### Usuário Teste
```
Email: test@example.com
Senha: Test@123
```

### Conta Teste
```
Account Number: 001
Bank Code: 001 (Sicoob)
```

### Valores Teste
```
Saldo Inicial: 10.000,00
PIX QR Code: Qualquer valor
Saque: Qualquer valor
```

---

## 🎯 Fluxo Completo de Teste

1. ✅ Registre um usuário
2. ✅ Faça login
3. ✅ Obtenha o saldo
4. ✅ Gere um QR Code PIX
5. ✅ Solicite um saque
6. ✅ Verifique o status da transação
7. ✅ Obtenha o histórico de transações

---

## 📮 Collections Disponíveis

- **Postman_API_Interna.json** - Para testes administrativos
- **Postman_API_Cliente.json** - Para testes de cliente

---

## 📖 Documentação Completa

- **POSTMAN_GUIDE.md** - Guia detalhado
- **CURL_EXAMPLES.md** - Exemplos com curl
- **TEST_SICOOB_SANDBOX.md** - Testes com Sicoob

---

**Pronto para testar? Comece agora! 🚀**

Abra http://localhost:5173 ou use Postman!

