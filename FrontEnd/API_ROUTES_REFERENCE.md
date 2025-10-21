# 🔗 FinTech Banking - API Routes Reference

**Data:** 2025-10-21  
**Status:** ✅ Atualizado

---

## 📍 Base URL

```
http://localhost:5036
```

---

## 🏢 Admin Dashboard (Porta 3000)

### Autenticação
```
POST   /api/auth/login              - Login administrador
POST   /api/auth/register           - Registrar administrador
POST   /api/auth/logout             - Logout
GET    /api/admin/profile           - Obter perfil do admin
```

### Clientes
```
GET    /api/admin/clientes          - Listar clientes
GET    /api/admin/clientes/{id}     - Obter cliente por ID
POST   /api/admin/clientes          - Criar cliente
PUT    /api/admin/clientes/{id}     - Atualizar cliente
DELETE /api/admin/clientes/{id}     - Deletar cliente
```

### Usuários
```
GET    /api/admin/usuarios          - Listar usuários
GET    /api/admin/usuarios/{id}     - Obter usuário por ID
POST   /api/admin/usuarios          - Criar usuário
PUT    /api/admin/usuarios/{id}     - Atualizar usuário
DELETE /api/admin/usuarios/{id}     - Deletar usuário
```

### Transações
```
GET    /api/admin/transacoes        - Listar transações
GET    /api/admin/transacoes/{id}   - Obter transação por ID
GET    /api/admin/clientes/{id}/transacoes - Transações por cliente
```

### Webhooks
```
GET    /api/admin/webhooks/logs     - Listar logs de webhooks
GET    /api/admin/webhooks/logs/{id} - Obter log específico
```

### Liberações Manuais
```
POST   /api/admin/liberacoes        - Executar liberação manual
GET    /api/admin/liberacoes        - Listar liberações
```

### Dashboard
```
GET    /api/admin/dashboard         - Obter métricas do dashboard
```

---

## 💻 Internet Banking (Porta 3001)

### Autenticação
```
POST   /api/auth/login              - Login cliente
POST   /api/auth/register           - Registrar cliente
POST   /api/auth/logout             - Logout
GET    /api/cliente/profile         - Obter perfil do cliente
POST   /api/cliente/alterar-senha   - Alterar senha
```

### Saldo e Extrato
```
GET    /api/cliente/saldo           - Obter saldo
GET    /api/cliente/extrato         - Obter extrato
GET    /api/cliente/transacoes      - Listar transações
GET    /api/cliente/transacoes/{id} - Obter transação por ID
PUT    /api/cliente/perfil          - Atualizar perfil
```

### Cobranças
```
POST   /api/cliente/cobrancas       - Gerar cobrança
GET    /api/cliente/cobrancas       - Listar cobranças
GET    /api/cliente/cobrancas/{id}  - Obter cobrança por ID
POST   /api/cliente/cobrancas/{id}/cancelar - Cancelar cobrança
```

### Saques
```
POST   /api/cliente/saques          - Solicitar saque
GET    /api/cliente/saques          - Listar saques
GET    /api/cliente/saques/{id}     - Obter saque por ID
POST   /api/cliente/saques/{id}/cancelar - Cancelar saque
```

### PIX
```
POST   /api/cliente/pix/qrcode      - Gerar QR Code PIX
POST   /api/cliente/pix/send        - Enviar PIX
```

### Dashboard
```
GET    /api/cliente/dashboard       - Obter métricas do dashboard
```

---

## 📦 Estrutura de Resposta

### Sucesso (200)
```json
{
  "success": true,
  "data": {
    // dados da resposta
  },
  "message": "Operação realizada com sucesso"
}
```

### Erro (400, 401, 404, 500)
```json
{
  "success": false,
  "data": null,
  "message": "Descrição do erro"
}
```

---

## 🔐 Autenticação

### Headers Obrigatórios
```
Authorization: Bearer {accessToken}
Content-Type: application/json
```

### Tokens
- **accessToken**: JWT com expiração de 1 hora
- **refreshToken**: Token para renovar accessToken
- **expiresIn**: Data/hora de expiração

---

## 📝 Exemplos de Requisição

### Login Admin
```bash
curl -X POST http://localhost:5036/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@fintech.com",
    "password": "Admin123!"
  }'
```

### Login Cliente
```bash
curl -X POST http://localhost:5036/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "cliente@fintech.com",
    "password": "Cliente123!"
  }'
```

### Obter Saldo
```bash
curl -X GET http://localhost:5036/api/cliente/saldo \
  -H "Authorization: Bearer {accessToken}"
```

### Listar Clientes (Admin)
```bash
curl -X GET http://localhost:5036/api/admin/clientes \
  -H "Authorization: Bearer {accessToken}"
```

---

## 🔄 Fluxo de Autenticação

1. **Login** → POST `/api/auth/login`
   - Retorna: `accessToken`, `refreshToken`, `expiresIn`

2. **Armazenar Token** → localStorage
   - `localStorage.setItem('accessToken', accessToken)`

3. **Usar Token** → Adicionar ao header
   - `Authorization: Bearer {accessToken}`

4. **Renovar Token** → Quando expirar
   - Usar `refreshToken` para obter novo `accessToken`

5. **Logout** → POST `/api/auth/logout`
   - Limpar localStorage

---

## 🛠️ Serviços Disponíveis

### Admin Dashboard
```typescript
import { 
  authService, 
  clienteService, 
  usuarioService, 
  transacaoService,
  webhookService,
  liberacaoService,
  dashboardService 
} from '@/services/api.admin';

// Exemplo
const clientes = await clienteService.list();
```

### Internet Banking
```typescript
import { 
  authService, 
  contaService, 
  cobrancaService, 
  saqueService,
  pixService,
  dashboardService 
} from '@/services/api.cliente';

// Exemplo
const saldo = await contaService.getSaldo();
```

---

## ⚠️ Códigos de Erro

| Código | Descrição |
|--------|-----------|
| 200 | OK - Sucesso |
| 400 | Bad Request - Dados inválidos |
| 401 | Unauthorized - Token inválido/expirado |
| 403 | Forbidden - Sem permissão |
| 404 | Not Found - Recurso não encontrado |
| 500 | Internal Server Error - Erro do servidor |

---

## 📚 Documentação Completa

- **Swagger:** http://localhost:5036/swagger
- **Admin Dashboard:** http://localhost:3000
- **Internet Banking:** http://localhost:3001

---

**Última atualização:** 2025-10-21

