# 📋 PRÓXIMOS PASSOS - FASE 4

## 🎯 OBJETIVO

Implementar uma das seguintes features para a Fase 4:

---

## 🔄 OPÇÃO 1: TRANSFERÊNCIAS AGENDADAS (Recomendado)

**Tempo estimado**: 2-3 dias  
**Complexidade**: Médio  
**Prioridade**: Alta

### Descrição
Permitir que usuários agendem transferências PIX para serem executadas em data/hora específica.

### Tarefas
1. Criar entidade `ScheduledTransfer` com campos:
   - Id, UserId, AccountId, RecipientKey, Amount, Description
   - ScheduledDate, Status (PENDING, EXECUTED, CANCELLED, FAILED)
   - CreatedAt, UpdatedAt, ExecutedAt

2. Criar interfaces `IScheduledTransferService` e `IScheduledTransferRepository`

3. Criar DTOs para requisição/resposta

4. Implementar repositório com Dapper

5. Implementar serviço com:
   - Validação de data (não pode ser no passado)
   - Agendamento de execução
   - Notificações via webhook

6. Criar controller com endpoints:
   - `POST /api/transferencias/agendar` - Agendar transferência
   - `GET /api/transferencias/agendadas` - Listar agendadas
   - `DELETE /api/transferencias/agendadas/{id}` - Cancelar
   - `GET /api/transferencias/agendadas/{id}` - Obter detalhes

7. Criar migração do banco

8. Adicionar 6+ testes de integração

---

## 📊 OPÇÃO 2: RELATÓRIOS AVANÇADOS

**Tempo estimado**: 3-4 dias  
**Complexidade**: Médio  
**Prioridade**: Média

### Descrição
Criar sistema de relatórios com filtros avançados e múltiplos formatos de exportação.

### Tarefas
1. Criar entidade `Report` com campos:
   - Id, UserId, ReportType, Filters, Format, Status
   - GeneratedAt, ExpiresAt, FileUrl

2. Criar interfaces `IReportService` e `IReportRepository`

3. Implementar serviço com:
   - Geração de relatórios em PDF, CSV, Excel
   - Filtros por período, tipo, valor
   - Cache de relatórios

4. Criar controller com endpoints:
   - `POST /api/relatorios/gerar` - Gerar novo relatório
   - `GET /api/relatorios` - Listar relatórios
   - `GET /api/relatorios/{id}/download` - Download

5. Adicionar 6+ testes

---

## 🏦 OPÇÃO 3: INTEGRAÇÃO COM MAIS BANCOS

**Tempo estimado**: 4-5 dias  
**Complexidade**: Alto  
**Prioridade**: Média

### Descrição
Adicionar suporte a Bradesco, Itaú e Santander além do Sicoob.

### Tarefas
1. Criar abstrações para diferentes bancos
2. Implementar `BradescoService`, `ItauService`, `SantanderService`
3. Criar factory pattern para seleção de banco
4. Adicionar configurações por banco
5. Testes de integração com cada banco

---

## 📚 LEITURA RECOMENDADA

Leia nesta ordem:
1. **RESUMO_FINAL_FASE_3.md** (5 min)
2. **RESUMO_FINAL_FASE_2.md** (5 min)
3. **CONTEXT_BASE_PARA_PROXIMO_AGENTE.md** (10 min)

---

## ✅ CHECKLIST ANTES DE COMEÇAR

- [ ] Ler documentação de fases anteriores
- [ ] Validar ambiente com `dotnet test`
- [ ] Verificar que todos os 74 testes passam
- [ ] Escolher uma das 3 opções
- [ ] Criar branch para a feature
- [ ] Seguir o padrão estabelecido

---

## 🎓 PADRÃO A SEGUIR

Todas as implementações devem seguir:

1. **Clean Architecture**
   - Core (Entities, Interfaces, DTOs)
   - Data (Repositories, Migrations)
   - Services (Business Logic)
   - API (Controllers)

2. **Estrutura de Arquivos**
   ```
   Backend/src/FinTechBanking.Core/
   ├── Entities/
   ├── Interfaces/
   └── DTOs/
   
   Backend/src/FinTechBanking.Data/
   ├── Repositories/
   └── Migrations/
   
   Backend/src/FinTechBanking.Services/
   └── [Feature]/
   
   Backend/src/FinTechBanking.API.Interna/
   └── Controllers/
   ```

3. **Padrões de Código**
   - Async/Await
   - Dependency Injection
   - Exception Handling
   - Logging
   - Rate Limiting
   - JWT Authentication

4. **Testes**
   - Mínimo 6 testes de integração
   - Usar xUnit + FluentAssertions
   - Aceitar 404 se API não estiver rodando

---

## 🚀 COMO COMEÇAR

1. Escolha uma opção (recomendado: Transferências Agendadas)
2. Crie um branch: `git checkout -b feat/fase-4-[feature]`
3. Siga o padrão de implementação
4. Adicione testes
5. Valide com `dotnet test`
6. Faça commit e push

---

## 📞 SUPORTE

Se tiver dúvidas:
- Consulte implementações anteriores (PIX Dinâmico, Webhooks)
- Verifique padrões em arquivos existentes
- Teste incrementalmente

---

**Boa sorte! 🎉**

