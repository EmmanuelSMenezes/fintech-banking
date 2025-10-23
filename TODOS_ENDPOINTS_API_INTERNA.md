# 📋 TODOS OS ENDPOINTS DA API INTERNA (Porta 5036)

## 🎯 OBJETIVO
Listar TODOS os endpoints disponíveis na API Interna e verificar quais estão sendo usados pelos frontends.

---

## ✅ ENDPOINTS IMPLEMENTADOS NA API INTERNA

### 1. **AuthController** - Autenticação
```
POST   /api/auth/login                    - Login com email/senha
POST   /api/auth/register                 - Registrar novo usuário
POST   /api/auth/refresh-token            - Renovar token JWT
```

### 2. **AdminController** - Gerenciamento Admin
```
GET    /api/admin/dashboard               - Dashboard com estatísticas
GET    /api/admin/users                   - Listar usuários (paginado)
GET    /api/admin/users/{id}              - Detalhes do usuário
GET    /api/admin/transactions            - Listar transações (paginado)
GET    /api/admin/reports/transactions    - Relatório de transações
GET    /api/admin/clientes                - Listar clientes (paginado) ✅ NOVO
GET    /api/admin/clientes/{id}           - Detalhes do cliente ✅ NOVO
POST   /api/admin/clientes                - Criar novo cliente ✅ NOVO
PUT    /api/admin/clientes/{id}           - Atualizar cliente ✅ NOVO
POST   /api/admin/fix-admin-role          - Corrigir role de admin
GET    /api/admin/check-admin-role        - Verificar role de admin
POST   /api/admin/migrate-add-role        - Migração de role
```

### 3. **ClienteController** - Dados do Cliente
```
GET    /api/cliente/saldo                 - Obter saldo da conta
GET    /api/cliente/transacoes            - Listar transações do cliente
GET    /api/cliente/perfil                - Obter perfil do cliente
PUT    /api/cliente/perfil                - Atualizar perfil do cliente
POST   /api/cliente/cobrancas             - Gerar cobrança PIX ✅ NOVO
GET    /api/cliente/saques                - Listar saques ✅ NOVO
POST   /api/cliente/saques                - Solicitar saque ✅ NOVO
```

### 4. **PixController** - PIX Dinâmico
```
POST   /api/pix/criar-dinamico            - Criar PIX dinâmico
GET    /api/pix/status/{pixId}            - Obter status do PIX
GET    /api/pix/listar                    - Listar PIX dinâmicos
GET    /api/pix/listar/{pixId}            - Detalhes do PIX
PUT    /api/pix/atualizar/{pixId}         - Atualizar PIX
DELETE /api/pix/deletar/{pixId}           - Deletar PIX
```

### 5. **PixWebhookController** - Webhooks de PIX
```
POST   /api/pix-webhooks/registrar        - Registrar webhook
GET    /api/pix-webhooks/listar           - Listar webhooks
GET    /api/pix-webhooks/{id}             - Detalhes do webhook
POST   /api/pix-webhooks/testar           - Testar webhook
PUT    /api/pix-webhooks/{id}/ativar      - Ativar webhook
PUT    /api/pix-webhooks/{id}/desativar   - Desativar webhook
DELETE /api/pix-webhooks/{id}             - Deletar webhook
```

### 6. **ScheduledTransfersController** - Transferências Agendadas
```
POST   /api/transferencias/agendar        - Agendar transferência
GET    /api/transferencias/agendadas      - Listar agendamentos
GET    /api/transferencias/agendadas/{id} - Detalhes do agendamento
DELETE /api/transferencias/agendadas/{id} - Cancelar agendamento
```

### 7. **AuditController** - Auditoria
```
GET    /api/audit/logs                    - Listar logs de auditoria
GET    /api/audit/logs/{id}               - Detalhes do log
GET    /api/audit/user/{userId}           - Logs do usuário
GET    /api/audit/entity/{entity}         - Logs por entidade
GET    /api/audit/action/{action}         - Logs por ação
```

### 8. **RelatoriosController** - Relatórios
```
GET    /api/relatorios/transacoes         - Relatório de transações
GET    /api/relatorios/usuarios           - Relatório de usuários
GET    /api/relatorios/dashboard          - Relatório do dashboard
```

### 9. **WebhooksController** - Webhooks do Banco
```
POST   /api/webhooks/sicoob               - Receber webhook do Sicoob
GET    /api/webhooks/logs                 - Listar logs de webhooks
```

---

## 📊 RESUMO TOTAL

| Categoria | Endpoints | Status |
|-----------|-----------|--------|
| **Auth** | 3 | ✅ Completo |
| **Admin** | 11 | ✅ Completo |
| **Cliente** | 7 | ✅ Completo |
| **PIX** | 6 | ✅ Completo |
| **Webhooks PIX** | 7 | ✅ Completo |
| **Transferências** | 4 | ✅ Completo |
| **Auditoria** | 5 | ✅ Completo |
| **Relatórios** | 3 | ✅ Completo |
| **Webhooks Banco** | 2 | ✅ Completo |
| **TOTAL** | **48+** | ✅ Implementado |

---

## 🎯 ENDPOINTS USADOS PELOS FRONTENDS

### BackOffice (Admin)
- ✅ POST /api/auth/login
- ✅ GET /api/admin/dashboard
- ✅ GET /api/admin/clientes
- ✅ GET /api/admin/clientes/{id}
- ✅ POST /api/admin/clientes
- ✅ PUT /api/admin/clientes/{id}
- ✅ GET /api/admin/transactions
- ✅ GET /api/admin/users

### InternetBanking (Cliente)
- ✅ POST /api/auth/login
- ✅ POST /api/auth/register
- ✅ GET /api/cliente/saldo
- ✅ GET /api/cliente/transacoes
- ✅ GET /api/cliente/perfil
- ✅ PUT /api/cliente/perfil
- ✅ POST /api/cliente/cobrancas
- ✅ GET /api/cliente/saques
- ✅ POST /api/cliente/saques

---

## 🚀 ENDPOINTS NÃO USADOS (Disponíveis para Expansão)

### PIX Dinâmico
- GET /api/pix/listar
- GET /api/pix/listar/{pixId}
- PUT /api/pix/atualizar/{pixId}
- DELETE /api/pix/deletar/{pixId}

### Webhooks PIX
- GET /api/pix-webhooks/{id}
- POST /api/pix-webhooks/testar
- PUT /api/pix-webhooks/{id}/ativar
- PUT /api/pix-webhooks/{id}/desativar
- DELETE /api/pix-webhooks/{id}

### Transferências Agendadas
- GET /api/transferencias/agendadas
- GET /api/transferencias/agendadas/{id}
- DELETE /api/transferencias/agendadas/{id}

### Auditoria
- GET /api/audit/logs
- GET /api/audit/logs/{id}
- GET /api/audit/user/{userId}
- GET /api/audit/entity/{entity}
- GET /api/audit/action/{action}

### Relatórios
- GET /api/relatorios/transacoes
- GET /api/relatorios/usuarios
- GET /api/relatorios/dashboard

---

**Status**: ✅ **TODOS OS ENDPOINTS IMPLEMENTADOS NA API INTERNA**

Os frontends estão usando os endpoints corretos e têm acesso a MUITO MAIS funcionalidades disponíveis!

