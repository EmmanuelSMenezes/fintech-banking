# 🎨 FLUXO COMPLETO DOS FRONTENDS

## 📊 ARQUITETURA GERAL

```
┌─────────────────────────────────────────────────────────────────┐
│                    FRONTENDS (Next.js 14)                       │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  ┌──────────────────────┐      ┌──────────────────────┐        │
│  │   BackOffice         │      │  InternetBanking     │        │
│  │   (Admin)            │      │  (Cliente)           │        │
│  │   Port: 3000         │      │  Port: 3001          │        │
│  └──────────────────────┘      └──────────────────────┘        │
│           │                              │                      │
│           └──────────────┬───────────────┘                      │
│                          │                                      │
│                    Axios HTTP Client                            │
│                    JWT Bearer Token                             │
│                          │                                      │
└──────────────────────────┼──────────────────────────────────────┘
                           │
        ┌──────────────────┼──────────────────┐
        │                  │                  │
        ▼                  ▼                  ▼
   ┌─────────┐        ┌─────────┐       ┌─────────┐
   │ API     │        │ API     │       │ API     │
   │ Interna │        │ Cliente │       │ Webhooks│
   │ 5036    │        │ 5167    │       │ 5672    │
   └─────────┘        └─────────┘       └─────────┘
```

---

## 🔐 BACKOFFICE - FLUXO ADMIN

### 1️⃣ **Login (Admin)**
```
┌─────────────────────────────────────────┐
│ Página: /login                          │
├─────────────────────────────────────────┤
│ Inputs:                                 │
│  • Email: admin@fintech.com             │
│  • Senha: Admin@123                     │
│                                         │
│ Ação: POST /api/auth/login              │
│ Resposta: { accessToken, refreshToken } │
│ Armazenamento: localStorage.token       │
│                                         │
│ Redirecionamento: /dashboard            │
└─────────────────────────────────────────┘
```

### 2️⃣ **Dashboard (Admin)**
```
┌─────────────────────────────────────────┐
│ Página: /dashboard                      │
├─────────────────────────────────────────┤
│ Requisições:                            │
│  • GET /api/admin/dashboard             │
│                                         │
│ Dados Exibidos:                         │
│  • Total de Transações                  │
│  • Valor Total (R$)                     │
│  • Transações Pendentes                 │
│  • Usuários Ativos                      │
│  • Últimas Transações (tabela)          │
│                                         │
│ Botões:                                 │
│  • Gerenciar Clientes → /clientes       │
│  • Ver Transações → /transacoes         │
└─────────────────────────────────────────┘
```

### 3️⃣ **Transações (Admin)**
```
┌─────────────────────────────────────────┐
│ Página: /transacoes                     │
├─────────────────────────────────────────┤
│ Requisições:                            │
│  • GET /api/admin/transactions          │
│    (com paginação e filtros)            │
│                                         │
│ Filtros:                                │
│  • Status: Todos, Concluída, Pendente   │
│  • Tipo: Todos, PIX, Transferência      │
│                                         │
│ Tabela:                                 │
│  • ID | Tipo | Valor | Status | Data   │
│  • Paginação: 5, 10, 25 por página      │
└─────────────────────────────────────────┘
```

### 4️⃣ **Clientes (Admin)**
```
┌─────────────────────────────────────────┐
│ Página: /clientes                       │
├─────────────────────────────────────────┤
│ Requisições:                            │
│  • GET /api/admin/clients               │
│                                         │
│ Funcionalidades:                        │
│  • Listar clientes                      │
│  • Visualizar detalhes                  │
│  • Gerenciar permissões                 │
└─────────────────────────────────────────┘
```

---

## 👤 INTERNETBANKING - FLUXO CLIENTE

### 1️⃣ **Login/Registro (Cliente)**
```
┌─────────────────────────────────────────┐
│ Página: /login ou /register             │
├─────────────────────────────────────────┤
│ Login:                                  │
│  • POST /api/auth/login                 │
│  • Email + Senha                        │
│                                         │
│ Registro:                               │
│  • POST /api/auth/register              │
│  • Email, Senha, Nome, CPF, Telefone    │
│                                         │
│ Armazenamento: localStorage.token       │
│ Redirecionamento: /dashboard            │
└─────────────────────────────────────────┘
```

### 2️⃣ **Dashboard (Cliente)**
```
┌─────────────────────────────────────────┐
│ Página: /dashboard                      │
├─────────────────────────────────────────┤
│ Requisições:                            │
│  • GET /api/cliente/saldo               │
│  • GET /api/cliente/transacoes          │
│                                         │
│ Cards Exibidos:                         │
│  • Saldo Disponível (R$)                │
│  • Total de Transações                  │
│  • Status (Ativo)                       │
│                                         │
│ Tabela:                                 │
│  • Últimas 5 transações                 │
│                                         │
│ Botões:                                 │
│  • Ver Saldo Completo → /saldo          │
│  • Enviar PIX → /pix                    │
└─────────────────────────────────────────┘
```

### 3️⃣ **PIX (Cliente)**
```
┌─────────────────────────────────────────┐
│ Página: /pix                            │
├─────────────────────────────────────────┤
│ Abas:                                   │
│                                         │
│ 📥 Receber PIX:                         │
│  • Input: Valor (R$)                    │
│  • POST /api/cliente/cobrancas          │
│  • Exibe: QR Code + Chave PIX           │
│                                         │
│ 📤 Enviar PIX:                          │
│  • Inputs:                              │
│    - Chave PIX (CPF/Email/Tel/Aleatória)│
│    - Valor (R$)                         │
│    - Descrição (opcional)               │
│  • POST /api/cliente/cobrancas          │
│  • Confirmação de sucesso               │
└─────────────────────────────────────────┘
```

### 4️⃣ **Saldo (Cliente)**
```
┌─────────────────────────────────────────┐
│ Página: /saldo                          │
├─────────────────────────────────────────┤
│ Requisições:                            │
│  • GET /api/cliente/saldo               │
│                                         │
│ Exibição:                               │
│  • Saldo Total                          │
│  • Histórico de movimentações           │
│  • Filtros por período                  │
└─────────────────────────────────────────┘
```

### 5️⃣ **Saques (Cliente)**
```
┌─────────────────────────────────────────┐
│ Página: /saques                         │
├─────────────────────────────────────────┤
│ Requisições:                            │
│  • GET /api/cliente/saques              │
│  • POST /api/cliente/saques (novo)      │
│                                         │
│ Funcionalidades:                        │
│  • Solicitar saque                      │
│  • Listar saques pendentes               │
│  • Histórico de saques                  │
└─────────────────────────────────────────┘
```

### 6️⃣ **Perfil (Cliente)**
```
┌─────────────────────────────────────────┐
│ Página: /perfil                         │
├─────────────────────────────────────────┤
│ Requisições:                            │
│  • GET /api/cliente/perfil              │
│  • PUT /api/cliente/perfil (atualizar)  │
│                                         │
│ Dados:                                  │
│  • Nome, Email, Telefone, CPF           │
│  • Endereço, Cidade, Estado             │
│  • Foto de perfil                       │
└─────────────────────────────────────────┘
```

---

## 🔄 FLUXO DE AUTENTICAÇÃO

```
┌──────────────────────────────────────────────────────────┐
│ 1. Usuário faz login                                     │
│    ↓                                                     │
│ 2. Frontend envia POST /api/auth/login                   │
│    ↓                                                     │
│ 3. Backend valida credenciais                           │
│    ↓                                                     │
│ 4. Backend retorna JWT Token                            │
│    ↓                                                     │
│ 5. Frontend armazena em localStorage                    │
│    ↓                                                     │
│ 6. Interceptor adiciona "Authorization: Bearer {token}" │
│    ↓                                                     │
│ 7. Todas as requisições incluem o token                 │
│    ↓                                                     │
│ 8. Se 401 (não autorizado):                             │
│    - Remove token                                       │
│    - Redireciona para /login                            │
└──────────────────────────────────────────────────────────┘
```

---

## 📋 ENDPOINTS UTILIZADOS

### BackOffice (Admin)
- `POST /api/auth/login` - Login
- `GET /api/admin/dashboard` - Dashboard
- `GET /api/admin/transactions` - Transações
- `GET /api/admin/clients` - Clientes

### InternetBanking (Cliente)
- `POST /api/auth/login` - Login
- `POST /api/auth/register` - Registro
- `GET /api/cliente/saldo` - Saldo
- `GET /api/cliente/transacoes` - Transações
- `POST /api/cliente/cobrancas` - PIX (receber/enviar)
- `GET /api/cliente/saques` - Saques
- `POST /api/cliente/saques` - Solicitar saque
- `GET /api/cliente/perfil` - Perfil
- `PUT /api/cliente/perfil` - Atualizar perfil

---

## ⚠️ PROBLEMAS IDENTIFICADOS

### 1. **Endpoints Implementados no Backend** ✅

#### API Interna (5036) - Admin
- ✅ `POST /api/auth/login` - Login
- ✅ `GET /api/admin/dashboard` - Dashboard
- ✅ `GET /api/admin/users` - Listar usuários
- ✅ `GET /api/admin/users/{id}` - Detalhes do usuário
- ✅ `GET /api/admin/transactions` - Listar transações
- ✅ `GET /api/admin/reports/transactions` - Relatório de transações
- ✅ `GET /api/pix/listar` - Listar PIX Dinâmico
- ✅ `POST /api/pix/criar` - Criar PIX Dinâmico
- ✅ `GET /api/pix-webhooks/listar` - Listar webhooks PIX
- ✅ `POST /api/pix-webhooks/registrar` - Registrar webhook PIX
- ✅ `POST /api/transferencias/agendar` - Agendar transferência
- ✅ `GET /api/transferencias/listar` - Listar agendamentos

#### API Cliente (5167) - Cliente
- ✅ `POST /api/auth/login` - Login
- ✅ `POST /api/auth/register` - Registro
- ✅ `GET /api/cliente/saldo` - Saldo
- ✅ `GET /api/cliente/transacoes` - Transações
- ✅ `GET /api/cliente/perfil` - Perfil
- ✅ `PUT /api/cliente/perfil` - Atualizar perfil

### 2. **Endpoints Faltando no Backend** ❌

#### BackOffice (Admin)
- ❌ `GET /api/admin/clientes` - Listar clientes (chamado em /clientes)
- ❌ `GET /api/admin/clientes/{id}` - Detalhes do cliente
- ❌ `POST /api/admin/clientes` - Criar cliente
- ❌ `PUT /api/admin/clientes/{id}` - Atualizar cliente

#### InternetBanking (Cliente)
- ❌ `POST /api/cliente/cobrancas` - Gerar cobrança/QR Code (chamado em /pix)
- ❌ `GET /api/cliente/saques` - Listar saques (chamado em /saques)
- ❌ `POST /api/cliente/saques` - Solicitar saque (chamado em /saques)

---

## 🎯 AÇÕES NECESSÁRIAS

### 1. **BackOffice - Endpoints Faltando**

#### Problema
- Página `/clientes` chama `GET /api/admin/clientes` (não existe)
- Precisa listar clientes do sistema

#### Solução
Implementar no Backend (API Interna):
```csharp
[HttpGet("clientes")]
public async Task<ActionResult> ListClientes([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
{
    // Retornar lista de clientes com paginação
}

[HttpGet("clientes/{id}")]
public async Task<ActionResult> GetCliente(Guid id)
{
    // Retornar detalhes do cliente
}

[HttpPost("clientes")]
public async Task<ActionResult> CreateCliente([FromBody] CreateClienteDto dto)
{
    // Criar novo cliente
}

[HttpPut("clientes/{id}")]
public async Task<ActionResult> UpdateCliente(Guid id, [FromBody] UpdateClienteDto dto)
{
    // Atualizar cliente
}
```

### 2. **InternetBanking - Endpoints Faltando**

#### Problema
- Página `/pix` chama `POST /api/cliente/cobrancas` (não existe)
- Página `/saques` chama `GET/POST /api/cliente/saques` (não existe)

#### Solução
Implementar no Backend (API Cliente):
```csharp
[HttpPost("cobrancas")]
public async Task<ActionResult> CreateCobranca([FromBody] CreateCobrancaDto dto)
{
    // Gerar QR Code PIX Dinâmico
    // Chamar PixService
}

[HttpGet("saques")]
public async Task<ActionResult> ListSaques()
{
    // Listar saques do cliente
}

[HttpPost("saques")]
public async Task<ActionResult> CreateSaque([FromBody] CreateSaqueDto dto)
{
    // Solicitar novo saque
}
```

### 3. **Mapeamento de Rotas**

| Frontend | Página | Endpoint Chamado | Status | Ação |
|----------|--------|------------------|--------|------|
| BackOffice | /clientes | GET /api/admin/clientes | ❌ | Implementar |
| BackOffice | /clientes | POST /api/admin/clientes | ❌ | Implementar |
| InternetBanking | /pix | POST /api/cliente/cobrancas | ❌ | Implementar |
| InternetBanking | /saques | GET /api/cliente/saques | ❌ | Implementar |
| InternetBanking | /saques | POST /api/cliente/saques | ❌ | Implementar |

### 4. **Prioridade de Implementação**

1. **Alta Prioridade** (Funcionalidades Críticas)
   - `POST /api/cliente/cobrancas` - PIX é funcionalidade principal
   - `GET /api/admin/clientes` - Gerenciamento de clientes

2. **Média Prioridade** (Funcionalidades Importantes)
   - `GET /api/cliente/saques` - Histórico de saques
   - `POST /api/cliente/saques` - Solicitar saque

3. **Baixa Prioridade** (Funcionalidades Complementares)
   - `PUT /api/admin/clientes/{id}` - Editar cliente
   - `POST /api/admin/clientes` - Criar cliente

