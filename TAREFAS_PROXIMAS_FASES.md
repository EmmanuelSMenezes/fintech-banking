# 📋 TAREFAS PARA PRÓXIMAS FASES

## 🎯 PRIORIDADE ALTA

### Fase 2 - PIX Dinâmico (Recomendado)
- [ ] Criar entidade `PixDinamico`
- [ ] Implementar `IPixService` e `PixService`
- [ ] Criar `PixController` com endpoints:
  - `POST /api/pix/criar-dinamico` - Criar QR code dinâmico
  - `GET /api/pix/status/{id}` - Verificar status
  - `POST /api/pix/confirmar/{id}` - Confirmar pagamento
- [ ] Integrar com Banking Hub
- [ ] Publicar eventos RabbitMQ
- [ ] Adicionar 5+ testes de integração

### Fase 2 - Empréstimos
- [ ] Criar entidade `Emprestimo`
- [ ] Implementar `IEmprestimoService`
- [ ] Criar `EmprestimoController` com endpoints:
  - `POST /api/emprestimos/solicitar` - Solicitar empréstimo
  - `GET /api/emprestimos/minhas-solicitacoes` - Listar solicitações
  - `POST /api/emprestimos/{id}/aprovar` - Aprovar (admin)
  - `POST /api/emprestimos/{id}/rejeitar` - Rejeitar (admin)
- [ ] Implementar cálculo de juros
- [ ] Adicionar 5+ testes

## 🎯 PRIORIDADE MÉDIA

### Fase 3 - Segurança Avançada
- [ ] Implementar Webhook Signatures (HMAC-SHA256)
- [ ] Adicionar Idempotency Keys
- [ ] Encryption de dados sensíveis (CPF, etc)
- [ ] 2FA (Two-Factor Authentication)
- [ ] Adicionar 4+ testes de segurança

### Fase 3 - Investimentos
- [ ] Criar entidade `Investimento`
- [ ] Implementar `IInvestimentoService`
- [ ] Criar `InvestimentoController`:
  - `POST /api/investimentos/aplicar` - Aplicar investimento
  - `GET /api/investimentos/meus-investimentos` - Listar
  - `GET /api/investimentos/rentabilidade` - Rentabilidade
- [ ] Integrar com Banking Hub
- [ ] Adicionar 4+ testes

## 🎯 PRIORIDADE BAIXA

### Fase 4 - Performance
- [ ] Implementar Redis para rate limiting distribuído
- [ ] Cache de relatórios (Redis)
- [ ] Otimização de queries (índices)
- [ ] Implementar paginação em endpoints
- [ ] Adicionar 3+ testes de performance

### Fase 4 - Cartão de Crédito
- [ ] Criar entidade `CartaoCredito`
- [ ] Implementar `ICartaoService`
- [ ] Criar `CartaoController`:
  - `POST /api/cartao/solicitar` - Solicitar cartão
  - `GET /api/cartao/meus-cartoes` - Listar cartões
  - `POST /api/cartao/{id}/ativar` - Ativar cartão
  - `POST /api/cartao/{id}/bloquear` - Bloquear cartão
- [ ] Integrar com Banking Hub
- [ ] Adicionar 4+ testes

## 📊 CHECKLIST DE QUALIDADE

Para cada nova feature:
- [ ] Criar entidade/modelo
- [ ] Criar interface de serviço
- [ ] Implementar serviço
- [ ] Criar controller com endpoints
- [ ] Adicionar validações
- [ ] Integrar com RabbitMQ (se necessário)
- [ ] Adicionar auditoria (middleware)
- [ ] Adicionar rate limiting
- [ ] Escrever 5+ testes de integração
- [ ] Documentar endpoints
- [ ] Fazer commit com mensagem clara

## 🔄 WORKFLOW RECOMENDADO

1. **Planejamento**: Ler este arquivo
2. **Validação**: Executar `dotnet test` (deve passar 62/62)
3. **Desenvolvimento**: Implementar feature
4. **Testes**: Adicionar testes de integração
5. **Validação**: Executar `dotnet test` novamente
6. **Commit**: `git commit -m "Feature: descrição"`
7. **Documentação**: Atualizar sumários

## 📝 TEMPLATE PARA NOVA FEATURE

```csharp
// 1. Entidade
public class NovaFeature
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

// 2. Interface
public interface INovaFeatureService
{
    Task<NovaFeature> CriarAsync(NovaFeatureRequest request);
    Task<List<NovaFeature>> ListarAsync(Guid userId);
}

// 3. Serviço
public class NovaFeatureService : INovaFeatureService
{
    // Implementação
}

// 4. Controller
[ApiController]
[Route("api/nova-feature")]
public class NovaFeatureController : ControllerBase
{
    // Endpoints
}

// 5. Testes
[Fact]
public async Task CriarNovaFeature_ComDadosValidos_RetornaOk()
{
    // Teste
}
```

## 🚀 PRÓXIMOS PASSOS IMEDIATOS

1. Ler `CONTEXT_BASE_PARA_PROXIMO_AGENTE.md`
2. Executar `dotnet test` para validar
3. Escolher uma feature da Fase 2
4. Implementar seguindo o workflow
5. Adicionar testes
6. Fazer commit

---

**Última Atualização**: 22/10/2025
**Status**: Pronto para Fase 2 ✅

