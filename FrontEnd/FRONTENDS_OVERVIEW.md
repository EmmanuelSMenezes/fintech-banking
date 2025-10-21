# 🎯 FinTech Banking - Frontends Overview

## 📊 Comparação dos Frontends

| Aspecto | Admin Dashboard | Internet Banking |
|---------|-----------------|------------------|
| **Usuário** | Equipe Interna | Cliente Final |
| **Porta** | 3000 | 3001 |
| **Framework** | Next.js 14 + TypeScript | Next.js 14 + TypeScript |
| **UI Library** | Material-UI (MUI) | Material-UI (MUI) |
| **API** | Internal (5036) | Internal (5036) |
| **Autenticação** | JWT (Admin) | JWT (Cliente) |

---

## 🏢 Admin Dashboard

### 📍 Localização
```
FrontEnd/admin-dashboard/
```

### 🎯 Objetivo
Painel administrativo para gerenciamento interno da plataforma FinTech.

### 👥 Usuários
- Administradores
- Gerentes de Operações
- Suporte Técnico

### 📋 Funcionalidades Principais

#### 1. Gestão de Clientes
- ✅ Cadastrar novos clientes
- ✅ Editar informações de clientes
- ✅ Visualizar histórico de clientes
- ✅ Ativar/Desativar clientes

#### 2. Gestão de Usuários
- ✅ Cadastrar usuários por cliente
- ✅ Gerenciar permissões
- ✅ Controlar acesso
- ✅ Resetar senhas

#### 3. Monitoramento
- ✅ Dashboard com métricas
- ✅ Monitorar transações em tempo real
- ✅ Visualizar saldos de clientes
- ✅ Alertas de anomalias

#### 4. Webhooks
- ✅ Consultar logs de webhooks
- ✅ Reenviar webhooks falhados
- ✅ Configurar endpoints

#### 5. Operações Manuais
- ✅ Executar liberações manuais
- ✅ Reverter transações
- ✅ Ajustar saldos (quando necessário)

### 🔐 Permissões
- Acesso restrito a administradores
- Controle de acesso baseado em roles
- Auditoria de todas as ações

### 📡 Endpoints Utilizados
```
POST   /api/auth/login
GET    /api/admin/clientes
POST   /api/admin/clientes
PUT    /api/admin/clientes/{id}
GET    /api/admin/usuarios
POST   /api/admin/usuarios
GET    /api/admin/transacoes
GET    /api/admin/webhooks/logs
POST   /api/admin/liberacoes
```

---

## 💻 Internet Banking

### 📍 Localização
```
FrontEnd/internet-banking/
```

### 🎯 Objetivo
Plataforma de internet banking para clientes finais gerenciarem suas contas.

### 👥 Usuários
- Clientes da plataforma
- Usuários finais

### 📋 Funcionalidades Principais

#### 1. Autenticação
- ✅ Login seguro com JWT
- ✅ Recuperação de senha
- ✅ Autenticação de dois fatores (2FA)

#### 2. Visualização de Dados
- ✅ Saldo da conta
- ✅ Extrato de transações
- ✅ Histórico de movimentações
- ✅ Detalhes de transações

#### 3. Operações Financeiras
- ✅ Gerar cobranças
- ✅ Solicitar saques
- ✅ Transferências (quando habilitado)
- ✅ Pagamentos

#### 4. Gestão de Conta
- ✅ Atualizar perfil
- ✅ Alterar senha
- ✅ Gerenciar dispositivos
- ✅ Configurações de segurança

#### 5. Suporte
- ✅ Chat com suporte
- ✅ Abrir tickets
- ✅ Consultar FAQ

### 🔐 Permissões
- Acesso apenas aos dados próprios
- Visualização de transações pessoais
- Operações limitadas ao saldo disponível

### 📡 Endpoints Utilizados
```
POST   /api/auth/login
GET    /api/cliente/saldo
GET    /api/cliente/extrato
GET    /api/cliente/transacoes
POST   /api/cliente/cobrancas
POST   /api/cliente/saques
PUT    /api/cliente/perfil
POST   /api/cliente/alterar-senha
```

---

## 🔄 Fluxo de Dados

### Admin Dashboard
```
Admin → Login (JWT) → API Internal (5036) → Database
                   ↓
              Gerenciar Clientes
              Monitorar Transações
              Executar Operações
```

### Internet Banking
```
Cliente → Login (JWT) → API Internal (5036) → Database
                     ↓
                Visualizar Dados
                Solicitar Operações
                Gerenciar Conta
```

---

## 🎨 Tema e Estilo

### Cores
- **Admin Dashboard:** Tons profissionais (azul, cinza)
- **Internet Banking:** Tons amigáveis (verde, azul claro)

### Componentes
Ambos usam Material-UI com customizações específicas:
- Admin: Componentes mais complexos (tabelas, gráficos)
- Banking: Componentes mais simples (cards, formulários)

---

## 📦 Estrutura de Pastas

### Admin Dashboard
```
admin-dashboard/src/
├── pages/
│   ├── auth/
│   ├── dashboard/
│   ├── clientes/
│   ├── usuarios/
│   ├── transacoes/
│   └── webhooks/
├── sections/
│   ├── @dashboard/
│   └── admin/
└── components/
    ├── admin/
    └── common/
```

### Internet Banking
```
internet-banking/src/
├── pages/
│   ├── auth/
│   ├── dashboard/
│   ├── saldo/
│   ├── extrato/
│   ├── cobrancas/
│   └── saques/
├── sections/
│   ├── @dashboard/
│   └── banking/
└── components/
    ├── banking/
    └── common/
```

---

## 🚀 Desenvolvimento

### Adicionar Nova Página no Admin
```bash
cd FrontEnd/admin-dashboard
# Criar arquivo em src/pages/nova-pagina.tsx
npm run dev
```

### Adicionar Nova Página no Banking
```bash
cd FrontEnd/internet-banking
# Criar arquivo em src/pages/nova-pagina.tsx
npm run dev
```

---

## 🔗 Links Úteis

| Recurso | URL |
|---------|-----|
| Admin Dashboard | http://localhost:3000 |
| Internet Banking | http://localhost:3001 |
| API Swagger | http://localhost:5036/swagger |
| GitHub | https://github.com/EmmanuelSMenezes/fintech-banking |

---

## 📝 Notas Importantes

1. **Ambos usam a mesma API Internal (5036)**
   - Diferentes endpoints para diferentes usuários
   - Autenticação separada (admin vs cliente)

2. **Estrutura idêntica**
   - Facilita manutenção
   - Reutilização de componentes comuns
   - Padrão consistente

3. **Desenvolvimento independente**
   - Podem ser desenvolvidos em paralelo
   - Sem dependências entre eles
   - Deploy separado

---

**Status:** ✅ Pronto para desenvolvimento  
**Última atualização:** 2025-10-21

