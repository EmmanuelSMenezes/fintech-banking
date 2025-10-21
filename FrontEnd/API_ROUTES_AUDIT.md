# 🔍 Auditoria de Rotas de API e React

**Data:** 2025-10-21  
**Status:** ⚠️ Requer Correções

---

## 📋 Resumo Executivo

### ❌ Problemas Identificados

1. **Rotas de API Incorretas nos Serviços**
   - Admin Dashboard: Chamando `/api/admin/clientes` (não existe)
   - Admin Dashboard: Chamando `/api/admin/usuarios` (não existe)
   - Admin Dashboard: Chamando `/api/admin/transacoes` (não existe)
   - Internet Banking: Chamando `/api/cliente/profile` (não existe)
   - Internet Banking: Chamando `/api/cliente/perfil` (correto, mas não implementado no backend)

2. **Rotas do React Faltando**
   - Página `/clientes` existe mas não está em `paths.ts`
   - Página `/transacoes` existe mas não está em `paths.ts`
   - Página `/perfil` existe mas não está em `paths.ts`

3. **Proteção de Rotas**
   - Página inicial (`/`) não redireciona para login se não autenticado
   - Não há middleware de autenticação global

---

## 🔌 Endpoints Implementados no Backend

### API Interna (Porta 5036)

| Método | Endpoint | Status | Implementado |
|--------|----------|--------|--------------|
| POST | `/api/auth/login` | ✅ | Sim |
| POST | `/api/auth/register` | ❓ | Não verificado |
| GET | `/api/admin/dashboard` | ✅ | Sim |
| GET | `/api/admin/users` | ✅ | Sim |
| GET | `/api/admin/users/{id}` | ✅ | Sim |
| GET | `/api/admin/transactions` | ✅ | Sim |
| GET | `/api/admin/reports/transactions` | ✅ | Sim |
| GET | `/api/cliente/saldo` | ❓ | Não verificado |
| GET | `/api/cliente/transacoes` | ❓ | Não verificado |
| GET | `/api/cliente/perfil` | ❓ | Não verificado |
| PUT | `/api/cliente/perfil` | ❓ | Não verificado |

---

## 📱 Rotas Chamadas nos Frontends

### Admin Dashboard (api.admin.ts)

| Serviço | Endpoint Chamado | Status | Correto |
|---------|------------------|--------|---------|
| authService | POST `/api/auth/login` | ✅ | Sim |
| authService | GET `/api/admin/profile` | ❌ | Não existe |
| clienteService | GET `/api/admin/clientes` | ❌ | Não existe |
| clienteService | POST `/api/admin/clientes` | ❌ | Não existe |
| usuarioService | GET `/api/admin/usuarios` | ❌ | Não existe |
| transacaoService | GET `/api/admin/transacoes` | ❌ | Não existe |
| dashboardService | GET `/api/admin/dashboard` | ✅ | Sim |

### Internet Banking (api.cliente.ts)

| Serviço | Endpoint Chamado | Status | Correto |
|---------|------------------|--------|---------|
| authService | POST `/api/auth/login` | ✅ | Sim |
| authService | GET `/api/cliente/profile` | ❌ | Não existe |
| contaService | GET `/api/cliente/saldo` | ✅ | Sim |
| contaService | GET `/api/cliente/transacoes` | ✅ | Sim |
| contaService | PUT `/api/cliente/perfil` | ✅ | Sim |
| cobrancaService | POST `/api/cliente/cobrancas` | ❓ | Não verificado |
| saqueService | POST `/api/cliente/saques` | ❓ | Não verificado |
| pixService | POST `/api/cliente/pix/qrcode` | ❓ | Não verificado |

---

## 🛣️ Rotas do React

### Admin Dashboard

| Página | Arquivo | Rota | Em paths.ts |
|--------|---------|------|-------------|
| Home | `pages/index.tsx` | `/` | ❌ |
| Login | `pages/auth/login.tsx` | `/auth/login` | ✅ |
| Clientes | `pages/clientes.tsx` | `/clientes` | ❌ |
| Transações | `pages/transacoes.tsx` | `/transacoes` | ❌ |
| Dashboard | `pages/dashboard/app.tsx` | `/dashboard/app` | ✅ |

### Internet Banking

| Página | Arquivo | Rota | Em paths.ts |
|--------|---------|------|-------------|
| Home | `pages/index.tsx` | `/` | ❌ |
| Login | `pages/auth/login.tsx` | `/auth/login` | ✅ |
| Perfil | `pages/perfil.tsx` | `/perfil` | ❌ |
| Dashboard | `pages/dashboard/app.tsx` | `/dashboard/app` | ✅ |

---

## ⚠️ Problemas Críticos

### 1. Página Inicial Não Protegida
- **Problema:** Acessar `/` não redireciona para login
- **Esperado:** Se não autenticado, redirecionar para `/auth/login`
- **Solução:** Adicionar middleware de autenticação em `_app.tsx`

### 2. Endpoints Não Existem
- **Problema:** Serviços chamam endpoints que não existem no backend
- **Exemplos:**
  - `/api/admin/clientes` → Deveria ser `/api/admin/users`
  - `/api/admin/usuarios` → Deveria ser `/api/admin/users`
  - `/api/admin/transacoes` → Deveria ser `/api/admin/transactions`
  - `/api/cliente/profile` → Deveria ser `/api/cliente/perfil`

### 3. Rotas Não Documentadas
- **Problema:** Novas páginas não estão em `paths.ts`
- **Impacto:** Difícil manutenção e navegação inconsistente

---

## ✅ Ações Necessárias

### Fase 1: Corrigir Rotas de API
- [ ] Atualizar `api.admin.ts` com endpoints corretos
- [ ] Atualizar `api.cliente.ts` com endpoints corretos
- [ ] Remover endpoints que não existem

### Fase 2: Atualizar paths.ts
- [ ] Adicionar rota para `/` (home)
- [ ] Adicionar rota para `/clientes`
- [ ] Adicionar rota para `/transacoes`
- [ ] Adicionar rota para `/perfil`

### Fase 3: Implementar Proteção de Rotas
- [ ] Criar middleware de autenticação
- [ ] Redirecionar `/` para `/auth/login` se não autenticado
- [ ] Proteger rotas internas

### Fase 4: Testar
- [ ] Testar login
- [ ] Testar acesso a páginas protegidas
- [ ] Testar redirecionamento de página inicial

---

**Última atualização:** 2025-10-21

