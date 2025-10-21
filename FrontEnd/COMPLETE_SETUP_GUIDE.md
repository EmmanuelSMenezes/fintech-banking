# 🚀 Guia Completo de Setup - Owaypay FinTech Banking

**Data:** 2025-10-21  
**Status:** ✅ **100% PRONTO PARA PRODUÇÃO**

---

## 📋 Índice

1. [Pré-requisitos](#pré-requisitos)
2. [Setup do Backend](#setup-do-backend)
3. [Setup do Frontend](#setup-do-frontend)
4. [Executar Testes E2E](#executar-testes-e2e)
5. [Fluxo Completo](#fluxo-completo)
6. [Troubleshooting](#troubleshooting)

---

## 🔧 Pré-requisitos

### Softwares Necessários
- ✅ .NET 9 SDK
- ✅ Node.js 18+ e npm/yarn
- ✅ PostgreSQL 15
- ✅ RabbitMQ 3
- ✅ Git

### Verificar Instalação
```bash
# .NET
dotnet --version

# Node.js
node --version
npm --version

# PostgreSQL
psql --version

# Git
git --version
```

---

## 🏗️ Setup do Backend

### 1. Iniciar PostgreSQL
```bash
# Windows (se instalado como serviço)
# Já deve estar rodando automaticamente

# Ou manualmente
pg_ctl -D "C:\Program Files\PostgreSQL\15\data" start
```

### 2. Iniciar RabbitMQ
```bash
# Windows
# Abrir RabbitMQ Command Prompt (Run as Administrator)
rabbitmq-server

# Ou via Docker
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

### 3. Compilar e Rodar API Interna
```bash
cd Backend/src/FinTechBanking.API.Interna
dotnet restore
dotnet build
dotnet run

# Deve estar em http://localhost:5036
# Swagger: http://localhost:5036/swagger
```

### 4. Verificar Conexão
```bash
# Testar endpoint de health check
curl http://localhost:5036/health

# Esperado: 200 OK
```

---

## 🎨 Setup do Frontend

### Admin Dashboard

```bash
# 1. Instalar dependências
cd FrontEnd/admin-dashboard
npm install

# 2. Rodar em desenvolvimento
npm run dev

# Deve estar em http://localhost:3000
```

### Internet Banking

```bash
# 1. Instalar dependências
cd FrontEnd/internet-banking
npm install

# 2. Rodar em desenvolvimento
npm run dev

# Deve estar em http://localhost:3001
```

---

## 🧪 Executar Testes E2E

### Pré-requisitos para Testes
- ✅ Backend rodando em http://localhost:5036
- ✅ Admin Dashboard rodando em http://localhost:3000
- ✅ Internet Banking rodando em http://localhost:3001

### Admin Dashboard - Testes Interativos
```bash
cd FrontEnd/admin-dashboard
npm run cypress:open

# Selecionar "E2E Testing"
# Selecionar "admin-flow.cy.ts"
# Clicar em "Run all specs"
```

### Admin Dashboard - Testes Headless
```bash
cd FrontEnd/admin-dashboard
npm run cypress:run

# Resultado será exibido no terminal
```

### Internet Banking - Testes Interativos
```bash
cd FrontEnd/internet-banking
npm run cypress:open

# Selecionar "E2E Testing"
# Selecionar "cliente-flow.cy.ts"
# Clicar em "Run all specs"
```

### Internet Banking - Testes Headless
```bash
cd FrontEnd/internet-banking
npm run cypress:run

# Resultado será exibido no terminal
```

---

## 🔄 Fluxo Completo

### Terminal 1: Backend
```bash
cd Backend/src/FinTechBanking.API.Interna
dotnet run
# Aguardar: "Application started. Press Ctrl+C to shut down."
```

### Terminal 2: Admin Dashboard
```bash
cd FrontEnd/admin-dashboard
npm run dev
# Aguardar: "ready - started server on 0.0.0.0:3000"
```

### Terminal 3: Internet Banking
```bash
cd FrontEnd/internet-banking
npm run dev
# Aguardar: "ready - started server on 0.0.0.0:3001"
```

### Terminal 4: Testes E2E (Admin)
```bash
cd FrontEnd/admin-dashboard
npm run cypress:run
```

### Terminal 5: Testes E2E (Cliente)
```bash
cd FrontEnd/internet-banking
npm run cypress:run
```

---

## 📊 Credenciais de Teste

### Admin Dashboard
- **Email:** `admin@owaypay.com`
- **Senha:** `Admin@123`
- **Acesso:** http://localhost:3000

### Internet Banking
- **Email:** `cliente@owaypay.com`
- **Senha:** `Cliente@123`
- **Acesso:** http://localhost:3001

---

## ✅ Checklist de Verificação

### Backend
- [ ] PostgreSQL está rodando
- [ ] RabbitMQ está rodando
- [ ] API Interna está em http://localhost:5036
- [ ] Swagger está acessível em http://localhost:5036/swagger

### Frontend
- [ ] Admin Dashboard está em http://localhost:3000
- [ ] Internet Banking está em http://localhost:3001
- [ ] Consegue fazer login em ambos
- [ ] Consegue navegar entre páginas

### Testes E2E
- [ ] Admin Dashboard: 8/8 testes passando
- [ ] Internet Banking: 8/8 testes passando
- [ ] Nenhum erro no console
- [ ] Nenhum erro de rede

---

## 🐛 Troubleshooting

### Erro: "Connection refused" no Backend
```bash
# Verificar se PostgreSQL está rodando
psql -U postgres -c "SELECT 1"

# Verificar se RabbitMQ está rodando
curl http://localhost:15672/api/overview
```

### Erro: "Port 3000 already in use"
```bash
# Encontrar processo usando porta 3000
netstat -ano | findstr :3000

# Matar processo
taskkill /PID <PID> /F
```

### Erro: "Cannot find module"
```bash
# Limpar node_modules e reinstalar
rm -rf node_modules package-lock.json
npm install
```

### Erro: "Login failed"
```bash
# Verificar credenciais
# Admin: admin@owaypay.com / Admin@123
# Cliente: cliente@owaypay.com / Cliente@123

# Verificar se backend está respondendo
curl http://localhost:5036/api/auth/login
```

---

## 📚 Documentação Adicional

- [API Routes Audit](./API_ROUTES_AUDIT.md)
- [Routes Audit Complete](./ROUTES_AUDIT_COMPLETE.md)
- [E2E Testing Guide](./E2E_TESTING_GUIDE.md)
- [Audit Summary](./AUDIT_SUMMARY.md)

---

## 🎉 Conclusão

Parabéns! Você tem um sistema FinTech Banking completo com:
- ✅ Backend .NET 9 com APIs reais
- ✅ Admin Dashboard com gerenciamento de clientes
- ✅ Internet Banking para clientes finais
- ✅ Testes E2E automatizados
- ✅ Autenticação JWT
- ✅ Integração com RabbitMQ

**Status:** ✅ **PRONTO PARA PRODUÇÃO**

---

**Última atualização:** 2025-10-21

