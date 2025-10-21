# 🏗️ Arquitetura Atualizada - FinTech Banking

## 📋 Visão Geral

O projeto foi reestruturado para separar claramente as responsabilidades entre **API Cliente** (pública) e **API Interna** (privada), com dois frontends distintos: **Internet Banking** e **Backoffice**.

---

## 🎯 Estrutura de Serviços

### **1. API Cliente (Pública) - Porta 5065**
**Responsabilidade**: Transações de clientes

**Endpoints**:
- `POST /api/auth/register` - Registrar novo cliente
- `POST /api/auth/login` - Login e geração de token JWT
- `POST /api/transactions/pix/qrcode` - Gerar QR Code PIX
- `POST /api/transactions/withdrawal` - Solicitar saque
- `GET /api/transactions/balance` - Obter saldo
- `GET /api/transactions/history` - Histórico de transações
- `GET /api/transactions/{id}/status` - Status de transação

**Autenticação**: JWT Token (Bearer)
**Público**: Sim (CORS habilitado)

---

### **2. API Interna (Privada) - Porta 5066**
**Responsabilidade**: Administração e dados para Internet Banking e Backoffice

**Endpoints**:
- `GET /api/admin/dashboard` - Dashboard (Internet Banking + Backoffice)
- `GET /api/admin/users` - Listar usuários (Backoffice)
- `GET /api/admin/users/{id}` - Detalhes do usuário (Backoffice)
- `GET /api/admin/transactions` - Listar transações (Backoffice)
- `GET /api/admin/reports/transactions` - Relatório de transações (Backoffice)

**Autenticação**: JWT Token (Bearer)
**Público**: Não (apenas para Internet Banking e Backoffice)

---

## 🖥️ Frontends

### **1. Internet Banking - Porta 3000**
**Tecnologia**: Next.js 15 + Tailwind CSS
**Localização**: `fintech-internet-banking/`
**Usuários**: Clientes
**API**: API Cliente (5065)

**Funcionalidades**:
- Login/Registro
- Visualizar saldo
- Histórico de transações
- Gerar QR Code PIX
- Solicitar saque
- Dashboard pessoal

### **2. Backoffice - Porta 3001**
**Tecnologia**: Next.js 15 + Tailwind CSS
**Localização**: `fintech-backoffice/`
**Usuários**: Administradores
**API**: API Interna (5066)

**Funcionalidades**:
- Gerenciar usuários
- Visualizar todas as transações
- Gerar relatórios
- Configurações do sistema
- Dashboard administrativo

---

## 🗄️ Backend Compartilhado

### **Projetos .NET 9**

| Projeto | Responsabilidade |
|---------|------------------|
| **FinTechBanking.Core** | Entities, DTOs, Interfaces |
| **FinTechBanking.Data** | Repositories com Dapper |
| **FinTechBanking.Services** | Autenticação, Mensageria |
| **FinTechBanking.Banking** | Integração com Sicoob |
| **FinTechBanking.Workers** | Consumers (PIX, Saque, Webhook) |
| **FinTechBanking.ConsumerWorker** | Worker Service |
| **FinTechBanking.API.Cliente** | API Pública |
| **FinTechBanking.API.Interna** | API Privada |

---

## 🔐 Autenticação

### **API Cliente**
- **Issuer**: `fintech-banking-cliente`
- **Audience**: `fintech-banking-cliente-api`
- **Expiração**: 60 minutos
- **Role**: `client`

### **API Interna**
- **Issuer**: `fintech-banking-interna`
- **Audience**: `fintech-banking-interna-api`
- **Expiração**: 120 minutos
- **Roles**: `admin`, `user`

---

## 🚀 Portas

| Serviço | Porta | Tipo |
|---------|-------|------|
| Internet Banking | 3000 | Frontend |
| Backoffice | 3001 | Frontend |
| API Cliente | 5065 | Backend |
| API Interna | 5066 | Backend |
| PostgreSQL | 5432 | Database |
| RabbitMQ | 5672 | Message Broker |

---

## 📊 Fluxo de Dados

```
Cliente
  ↓
Internet Banking (3000)
  ↓
API Cliente (5065) ← JWT Token
  ↓
┌─────────────────────────────────┐
│ PostgreSQL │ RabbitMQ │ Sicoob  │
└─────────────────────────────────┘
  ↓
Consumer Worker (Background)

---

Admin
  ↓
Backoffice (3001)
  ↓
API Interna (5066) ← JWT Token
  ↓
┌─────────────────────────────────┐
│ PostgreSQL │ RabbitMQ │ Sicoob  │
└─────────────────────────────────┘
```

---

## ✅ Status

- ✅ API Cliente: Compilada e pronta
- ✅ API Interna: Compilada e pronta
- ✅ Internet Banking: Criada (baseada em template)
- ✅ Backoffice: Criada (baseada em template)
- ✅ Backend compartilhado: Funcional
- ✅ Docker: PostgreSQL + RabbitMQ

---

## 🔧 Próximos Passos

1. Configurar portas diferentes para cada API
2. Atualizar docker-compose.yml
3. Configurar endpoints das APIs nos frontends
4. Testar fluxos completos
5. Implementar testes E2E

