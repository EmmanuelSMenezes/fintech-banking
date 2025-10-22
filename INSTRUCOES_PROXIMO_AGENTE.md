# 📖 INSTRUÇÕES PARA O PRÓXIMO AGENTE

**Fase Atual**: 2 (PIX Dinâmico) ✅ COMPLETA  
**Próxima Fase**: 3 (Webhooks PIX) 🚀

---

## 🎯 OBJETIVO

Implementar **Webhooks para PIX** seguindo o padrão estabelecido na Fase 2.

---

## 📚 LEITURA OBRIGATÓRIA (5 min)

1. **RESUMO_FINAL_FASE_2.md** - Entender o que foi feito
2. **PROXIMOS_PASSOS_FASE_3.md** - Roadmap detalhado
3. **VERSAO_ATUALIZADA_FASE_2.md** - Status técnico

---

## 🔍 ANÁLISE DE PADRÕES (10 min)

### Arquivos de Referência
```
Backend/src/FinTechBanking.Core/Entities/PixDinamico.cs
Backend/src/FinTechBanking.Core/Interfaces/IPixService.cs
Backend/src/FinTechBanking.Data/Repositories/PixRepository.cs
Backend/src/FinTechBanking.Services/Pix/PixService.cs
Backend/src/FinTechBanking.API.Interna/Controllers/PixController.cs
Backend/FinTechBanking.Tests/ApiIntegrationTests.cs
```

### Padrão a Seguir
1. **Entity** → `Core/Entities/PixWebhook.cs`
2. **Interfaces** → `Core/Interfaces/IPixWebhookService.cs`
3. **DTOs** → `Core/DTOs/PixWebhookDtos.cs`
4. **Repository** → `Data/Repositories/PixWebhookRepository.cs`
5. **Service** → `Services/Pix/PixWebhookService.cs`
6. **Controller** → `API.Interna/Controllers/PixWebhookController.cs`
7. **Tests** → `Tests/ApiIntegrationTests.cs` (adicionar)

---

## ✅ VALIDAÇÃO DO AMBIENTE (1 min)

```bash
# Clonar repositório
git clone https://github.com/EmmanuelSMenezes/fintech-banking.git
cd fintech-banking

# Validar build
cd Backend
dotnet build

# Rodar testes
dotnet test

# Esperado: 68/68 testes passando ✅
```

---

## 🚀 COMEÇAR IMPLEMENTAÇÃO

### Passo 1: Criar Branch
```bash
git checkout -b feature/pix-webhooks
```

### Passo 2: Criar Estrutura
```bash
# Criar arquivos vazios
touch Backend/src/FinTechBanking.Core/Entities/PixWebhook.cs
touch Backend/src/FinTechBanking.Core/Interfaces/IPixWebhookService.cs
touch Backend/src/FinTechBanking.Core/DTOs/PixWebhookDtos.cs
touch Backend/src/FinTechBanking.Data/Repositories/PixWebhookRepository.cs
touch Backend/src/FinTechBanking.Services/Pix/PixWebhookService.cs
touch Backend/src/FinTechBanking.API.Interna/Controllers/PixWebhookController.cs
```

### Passo 3: Implementar Seguindo Padrão
- Copiar estrutura do PixDinamico
- Adaptar para Webhooks
- Adicionar retry logic
- Implementar validações

### Passo 4: Adicionar Testes
- Mínimo 6 testes de integração
- Testar sucesso e falhas
- Testar autenticação

### Passo 5: Validar
```bash
dotnet build
dotnet test
# Esperado: 74/74 testes passando (68 + 6 novos)
```

### Passo 6: Commit e Push
```bash
git add .
git commit -m "feat: Implementar Webhooks para PIX - Fase 3"
git push origin feature/pix-webhooks
```

---

## 📋 CHECKLIST DE IMPLEMENTAÇÃO

### Entity (PixWebhook)
- [ ] Id (UUID)
- [ ] UserId (FK)
- [ ] EventType (string)
- [ ] WebhookUrl (string)
- [ ] IsActive (bool)
- [ ] RetryCount (int)
- [ ] LastAttempt (DateTime?)
- [ ] CreatedAt (DateTime)
- [ ] UpdatedAt (DateTime)

### Endpoints
- [ ] POST `/api/pix-webhooks/registrar` - Registrar webhook
- [ ] GET `/api/pix-webhooks/listar` - Listar webhooks
- [ ] DELETE `/api/pix-webhooks/{id}` - Deletar webhook
- [ ] POST `/api/pix-webhooks/{id}/testar` - Testar webhook

### Eventos
- [ ] `pix.dinamico.criado`
- [ ] `pix.dinamico.pago`
- [ ] `pix.dinamico.expirado`
- [ ] `pix.dinamico.cancelado`

### Testes
- [ ] Registrar webhook com dados válidos
- [ ] Registrar webhook sem URL
- [ ] Listar webhooks do usuário
- [ ] Deletar webhook
- [ ] Testar webhook
- [ ] Validar autenticação

---

## 🔗 RECURSOS ÚTEIS

### Documentação
- `QUICK_START_PROXIMO_AGENTE.md` - Quick start
- `CONTEXT_BASE_PARA_PROXIMO_AGENTE.md` - Contexto técnico
- `ARQUITETURA_TECNICA.md` - Arquitetura

### Código
- Repositório: https://github.com/EmmanuelSMenezes/fintech-banking
- Branch: `main` (base)
- Criar: `feature/pix-webhooks`

---

## ⏱️ ESTIMATIVA

- **Leitura**: 15 min
- **Análise**: 10 min
- **Implementação**: 2-3 dias
- **Testes**: 1 dia
- **Total**: 2-3 dias

---

## 🎯 SUCESSO

Quando terminar:
- [ ] 74/74 testes passando
- [ ] 0 erros de build
- [ ] Documentação atualizada
- [ ] Commit realizado
- [ ] Push concluído

---

**Boa sorte! 🚀**

