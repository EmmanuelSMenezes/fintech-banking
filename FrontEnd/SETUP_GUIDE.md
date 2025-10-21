# 🎯 FinTech Banking - Frontend Setup Guide

## 📁 Estrutura de Frontends

```
FrontEnd/
├── admin-dashboard/          # 🏢 Painel Administrativo
│   ├── src/
│   ├── public/
│   ├── .env.local.example
│   ├── package.json
│   └── README_FINTECH.md
│
├── internet-banking/         # 💻 Internet Banking (Cliente)
│   ├── src/
│   ├── public/
│   ├── .env.local.example
│   ├── package.json
│   └── README_FINTECH.md
│
└── SETUP_GUIDE.md           # Este arquivo
```

## 🚀 Instalação Rápida

### 1. Admin Dashboard

```bash
cd FrontEnd/admin-dashboard

# Copiar variáveis de ambiente
cp .env.local.example .env.local

# Instalar dependências
npm install

# Rodar em desenvolvimento
npm run dev
# Acesso: http://localhost:3000
```

### 2. Internet Banking

```bash
cd FrontEnd/internet-banking

# Copiar variáveis de ambiente
cp .env.local.example .env.local

# Instalar dependências
npm install

# Rodar em desenvolvimento (em outro terminal)
npm run dev
# Acesso: http://localhost:3001
```

## 🔧 Configuração de Ambiente

### Admin Dashboard (.env.local)

```env
NEXT_PUBLIC_API_URL=http://localhost:5036
NEXT_PUBLIC_API_INTERNAL=http://localhost:5036
NEXT_PUBLIC_APP_NAME=FinTech Admin Dashboard
```

### Internet Banking (.env.local)

```env
NEXT_PUBLIC_API_URL=http://localhost:5036
NEXT_PUBLIC_API_INTERNAL=http://localhost:5036
NEXT_PUBLIC_APP_NAME=FinTech Internet Banking
```

## 📊 Portas

| Serviço | Porta | URL |
|---------|-------|-----|
| Admin Dashboard | 3000 | http://localhost:3000 |
| Internet Banking | 3001 | http://localhost:3001 |
| API Principal | 5064 | http://localhost:5064 |
| API Cliente | 5167 | http://localhost:5167 |
| API Interna | 5036 | http://localhost:5036 |
| PostgreSQL | 5432 | localhost:5432 |
| RabbitMQ | 5672 | localhost:5672 |

## 🔐 Autenticação

Ambos os frontends usam **JWT** com contexto de autenticação.

### Admin Dashboard

```
Email: admin@fintech.com
Senha: Admin123!
```

### Internet Banking

```
Email: cliente@fintech.com
Senha: Cliente123!
```

## 📡 APIs Utilizadas

### Admin Dashboard → API Interna (5036)

- `POST /api/auth/login` - Login
- `GET /api/admin/clientes` - Listar clientes
- `GET /api/admin/usuarios` - Listar usuários
- `GET /api/admin/transacoes` - Monitorar transações
- `GET /api/admin/webhooks/logs` - Logs de webhooks
- `POST /api/admin/liberacoes` - Executar liberações

### Internet Banking → API Interna (5036)

- `POST /api/auth/login` - Login
- `GET /api/cliente/saldo` - Obter saldo
- `GET /api/cliente/extrato` - Extrato
- `GET /api/cliente/transacoes` - Transações
- `POST /api/cliente/cobrancas` - Gerar cobrança
- `POST /api/cliente/saques` - Solicitar saque

## 🎨 Tema e Componentes

Ambos usam **Material-UI (MUI)** com tema customizado:

- `src/theme/palette.ts` - Cores
- `src/theme/typography.ts` - Tipografia
- `src/theme/overrides/` - Customizações

## 📦 Dependências Principais

- **Next.js 14** - Framework React
- **Material-UI** - Componentes UI
- **TypeScript** - Type safety
- **Redux** - State management
- **Axios** - HTTP client
- **React Hook Form** - Formulários

## 🧪 Desenvolvimento

### Estrutura de Pastas

```
src/
├── @types/           # Type definitions
├── auth/             # Autenticação JWT
├── components/       # Componentes reutilizáveis
├── layouts/          # Layouts
├── pages/            # Páginas
├── sections/         # Seções específicas
├── theme/            # Tema MUI
└── utils/            # Utilitários
```

### Criar Nova Página

```typescript
// src/pages/nova-pagina.tsx
import { Helmet } from 'react-helmet-async';
import { Container } from '@mui/material';

export default function NovaPagina() {
  return (
    <>
      <Helmet>
        <title>Nova Página</title>
      </Helmet>
      <Container>
        {/* Conteúdo */}
      </Container>
    </>
  );
}
```

## 🚀 Build e Deploy

### Build

```bash
npm run build
```

### Produção

```bash
npm start
```

## 🐛 Troubleshooting

### Erro: "Cannot find module"

```bash
rm -rf node_modules package-lock.json
npm install
```

### Erro: "Port already in use"

```bash
# Matar processo na porta 3000
lsof -ti:3000 | xargs kill -9

# Matar processo na porta 3001
lsof -ti:3001 | xargs kill -9
```

### Erro: "API connection refused"

Verifique se as APIs estão rodando:
```bash
curl http://localhost:5036/swagger
```

## 📚 Documentação

- [Admin Dashboard README](./admin-dashboard/README_FINTECH.md)
- [Internet Banking README](./internet-banking/README_FINTECH.md)
- [Next.js Docs](https://nextjs.org/docs)
- [Material-UI Docs](https://mui.com/)

## 🔗 Links Úteis

- **Admin Dashboard:** http://localhost:3000
- **Internet Banking:** http://localhost:3001
- **API Swagger:** http://localhost:5036/swagger
- **GitHub:** https://github.com/EmmanuelSMenezes/fintech-banking

---

**Status:** ✅ Pronto para desenvolvimento  
**Última atualização:** 2025-10-21

