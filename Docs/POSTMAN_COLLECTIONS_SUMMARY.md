# 📮 Resumo das Collections Postman

## 🎯 Visão Geral

Duas collections Postman foram criadas para facilitar os testes da API FinTech Banking:

1. **Postman_API_Interna.json** - Para testes administrativos
2. **Postman_API_Cliente.json** - Para testes de cliente

---

## 📥 Arquivos Criados

### Collections
- ✅ `Postman_API_Interna.json` (4 grupos, 10 endpoints)
- ✅ `Postman_API_Cliente.json` (4 grupos, 14 endpoints)

### Documentação
- ✅ `POSTMAN_GUIDE.md` - Guia completo de uso
- ✅ `CURL_EXAMPLES.md` - Exemplos com curl
- ✅ `POSTMAN_COLLECTIONS_SUMMARY.md` - Este arquivo

---

## 📊 API Interna

### Estrutura
```
Autenticação
├── Register
└── Login

Contas
├── Get Balance
└── Get Account Details

Transações
├── PIX QR Code
├── Withdrawal
├── Get Transaction Status
└── Get Transaction History

Webhooks
├── Sicoob Webhook - PIX
└── Sicoob Webhook - Saque
```

### Endpoints (10 total)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | /api/auth/register | Registrar usuário |
| POST | /api/auth/login | Autenticar usuário |
| GET | /api/accounts/balance | Obter saldo |
| GET | /api/accounts/details | Detalhes da conta |
| POST | /api/transactions/pix-qrcode | Gerar QR Code PIX |
| POST | /api/transactions/withdrawal | Solicitar saque |
| GET | /api/transactions/{id}/status | Status da transação |
| GET | /api/transactions/history | Histórico |
| POST | /api/webhooks/sicoob | Webhook PIX |
| POST | /api/webhooks/sicoob | Webhook Saque |

---

## 👥 API Cliente

### Estrutura
```
Autenticação
├── Register - Novo Cliente
└── Login - Autenticar Cliente

Conta
├── Obter Saldo
└── Obter Detalhes da Conta

Transações
├── Gerar PIX QR Code
├── Solicitar Saque
├── Verificar Status da Transação
└── Obter Histórico de Transações

Exemplos de Fluxo
├── 1. Registrar Novo Cliente
├── 2. Fazer Login
├── 3. Verificar Saldo
├── 4. Gerar PIX QR Code
└── 5. Verificar Histórico
```

### Endpoints (14 total)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | /api/auth/register | Registrar cliente |
| POST | /api/auth/login | Autenticar cliente |
| GET | /api/accounts/balance | Obter saldo |
| GET | /api/accounts/details | Detalhes da conta |
| POST | /api/transactions/pix-qrcode | Gerar QR Code |
| POST | /api/transactions/withdrawal | Solicitar saque |
| GET | /api/transactions/{id}/status | Status |
| GET | /api/transactions/history | Histórico |
| + 5 exemplos de fluxo | - | Requisições prontas |

---

## 🚀 Como Importar

### Passo 1: Abrir Postman
- Abra o Postman
- Clique em "Import" (canto superior esquerdo)

### Passo 2: Selecionar Arquivo
- Clique em "Upload Files"
- Escolha `Postman_API_Interna.json` ou `Postman_API_Cliente.json`

### Passo 3: Importar
- Clique em "Import"
- A collection será adicionada ao seu Postman

### Passo 4: Configurar Variáveis
- Clique na collection
- Vá para "Variables"
- Configure:
  - `token`: Deixe vazio (será preenchido após login)
  - `transactionId`: Deixe vazio (será preenchido após criar transação)

---

## 🔄 Fluxo de Teste Recomendado

### 1. Autenticação
```
1. Register - Criar novo usuário
2. Login - Obter token JWT
3. Copiar token para {{token}}
```

### 2. Contas
```
3. Get Balance - Verificar saldo
4. Get Account Details - Ver detalhes
```

### 3. Transações
```
5. PIX QR Code - Gerar QR Code
6. Get Transaction History - Ver histórico
7. Get Transaction Status - Verificar status
```

### 4. Webhooks (Opcional)
```
8. Sicoob Webhook - PIX - Simular webhook
9. Sicoob Webhook - Saque - Simular webhook
```

---

## 📝 Variáveis Disponíveis

### Global Variables
```
token          - JWT Token (obtido após login)
transactionId  - ID da transação (obtido após criar transação)
```

### Como Usar
```
{{token}}           # Substitui pelo token JWT
{{transactionId}}   # Substitui pelo ID da transação
```

---

## 💡 Dicas de Uso

### 1. Salvar Token
Após fazer login, copie o token da resposta e cole em `{{token}}`:
```json
{
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIs..."
  }
}
```

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

## 🧪 Testes Inclusos

### API Interna
- [x] Register
- [x] Login
- [x] Get Balance
- [x] Get Account Details
- [x] PIX QR Code
- [x] Withdrawal
- [x] Get Transaction Status
- [x] Get Transaction History
- [x] Sicoob Webhook - PIX
- [x] Sicoob Webhook - Saque

### API Cliente
- [x] Register - Novo Cliente
- [x] Login - Autenticar Cliente
- [x] Obter Saldo
- [x] Obter Detalhes da Conta
- [x] Gerar PIX QR Code
- [x] Solicitar Saque
- [x] Verificar Status da Transação
- [x] Obter Histórico de Transações
- [x] Exemplos de Fluxo (5 requisições)

---

## 📚 Documentação Relacionada

- **POSTMAN_GUIDE.md** - Guia completo de uso
- **CURL_EXAMPLES.md** - Exemplos com curl
- **TEST_SICOOB_SANDBOX.md** - Como testar com Sandbox
- **API_EXAMPLES.md** - Exemplos de API

---

## 🎯 Próximos Passos

1. ✅ Importar collections
2. ✅ Configurar variáveis
3. ✅ Testar endpoints
4. ✅ Validar respostas
5. ✅ Testar fluxos completos
6. ⏳ Implementar Fase 12 (Testes)
7. ⏳ Deploy em staging

---

## 📊 Resumo

```
Collections:        2 (Interna + Cliente)
Endpoints:          24 (10 + 14)
Grupos:             8 (4 + 4)
Variáveis:          2 (token, transactionId)
Documentação:       3 arquivos
Status:             ✅ Pronto para uso
```

---

## 🔗 Links Úteis

- Postman: https://www.postman.com/
- Documentação Postman: https://learning.postman.com/
- API Base URL: https://localhost:5001
- Swagger: https://localhost:5001/swagger

---

**Status: ✅ Collections Prontas para Uso**

Comece testando agora!

---

*Última atualização: 2025-10-21*

