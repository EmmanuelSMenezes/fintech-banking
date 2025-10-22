# 🔧 TROUBLESHOOTING E DICAS

## ❌ PROBLEMAS COMUNS

### 1. Testes Falhando com "ServiceUnavailable"
**Causa**: Docker containers não estão rodando
**Solução**:
```bash
cd Backend
docker-compose up -d --build
docker-compose ps  # Verificar status
```

### 2. Erro "Connection refused" no PostgreSQL
**Causa**: PostgreSQL não está rodando
**Solução**:
```bash
docker-compose logs postgres
docker-compose restart postgres
```

### 3. Erro "The given key was not present in the dictionary"
**Causa**: Estrutura de resposta JSON incorreta
**Solução**: Usar `TryGetProperty()` em vez de `GetProperty()`
```csharp
// ❌ Errado
var token = loginData.GetProperty("data").GetProperty("token").GetString();

// ✅ Correto
if (!loginData.TryGetProperty("accessToken", out var tokenElement))
    return;
var token = tokenElement.GetString();
```

### 4. Erro "Filename too long" no Git
**Causa**: Caminho de arquivo muito longo
**Solução**:
```bash
git config --global core.longpaths true
```

### 5. Testes Lentos
**Causa**: Muitas requisições HTTP
**Solução**: Usar `HttpClientFactory` ou reutilizar cliente
```csharp
// ✅ Bom
using var client = new HttpClient();
var response1 = await client.GetAsync(url1);
var response2 = await client.GetAsync(url2);
```

## ✅ BOAS PRÁTICAS

### 1. Estrutura de Resposta
```csharp
// ✅ Padrão usado no projeto
return Ok(new
{
    message = "Sucesso",
    data = new { /* dados */ },
    accessToken = token,
    expiresIn = "3600"
});
```

### 2. Tratamento de Erros
```csharp
try
{
    // Lógica
}
catch (Exception ex)
{
    _logger.LogError(ex, "Erro ao processar");
    return StatusCode(500, new { message = "Erro interno" });
}
```

### 3. Validações
```csharp
if (string.IsNullOrEmpty(request.Email))
    return BadRequest(new { message = "Email é obrigatório" });

if (request.Amount <= 0)
    return BadRequest(new { message = "Valor deve ser maior que 0" });
```

### 4. Testes de Integração
```csharp
[Fact]
public async Task MinhaFeature_ComDadosValidos_RetornaOk()
{
    // Arrange
    using var client = new HttpClient();
    var request = new { /* dados */ };
    
    // Act
    var response = await client.PostAsJsonAsync(url, request);
    
    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
}
```

## 🔍 DEBUGGING

### 1. Ver Logs do Docker
```bash
docker-compose logs -f api_interna
docker-compose logs -f postgres
docker-compose logs -f rabbitmq
```

### 2. Conectar ao PostgreSQL
```bash
docker exec -it fintech_postgres psql -U postgres -d fintech_banking
# Listar tabelas
\dt
# Ver dados
SELECT * FROM users;
```

### 3. Acessar RabbitMQ Management
```
http://localhost:15672
Username: guest
Password: guest
```

### 4. Testar Endpoint com cURL
```bash
# Login
curl -X POST http://localhost:5036/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@owaypay.com","password":"Admin@123"}'

# Com token
curl -X GET http://localhost:5036/api/admin/dashboard \
  -H "Authorization: Bearer TOKEN_AQUI"
```

## 📊 MONITORAMENTO

### 1. Verificar Saúde da API
```bash
curl http://localhost:5036/health
```

### 2. Ver Métricas
```bash
curl http://localhost:5036/metrics
```

### 3. Verificar Logs
```bash
# Últimas 100 linhas
docker-compose logs --tail=100 api_interna

# Seguir em tempo real
docker-compose logs -f api_interna
```

## 🚀 PERFORMANCE

### 1. Otimizar Queries
```csharp
// ❌ N+1 Query Problem
var users = await _userRepository.GetAllAsync();
foreach (var user in users)
{
    var accounts = await _accountRepository.GetByUserIdAsync(user.Id);
}

// ✅ Usar Include
var users = await _userRepository.GetAllWithAccountsAsync();
```

### 2. Usar Índices
```sql
CREATE INDEX idx_user_email ON users(email);
CREATE INDEX idx_transaction_user_id ON transactions(user_id);
```

### 3. Paginação
```csharp
public async Task<PagedResult<T>> GetPagedAsync(int page, int pageSize)
{
    var skip = (page - 1) * pageSize;
    var items = await _repository.GetAsync(skip, pageSize);
    var total = await _repository.CountAsync();
    return new PagedResult<T> { Items = items, Total = total };
}
```

## 📚 RECURSOS ÚTEIS

- **Documentação .NET**: https://docs.microsoft.com/dotnet
- **PostgreSQL**: https://www.postgresql.org/docs
- **RabbitMQ**: https://www.rabbitmq.com/documentation.html
- **xUnit**: https://xunit.net/docs/getting-started
- **FluentAssertions**: https://fluentassertions.com

## 🎯 CHECKLIST ANTES DE COMMIT

- [ ] Todos os 62 testes passando
- [ ] Sem warnings de compilação
- [ ] Código formatado
- [ ] Comentários em português
- [ ] Mensagem de commit clara
- [ ] Documentação atualizada

---

**Última Atualização**: 22/10/2025

