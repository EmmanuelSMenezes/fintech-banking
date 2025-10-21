# 🚀 Setup do FinTech Banking Gateway

## Pré-requisitos

- **.NET 9 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Docker** e **Docker Compose** - [Download](https://www.docker.com/products/docker-desktop)
- **Git** - [Download](https://git-scm.com/)

## 1️⃣ Clonar o Repositório

```bash
git clone <repository-url>
cd Fintech-banking
```

## 2️⃣ Iniciar Serviços com Docker

```bash
docker-compose up -d
```

Isso iniciará:
- **PostgreSQL** em `localhost:5432`
- **RabbitMQ** em `localhost:5672` (Management UI: `http://localhost:15672`)

## 3️⃣ Criar Banco de Dados

```bash
# Conectar ao PostgreSQL
psql -U postgres -h localhost -d fintech_banking

# Executar o script de migrations
\i src/FinTechBanking.Data/Migrations/001_InitialSchema.sql

# Sair
\q
```

Ou use uma ferramenta como **pgAdmin** ou **DBeaver** para executar o script.

## 4️⃣ Restaurar Dependências

```bash
dotnet restore
```

## 5️⃣ Compilar o Projeto

```bash
dotnet build
```

## 6️⃣ Executar a API

```bash
cd src/FinTechBanking.API
dotnet run
```

A API estará disponível em:
- **HTTPS**: `https://localhost:5001`
- **HTTP**: `http://localhost:5000`

## 📚 Endpoints para Testar

### 1. Registrar Usuário

```bash
curl -X POST https://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "Password123!",
    "fullName": "John Doe",
    "document": "12345678901",
    "phoneNumber": "11999999999"
  }'
```

### 2. Fazer Login

```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "Password123!"
  }'
```

### 3. Obter Saldo da Conta

```bash
curl -X GET https://localhost:5001/api/accounts/balance \
  -H "Authorization: Bearer <seu-token-jwt>"
```

## 🛠️ Ferramentas Úteis

### pgAdmin (Gerenciar PostgreSQL)

```bash
docker run -d \
  --name pgadmin \
  -e PGADMIN_DEFAULT_EMAIL=admin@example.com \
  -e PGADMIN_DEFAULT_PASSWORD=admin \
  -p 5050:80 \
  dpage/pgadmin4
```

Acesse: `http://localhost:5050`

### RabbitMQ Management UI

Acesse: `http://localhost:15672`
- Usuário: `guest`
- Senha: `guest`

## 🧹 Limpar Tudo

```bash
# Parar e remover containers
docker-compose down

# Remover volumes (dados)
docker-compose down -v
```

## 📝 Variáveis de Ambiente

Edite `src/FinTechBanking.API/appsettings.json` para configurar:

- **ConnectionStrings**: String de conexão do PostgreSQL
- **Jwt**: Configurações de autenticação JWT
- **RabbitMq**: URL de conexão do RabbitMQ
- **Banking**: Credenciais dos bancos

## ❓ Troubleshooting

### Erro: "Connection refused"

Verifique se os containers estão rodando:
```bash
docker-compose ps
```

### Erro: "Database does not exist"

Execute o script de migrations novamente:
```bash
psql -U postgres -h localhost -d fintech_banking -f src/FinTechBanking.Data/Migrations/001_InitialSchema.sql
```

### Erro: "Port already in use"

Mude as portas no `docker-compose.yml`:
```yaml
ports:
  - "5433:5432"  # PostgreSQL
  - "5673:5672"  # RabbitMQ
```

## 📖 Próximos Passos

1. Implementar Consumer de Requisições
2. Implementar Consumer de Webhooks
3. Integração real com API Sicoob
4. Criar Frontend React
5. Escrever testes unitários e de integração

---

**Dúvidas?** Abra uma issue no repositório!

