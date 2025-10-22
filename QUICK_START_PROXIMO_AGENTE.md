# 🚀 QUICK START - PRÓXIMO AGENTE

## ⏱️ 5 MINUTOS PARA COMEÇAR

### 1️⃣ Ler Documentação (2 min)
```bash
# Leia nesta ordem:
1. RESUMO_EXECUTIVO_FASE_1_5.md (visão geral)
2. CONTEXT_BASE_PARA_PROXIMO_AGENTE.md (contexto)
3. TAREFAS_PROXIMAS_FASES.md (próximas tarefas)
```

### 2️⃣ Validar Ambiente (2 min)
```bash
# Verificar Docker
docker-compose ps

# Se não estiver rodando:
cd Backend
docker-compose up -d --build

# Aguardar ~30 segundos
```

### 3️⃣ Rodar Testes (1 min)
```bash
cd Backend/FinTechBanking.Tests
dotnet test -v normal

# Esperado: 62/62 testes passando ✅
```

---

## 📋 CHECKLIST RÁPIDO

- [ ] Leu RESUMO_EXECUTIVO_FASE_1_5.md
- [ ] Leu CONTEXT_BASE_PARA_PROXIMO_AGENTE.md
- [ ] Docker containers rodando
- [ ] 62/62 testes passando
- [ ] Escolheu próxima feature

---

## 🎯 PRÓXIMA FEATURE - RECOMENDAÇÃO

### Opção 1: PIX Dinâmico (⭐ Recomendado)
**Por quê?**
- Integração com Banking Hub já existe
- RabbitMQ já está configurado
- Padrão similar a Transferências

**Tempo estimado**: 4-6 horas

**Passos**:
1. Criar entidade `PixDinamico`
2. Criar `IPixService` e `PixService`
3. Criar `PixController`
4. Adicionar 5+ testes
5. Fazer commit

### Opção 2: Empréstimos
**Por quê?**
- Feature independente
- Lógica de negócio clara
- Bom para aprender o padrão

**Tempo estimado**: 5-7 horas

### Opção 3: Investimentos
**Por quê?**
- Similar a Empréstimos
- Integração com Banking Hub
- Cálculos de rentabilidade

**Tempo estimado**: 5-7 horas

---

## 🔑 CREDENCIAIS PADRÃO

```
Email: admin@owaypay.com
Senha: Admin@123
```

**Para popular dados**:
```bash
curl -X POST http://localhost:5036/api/auth/seed
```

---

## 🌐 ENDPOINTS PRINCIPAIS

### Autenticação
```
POST   /api/auth/login
POST   /api/auth/logout
POST   /api/auth/seed
```

### Admin
```
GET    /api/admin/dashboard
GET    /api/admin/users
GET    /api/admin/transactions
```

### Auditoria
```
GET    /api/audit/my-logs
GET    /api/audit/search
GET    /api/audit/stats
```

### Webhooks
```
POST   /api/webhooks/register
DELETE /api/webhooks/unregister
GET    /api/webhooks/history
```

### Transferências
```
POST   /api/transferencias/transferir
GET    /api/transferencias/historico
```

### Relatórios
```
GET    /api/relatorios/transacoes-excel
GET    /api/relatorios/extrato-pdf
GET    /api/relatorios/resumo
```

---

## 📊 ESTRUTURA DO PROJETO

```
Backend/
├── src/
│   ├── FinTechBanking.API.Interna/
│   │   ├── Controllers/
│   │   ├── Middleware/
│   │   └── Attributes/
│   ├── FinTechBanking.Core/
│   │   ├── Entities/
│   │   └── Interfaces/
│   ├── FinTechBanking.Data/
│   │   └── Repositories/
│   └── FinTechBanking.Services/
│       ├── Auditing/
│       ├── Webhooks/
│       └── RateLimiting/
└── FinTechBanking.Tests/
    └── ApiIntegrationTests.cs
```

---

## 🛠️ COMANDOS ÚTEIS

```bash
# Build
cd Backend && dotnet build

# Testes
cd Backend/FinTechBanking.Tests && dotnet test

# Docker
docker-compose up -d --build
docker-compose down
docker-compose logs -f api_interna

# Git
git status
git add .
git commit -m "feat: descrição"
git push
```

---

## ⚠️ COISAS IMPORTANTES

1. **Sempre rodar testes antes de commit**
   ```bash
   dotnet test
   ```

2. **Manter padrão de resposta**
   ```json
   {
     "message": "Sucesso",
     "data": { /* dados */ },
     "accessToken": "token"
   }
   ```

3. **Adicionar auditoria automaticamente**
   - Middleware já captura tudo
   - Não precisa fazer nada extra

4. **Rate limiting já está ativo**
   - Use `[RateLimit]` em endpoints críticos

5. **Testes devem usar admin@owaypay.com**
   - Não criar novos usuários nos testes

---

## 🚨 SE ALGO DER ERRADO

1. **Testes falhando?**
   ```bash
   docker-compose restart
   dotnet test
   ```

2. **Erro de conexão?**
   ```bash
   docker-compose logs postgres
   docker-compose restart postgres
   ```

3. **Porta em uso?**
   ```bash
   netstat -ano | findstr :5036
   taskkill /PID <PID> /F
   ```

4. **Mais ajuda?**
   - Leia `TROUBLESHOOTING_E_DICAS.md`

---

## 📞 PRÓXIMOS PASSOS

1. ✅ Validar ambiente (5 min)
2. ✅ Rodar testes (1 min)
3. ✅ Escolher feature (5 min)
4. ✅ Implementar (4-6 horas)
5. ✅ Testar (30 min)
6. ✅ Commit (5 min)

---

**Boa sorte! 🚀**

Qualquer dúvida, consulte os documentos de contexto.

