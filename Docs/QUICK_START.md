# 🚀 Quick Start - FinTech Banking

## ⚡ Iniciar em 5 Minutos

### Pré-requisitos
- Docker Desktop instalado
- .NET 9 SDK instalado
- Node.js 18+ instalado
- PowerShell 7+

### Passo 1: Iniciar Infraestrutura (1 min)
```bash
cd Backend
docker-compose up -d
```

Verificar:
```bash
docker-compose ps
```

### Passo 2: Iniciar APIs (1 min)

**Terminal 1 - API Principal:**
```bash
cd Backend/src/FinTechBanking.API
dotnet run
# Aguarde: "Now listening on: http://localhost:5064"
```

**Terminal 2 - API Cliente:**
```bash
cd Backend/src/FinTechBanking.API.Cliente
dotnet run
# Aguarde: "Now listening on: http://localhost:5167"
```

**Terminal 3 - API Interna:**
```bash
cd Backend/src/FinTechBanking.API.Interna
dotnet run
# Aguarde: "Now listening on: http://localhost:5036"
```

### Passo 3: Iniciar Frontend (1 min)

**Terminal 4 - Frontend (Vite + React):**
```bash
cd FrontEnd/fintech-frontend
npm install  # Primeira vez apenas
npm run dev
# Aguarde: "VITE v7.x.x ready in X.Xs"
```

---

## 🌐 Acessar Aplicações

| Aplicação | URL | Credenciais |
|-----------|-----|-------------|
| Frontend | http://localhost:5173 | Registre-se |
| API Principal | http://localhost:5064 | N/A |
| API Cliente | http://localhost:5167 | N/A |
| API Interna | http://localhost:5036 | N/A |
| Swagger Principal | http://localhost:5064/swagger | N/A |
| Swagger Cliente | http://localhost:5167/swagger | N/A |
| Swagger Interna | http://localhost:5036/swagger | N/A |
| RabbitMQ | http://localhost:15672 | guest/guest |
| PostgreSQL | localhost:5432 | postgres/postgres |

---

## 🧪 Testar Fluxo Completo

### Opção 1: Frontend (Recomendado)
1. Abra http://localhost:5173
2. Clique em "Registre-se"
3. Preencha os dados
4. Clique em "Registrar"
5. Faça login com as credenciais
6. Verifique console (F12) para logs

### Opção 2: Swagger
1. Acesse http://localhost:5064/swagger
2. Clique em "Authorize"
3. Faça login para obter token
4. Teste endpoints protegidos

### Opção 3: Postman
1. Abrir Postman
2. Importar collections em `Collections_Postman/`
3. Executar requests

### Opção 4: Manual com cURL

**1. Registrar Usuário:**
```bash
curl -X POST http://localhost:5064/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "Senha123!",
    "fullName": "Test User",
    "document": "12345678901",
    "phoneNumber": "11987654321"
  }'
```

**2. Login:**
```bash
curl -X POST http://localhost:5064/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "Senha123!"
  }'
```

**3. Consultar Saldo:**
```bash
curl -X GET http://localhost:5064/api/accounts/balance \
  -H "Authorization: Bearer SEU_TOKEN_AQUI"
```

---

## 🔧 Configurações Importantes

### SMTP (Email)
Editar: `src/FinTechBanking.API.Interna/appsettings.json`

```json
"Email": {
  "SmtpServer": "smtp.gmail.com",
  "SmtpPort": 587,
  "SmtpUsername": "seu-email@gmail.com",
  "SmtpPassword": "sua-senha-app",
  "FromEmail": "seu-email@gmail.com",
  "FromName": "FinTech Banking"
}
```

### JWT Secret
Editar: `appsettings.json` em ambas as APIs

```json
"Jwt": {
  "SecretKey": "sua-chave-secreta-muito-segura-aqui",
  "Issuer": "fintech-banking",
  "Audience": "fintech-banking-api",
  "ExpirationMinutes": 60
}
```

---

## 📊 Verificar Status

### Verificar APIs
```bash
# API Cliente
curl http://localhost:5167/swagger/index.html

# API Interna
curl http://localhost:5036/swagger/index.html
```

### Verificar Banco de Dados
```bash
docker exec fintech_postgres psql -U postgres -d fintech_banking -c "SELECT COUNT(*) FROM users;"
```

### Verificar RabbitMQ
```bash
# Acessar: http://localhost:15672
# Usuário: guest
# Senha: guest
```

---

## 🐛 Troubleshooting

### Erro: "Port already in use"
```bash
# Encontrar processo usando a porta
netstat -ano | findstr :5167

# Matar processo
taskkill /PID <PID> /F
```

### Erro: "Connection refused"
```bash
# Verificar se Docker está rodando
docker ps

# Reiniciar Docker
docker-compose restart
```

### Erro: "npm not found"
```bash
# Instalar Node.js de https://nodejs.org
# Ou usar nvm (Node Version Manager)
```

### Erro: ".NET SDK not found"
```bash
# Instalar .NET 9 de https://dotnet.microsoft.com/download
```

---

## 📚 Documentação Completa

- `DEPLOYMENT_SUMMARY.md` - Resumo completo
- `QA_TEST_REPORT.md` - Relatório de testes
- `CORS_AND_EMAIL_SETUP.md` - Configuração detalhada
- `GETTING_STARTED.md` - Guia de início
- `ARCHITECTURE.md` - Arquitetura do sistema

---

## ✅ Checklist de Inicialização

- [ ] Docker Compose iniciado
- [ ] APIs compiladas
- [ ] API Cliente rodando (5167)
- [ ] API Interna rodando (5036)
- [ ] Consumer Worker rodando
- [ ] Internet Banking rodando (3000)
- [ ] Backoffice rodando (3001)
- [ ] Banco de dados acessível
- [ ] RabbitMQ acessível
- [ ] Testes passando

---

## 🎯 Próximos Passos

1. **Configurar SMTP** - Para envio de emails
2. **Configurar JWT Secret** - Para produção
3. **Executar Testes** - Validar fluxos
4. **Integrar com Banco** - Sicoob ou outro
5. **Deploy** - Em staging/produção

---

**Pronto para começar! 🚀**

