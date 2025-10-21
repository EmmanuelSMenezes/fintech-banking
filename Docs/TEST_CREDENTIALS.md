# 🔐 Credenciais de Teste - FinTech Banking

## ✅ Usuários de Teste Criados

### Usuário Principal

```
Email: emmanuel@fintech.com
Senha: Senha123!
Nome: Emmanuel Test
CPF: 12345678901
Telefone: 11987654321
User ID: acda72ee-1c77-4108-97c9-159b82b02618
```

## 🚀 Como Testar

### 1. Via Frontend (Recomendado)

1. Abra http://localhost:5173
2. Clique em "Registre-se" ou use as credenciais acima
3. Faça login com:
   - Email: `emmanuel@fintech.com`
   - Senha: `Senha123!`

### 2. Via Swagger

1. Acesse http://localhost:5064/swagger
2. Clique em "Authorize"
3. Faça login para obter um token
4. Use o token para testar endpoints protegidos

### 3. Via cURL

**Registrar novo usuário:**
```bash
curl -X POST http://localhost:5064/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "newuser@example.com",
    "password": "Senha123!",
    "fullName": "New User",
    "document": "12345678901",
    "phoneNumber": "11987654321"
  }'
```

**Fazer login:**
```bash
curl -X POST http://localhost:5064/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "emmanuel@fintech.com",
    "password": "Senha123!"
  }'
```

**Usar token para acessar endpoints protegidos:**
```bash
curl -X GET http://localhost:5064/api/accounts/balance \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

## 📊 Endpoints Disponíveis

### Auth
- `POST /api/auth/register` - Registrar novo usuário
- `POST /api/auth/login` - Fazer login

### Accounts
- `GET /api/accounts/balance` - Obter saldo (requer token)
- `GET /api/accounts/details` - Obter detalhes da conta (requer token)

### Transactions
- `POST /api/transactions/pix-qrcode` - Gerar QR Code PIX (requer token)
- `POST /api/transactions/withdrawal` - Solicitar saque (requer token)
- `GET /api/transactions/{id}` - Obter status da transação (requer token)
- `GET /api/transactions/history` - Obter histórico (requer token)

## 🔑 Formato do Token JWT

Após fazer login, você receberá um token JWT com este formato:

```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "tokenType": "Bearer",
  "expiresIn": "2025-10-21T21:53:59.7985008Z"
}
```

**Use assim:**
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## 🧪 Teste Rápido

1. **Registre um novo usuário:**
   ```bash
   POST http://localhost:5064/api/auth/register
   ```

2. **Faça login:**
   ```bash
   POST http://localhost:5064/api/auth/login
   ```

3. **Copie o token retornado**

4. **Teste um endpoint protegido:**
   ```bash
   GET http://localhost:5064/api/accounts/balance
   Header: Authorization: Bearer <seu_token>
   ```

## 📝 Notas Importantes

- Tokens expiram após o tempo especificado em `expiresIn`
- Sempre use HTTPS em produção (aqui usamos HTTP para desenvolvimento)
- Senhas são hasheadas no banco de dados
- CPF e Telefone são validados no registro
- Email deve ser único

## 🔄 Fluxo de Autenticação

```
1. Usuário faz login
   ↓
2. API valida credenciais
   ↓
3. API gera JWT token
   ↓
4. Frontend armazena token em localStorage
   ↓
5. Frontend envia token em cada requisição protegida
   ↓
6. API valida token e processa requisição
```

## ⚠️ Troubleshooting

### Erro 401 - Não Autorizado
- Verifique se o token está correto
- Verifique se o token não expirou
- Verifique se está usando o formato correto: `Bearer <token>`

### Erro 400 - Bad Request
- Verifique se todos os campos obrigatórios foram enviados
- Verifique o formato do JSON

### Erro 500 - Internal Server Error
- Verifique se a API está rodando
- Verifique os logs da API
- Verifique se o banco de dados está acessível

---

**Última atualização:** 2025-10-21

