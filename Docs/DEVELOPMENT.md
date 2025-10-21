# 👨‍💻 Guia de Desenvolvimento

## Estrutura de Pastas

```
src/
├── FinTechBanking.API/
│   ├── Controllers/          # Endpoints REST
│   ├── appsettings.json      # Configurações
│   └── Program.cs            # Setup da aplicação
├── FinTechBanking.Core/
│   ├── Entities/             # Modelos de dados
│   ├── DTOs/                 # Data Transfer Objects
│   └── Interfaces/           # Contratos
├── FinTechBanking.Data/
│   ├── Repositories/         # Acesso a dados (Dapper)
│   └── Migrations/           # Scripts SQL
├── FinTechBanking.Services/
│   ├── Auth/                 # Autenticação
│   └── Messaging/            # RabbitMQ
└── FinTechBanking.Banking/
    ├── Hub/                  # Abstração bancária
    └── Services/             # Integrações específicas
```

## Padrões de Código

### 1. Criar uma Nova Entidade

```csharp
// 1. Criar em Core/Entities/
namespace FinTechBanking.Core.Entities;

public class MyEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
}

// 2. Criar Interface em Core/Interfaces/
namespace FinTechBanking.Core.Interfaces;

public interface IMyEntityRepository
{
    Task<MyEntity> GetByIdAsync(Guid id);
    Task<MyEntity> CreateAsync(MyEntity entity);
    Task<MyEntity> UpdateAsync(MyEntity entity);
    Task<bool> DeleteAsync(Guid id);
}

// 3. Implementar em Data/Repositories/
namespace FinTechBanking.Data.Repositories;

public class MyEntityRepository : IMyEntityRepository
{
    private readonly string _connectionString;

    public MyEntityRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<MyEntity> GetByIdAsync(Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = "SELECT * FROM my_entities WHERE id = @Id";
        return await connection.QueryFirstOrDefaultAsync<MyEntity>(sql, new { Id = id });
    }

    // ... implementar outros métodos
}

// 4. Registrar em Program.cs
builder.Services.AddScoped<IMyEntityRepository>(sp => 
    new MyEntityRepository(connectionString));
```

### 2. Criar um Novo Controller

```csharp
// Controllers/MyController.cs
namespace FinTechBanking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]  // Se precisar autenticação
public class MyController : ControllerBase
{
    private readonly IMyEntityRepository _repository;

    public MyController(IMyEntityRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MyEntity>> GetById(Guid id)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return NotFound();

            return Ok(entity);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error", error = ex.Message });
        }
    }
}
```

### 3. Adicionar Novo Banco

```csharp
// 1. Criar serviço em Banking/Services/
namespace FinTechBanking.Banking.Services;

public class StarkBankService
{
    private readonly string _apiKey;
    private readonly string _apiUrl;

    public StarkBankService(string apiKey, string apiUrl)
    {
        _apiKey = apiKey;
        _apiUrl = apiUrl;
    }

    public async Task<string> GeneratePixQrCodeAsync(decimal amount, string recipientKey, string description)
    {
        // Implementar integração com Stark Bank
        return "qr-code-data";
    }
}

// 2. Registrar em Program.cs
var bankServices = new Dictionary<string, object>
{
    { "001", new SicoobBankService(sicoobApiKey, sicoobApiUrl) },
    { "033", new StarkBankService(starkApiKey, starkApiUrl) }  // Novo
};
```

## Convenções

### Nomenclatura

- **Classes**: PascalCase (ex: `UserRepository`)
- **Métodos**: PascalCase (ex: `GetByIdAsync`)
- **Variáveis**: camelCase (ex: `userId`)
- **Constantes**: UPPER_SNAKE_CASE (ex: `MAX_RETRIES`)
- **Tabelas**: snake_case (ex: `user_accounts`)
- **Colunas**: snake_case (ex: `created_at`)

### Async/Await

Sempre use `async/await` para operações I/O:

```csharp
// ✅ Correto
public async Task<User> GetUserAsync(Guid id)
{
    return await _repository.GetByIdAsync(id);
}

// ❌ Evitar
public User GetUser(Guid id)
{
    return _repository.GetByIdAsync(id).Result;
}
```

### Tratamento de Erros

```csharp
try
{
    // Lógica
}
catch (InvalidOperationException ex)
{
    return BadRequest(new { message = ex.Message });
}
catch (Exception ex)
{
    return StatusCode(500, new { message = "Error", error = ex.Message });
}
```

## Testes

### Executar Testes

```bash
dotnet test
```

### Estrutura de Teste

```csharp
[TestClass]
public class UserRepositoryTests
{
    private UserRepository _repository;

    [TestInitialize]
    public void Setup()
    {
        _repository = new UserRepository("connection-string");
    }

    [TestMethod]
    public async Task GetByIdAsync_WithValidId_ReturnsUser()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(userId);

        // Assert
        Assert.IsNotNull(result);
    }
}
```

## Debugging

### Visual Studio

1. Abra o projeto em Visual Studio
2. Defina breakpoints (F9)
3. Pressione F5 para iniciar debug
4. Use a janela de Debug para inspecionar variáveis

### VS Code

1. Instale a extensão C# Dev Kit
2. Defina breakpoints
3. Pressione F5 para iniciar debug

## Performance

### Dapper Tips

```csharp
// ✅ Usar QueryFirstOrDefaultAsync para um resultado
var user = await connection.QueryFirstOrDefaultAsync<User>(sql, parameters);

// ✅ Usar QueryAsync para múltiplos resultados
var users = await connection.QueryAsync<User>(sql, parameters);

// ❌ Evitar QueryAsync quando espera um resultado
var user = (await connection.QueryAsync<User>(sql, parameters)).FirstOrDefault();
```

### Índices de Banco de Dados

Sempre crie índices para colunas frequentemente consultadas:

```sql
CREATE INDEX idx_users_email ON users(email);
CREATE INDEX idx_transactions_account_id ON transactions(account_id);
```

## Commits

### Mensagens de Commit

```
feat: adicionar novo endpoint de transações
fix: corrigir validação de email
docs: atualizar README
refactor: simplificar lógica de autenticação
test: adicionar testes para UserRepository
```

### Padrão

```
<tipo>: <descrição curta>

<descrição detalhada (opcional)>

<referência de issue (opcional)>
```

## Checklist de Desenvolvimento

- [ ] Código compila sem erros
- [ ] Código segue convenções
- [ ] Testes passam
- [ ] Documentação atualizada
- [ ] Sem warnings desnecessários
- [ ] Commit com mensagem clara

## Recursos Úteis

- [.NET Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [Dapper GitHub](https://github.com/DapperLib/Dapper)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [RabbitMQ Tutorials](https://www.rabbitmq.com/getstarted.html)
- [JWT.io](https://jwt.io/)

## Troubleshooting

### Erro: "Connection refused"
```bash
docker-compose ps  # Verificar se containers estão rodando
docker-compose up -d  # Reiniciar
```

### Erro: "Database does not exist"
```bash
psql -U postgres -h localhost -d fintech_banking -f src/FinTechBanking.Data/Migrations/001_InitialSchema.sql
```

### Erro: "Port already in use"
Mude as portas no `docker-compose.yml` ou mate o processo:
```bash
lsof -i :5432  # Encontrar processo
kill -9 <PID>  # Matar processo
```

---

**Dúvidas?** Abra uma issue ou entre em contato!

