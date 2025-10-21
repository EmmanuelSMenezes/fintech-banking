# 📮 Guia Postman - FinTech Banking API

## 🎯 Visão Geral

Duas collections Postman foram criadas para testar a API:

1. **Postman_API_Interna.json** - API Interna (Administrativo)
2. **Postman_API_Cliente.json** - API Cliente (Público)

---

## 📥 Como Importar as Collections

### Passo 1: Abrir Postman
- Abra o Postman
- Clique em "Import" (canto superior esquerdo)

### Passo 2: Importar Collection
- Selecione "Upload Files"
- Escolha `Postman_API_Interna.json` ou `Postman_API_Cliente.json`
- Clique em "Import"

### Passo 3: Configurar Variáveis
- Clique na collection
- Vá para "Variables"
- Configure:
  - `token`: Deixe vazio (será preenchido após login)
  - `transactionId`: Deixe vazio (será preenchido após criar transação)

---

## 🔐 API Interna

### Descrição
Collection para testes administrativos e de sistema.

### Endpoints

#### Autenticação
- **Register** - Registrar novo usuário
- **Login** - Autenticar e obter token

#### Contas
- **Get Balance** - Obter saldo
- **Get Account Details** - Obter detalhes da conta

#### Transações
- **PIX QR Code** - Gerar QR Code PIX
- **Withdrawal** - Solicitar saque
- **Get Transaction Status** - Verificar status
- **Get Transaction History** - Histórico

#### Webhooks
- **Sicoob Webhook - PIX** - Simular webhook PIX
- **Sicoob Webhook - Saque** - Simular webhook saque

---

## 👥 API Cliente

### Descrição
Collection para testes de aplicações cliente externas.

### Endpoints

#### Autenticação
- **Register - Novo Cliente** - Registrar novo cliente
- **Login - Autenticar Cliente** - Autenticar cliente

#### Conta
- **Obter Saldo** - Saldo da conta
- **Obter Detalhes da Conta** - Detalhes completos

#### Transações
- **Gerar PIX QR Code** - Gerar QR Code
- **Solicitar Saque** - Solicitar saque
- **Verificar Status da Transação** - Status
- **Obter Histórico de Transações** - Histórico

#### Exemplos de Fluxo
- **1. Registrar Novo Cliente**
- **2. Fazer Login**
- **3. Verificar Saldo**
- **4. Gerar PIX QR Code**
- **5. Verificar Histórico**

---

## 🚀 Como Usar

### Fluxo Básico

#### 1. Registrar Usuário
```
POST /api/auth/register
Body:
{
  "email": "user@example.com",
  "password": "Pass123!",
  "fullName": "John Doe",
  "document": "12345678901",
  "phoneNumber": "11999999999"
}
```

#### 2. Fazer Login
```
POST /api/auth/login
Body:
{
  "email": "user@example.com",
  "password": "Pass123!"
}
```

**Copie o token da resposta e cole em `{{token}}`**

#### 3. Obter Saldo
```
GET /api/accounts/balance
Header: Authorization: Bearer {{token}}
```

#### 4. Gerar PIX QR Code
```
POST /api/transactions/pix-qrcode
Header: Authorization: Bearer {{token}}
Body:
{
  "amount": 100.00,
  "description": "Pagamento",
  "recipientKey": "recipient@example.com"
}
```

#### 5. Verificar Histórico
```
GET /api/transactions/history
Header: Authorization: Bearer {{token}}
```

---

## 📊 Variáveis Disponíveis

### Global Variables
- `token` - JWT Token (obtido após login)
- `transactionId` - ID da transação (obtido após criar transação)

### Como Usar
```
{{token}}           # Substitui pelo token JWT
{{transactionId}}   # Substitui pelo ID da transação
```

---

## 🔄 Fluxo Completo de Teste

### Cenário 1: Novo Cliente
1. Register - Criar novo cliente
2. Login - Autenticar
3. Get Balance - Verificar saldo
4. PIX QR Code - Gerar QR Code
5. Get Transaction History - Ver histórico

### Cenário 2: Saque
1. Login - Autenticar
2. Get Balance - Verificar saldo
3. Withdrawal - Solicitar saque
4. Get Transaction Status - Verificar status
5. Get Transaction History - Ver histórico

### Cenário 3: Webhook
1. Login - Autenticar
2. PIX QR Code - Gerar QR Code
3. Sicoob Webhook - PIX - Simular webhook
4. Get Transaction Status - Verificar status

---

## 💡 Dicas

### 1. Salvar Token
Após fazer login, copie o token da resposta:
```json
{
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIs..."
  }
}
```

Cole em `{{token}}` nas variáveis.

### 2. Usar Exemplos de Fluxo
A collection "API Cliente" tem uma seção "Exemplos de Fluxo" com 5 requisições prontas para testar o fluxo completo.

### 3. Verificar Respostas
Todas as respostas seguem o padrão:
```json
{
  "success": true,
  "message": "Mensagem",
  "data": { ... }
}
```

### 4. Erros Comuns
- **401 Unauthorized** - Token inválido ou expirado
- **400 Bad Request** - Dados inválidos
- **404 Not Found** - Recurso não encontrado
- **500 Internal Server Error** - Erro no servidor

---

## 🧪 Testes Recomendados

### 1. Teste de Autenticação
- [ ] Register com dados válidos
- [ ] Register com email duplicado
- [ ] Login com credenciais corretas
- [ ] Login com credenciais incorretas

### 2. Teste de Conta
- [ ] Get Balance com token válido
- [ ] Get Balance com token inválido
- [ ] Get Account Details com token válido

### 3. Teste de Transações
- [ ] PIX QR Code com valor válido
- [ ] PIX QR Code com valor inválido
- [ ] Withdrawal com saldo suficiente
- [ ] Withdrawal com saldo insuficiente
- [ ] Get Transaction Status
- [ ] Get Transaction History

### 4. Teste de Webhooks
- [ ] Sicoob Webhook - PIX
- [ ] Sicoob Webhook - Saque

---

## 📝 Exemplo Completo

### 1. Registrar
```bash
POST https://localhost:5001/api/auth/register
Content-Type: application/json

{
  "email": "teste@example.com",
  "password": "Teste123!",
  "fullName": "Teste User",
  "document": "12345678901",
  "phoneNumber": "11999999999"
}
```

### 2. Login
```bash
POST https://localhost:5001/api/auth/login
Content-Type: application/json

{
  "email": "teste@example.com",
  "password": "Teste123!"
}
```

**Resposta:**
```json
{
  "success": true,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
      "id": "uuid",
      "email": "teste@example.com"
    }
  }
}
```

### 3. Copiar Token
Copie o valor de `token` e cole em `{{token}}`

### 4. Obter Saldo
```bash
GET https://localhost:5001/api/accounts/balance
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

## 🎯 Próximos Passos

1. Importar as collections
2. Configurar variáveis
3. Testar endpoints
4. Validar respostas
5. Testar fluxos completos

---

**Status: ✅ Collections Prontas para Uso**

Comece testando agora!

---

*Última atualização: 2025-10-21*

