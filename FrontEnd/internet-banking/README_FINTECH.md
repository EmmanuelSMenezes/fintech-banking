# 💻 FinTech Banking - Internet Banking

**Painel voltado ao Cliente (Usuário Final)**

## 📋 Responsabilidades

- ✅ Cadastrar usuários por cliente
- ✅ Controle de acesso e permissões de cliente
- ✅ Exibir saldos, extratos e transações
- ✅ Permitir geração de cobranças e solicitações de saque
- ✅ Comunicar-se via API Internal (porta 5036)

## 🚀 Quick Start

### Instalação

```bash
cd FrontEnd/internet-banking
npm install
# ou
yarn install
```

### Desenvolvimento

```bash
npm run dev
# Acesso: http://localhost:3001
```

### Build

```bash
npm run build
npm start
```

## 🔧 Configuração

### Variáveis de Ambiente

Crie um arquivo `.env.local`:

```env
# API Configuration
NEXT_PUBLIC_API_URL=http://localhost:5036
NEXT_PUBLIC_API_INTERNAL=http://localhost:5036

# Auth
NEXT_PUBLIC_AUTH_PROVIDER=jwt

# App
NEXT_PUBLIC_APP_NAME=FinTech Internet Banking
NEXT_PUBLIC_APP_VERSION=1.0.0
```

### Estrutura de Pastas

```
src/
├── @types/           # Type definitions
├── auth/             # Autenticação JWT
├── components/       # Componentes reutilizáveis
├── layouts/          # Layouts (dashboard, login, etc)
├── pages/            # Páginas da aplicação
├── sections/         # Seções específicas do banking
├── theme/            # Tema Material-UI
└── utils/            # Utilitários
```

## 🔐 Autenticação

O painel usa JWT com contexto de autenticação. Configurado em:
- `src/auth/JwtContext.tsx`
- `src/auth/useAuthContext.ts`

### Login

```typescript
import { useAuthContext } from '@/auth';

const { login } = useAuthContext();

await login(email, password);
```

## 📡 API Integration

### Endpoints Utilizados

**API Internal (5036):**
- `POST /api/auth/login` - Login cliente
- `GET /api/cliente/saldo` - Obter saldo
- `GET /api/cliente/extrato` - Extrato de transações
- `GET /api/cliente/transacoes` - Histórico de transações
- `POST /api/cliente/cobrancas` - Gerar cobrança
- `POST /api/cliente/saques` - Solicitar saque

### Exemplo de Requisição

```typescript
import axios from '@/utils/axios';

const fetchSaldo = async () => {
  const response = await axios.get('/api/cliente/saldo');
  return response.data;
};
```

## 🎨 Tema

Material-UI com tema customizado:
- `src/theme/palette.ts` - Cores
- `src/theme/typography.ts` - Tipografia
- `src/theme/overrides/` - Overrides de componentes

## 📦 Dependências Principais

- **Next.js 14** - Framework React
- **Material-UI (MUI)** - Componentes UI
- **TypeScript** - Type safety
- **Redux** - State management
- **Axios** - HTTP client
- **React Hook Form** - Formulários

## 🧪 Testes

```bash
npm run test
npm run test:watch
```

## 📚 Documentação

- [Next.js Docs](https://nextjs.org/docs)
- [Material-UI Docs](https://mui.com/material-ui/getting-started/)
- [Redux Docs](https://redux.js.org/)

## 🔗 Links Úteis

- **API Internal:** http://localhost:5036
- **Swagger:** http://localhost:5036/swagger
- **Frontend:** http://localhost:3001

## 👥 Credenciais de Teste

```
Email: cliente@fintech.com
Senha: Cliente123!
```

## 📝 Notas

- Painel para clientes finais
- Requer autenticação JWT
- Acesso via API Internal
- Visualização de dados financeiros pessoais

## 🚨 Troubleshooting

### Erro de conexão com API

Verifique se a API Internal está rodando na porta 5036:
```bash
curl http://localhost:5036/swagger
```

### Erro de autenticação

Limpe o localStorage e faça login novamente:
```javascript
localStorage.clear();
```

---

**Status:** ✅ Pronto para desenvolvimento  
**Última atualização:** 2025-10-21

