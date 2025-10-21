# 🔍 Swagger Setup - FinTech Banking APIs

## Overview

Swagger (Swashbuckle.AspNetCore) foi instalado e configurado em todas as 3 APIs para documentação automática e teste de endpoints.

## 📦 Instalação

### Pacote Instalado
- **Swashbuckle.AspNetCore v9.0.6**

### APIs Configuradas
1. ✅ FinTechBanking.API (Principal)
2. ✅ FinTechBanking.API.Cliente (Pública)
3. ✅ FinTechBanking.API.Interna (Privada)

## 🚀 Acessar Swagger

### FinTechBanking.API (Principal)
```
URL: http://localhost:5064
Swagger UI: http://localhost:5064/swagger
OpenAPI JSON: http://localhost:5064/openapi/v1.json
```

### FinTechBanking.API.Cliente
```
URL: http://localhost:5167
Swagger UI: http://localhost:5167/swagger
OpenAPI JSON: http://localhost:5167/openapi/v1.json
```

### FinTechBanking.API.Interna
```
URL: http://localhost:5036
Swagger UI: http://localhost:5036/swagger
OpenAPI JSON: http://localhost:5036/openapi/v1.json
```

## 🔐 Autenticação com JWT

### Passo 1: Fazer Login
1. Abra o Swagger da API
2. Procure por `/api/auth/login`
3. Clique em "Try it out"
4. Preencha com credenciais válidas:
```json
{
  "email": "user@example.com",
  "password": "password123"
}
```
5. Clique em "Execute"
6. Copie o `accessToken` da resposta

### Passo 2: Autorizar no Swagger
1. Clique no botão **"Authorize"** (cadeado) no topo
2. Cole o token no formato: `Bearer <seu_token>`
3. Clique em "Authorize"
4. Agora todos os endpoints protegidos estarão acessíveis

## 📋 Configurações Implementadas

### Swagger Gen Configuration
```csharp
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Name",
        Version = "v1",
        Description = "API Description",
        Contact = new OpenApiContact
        {
            Name = "FinTech Banking",
            Email = "support@fintech.com"
        }
    });

    // JWT Bearer Support
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
```

### Middleware Configuration
```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        c.RoutePrefix = "swagger";
    });
}
```

## 🎯 Recursos Disponíveis

### FinTechBanking.API
- **Auth**: Login, Register
- **Transactions**: Create, Get, List
- **Accounts**: Get Balance, Get Details
- **Admin**: User Management, Reports

### FinTechBanking.API.Cliente
- **Auth**: Login, Register
- **Transactions**: PIX QR Code, Withdrawal, Status
- **Account**: Get Balance, Get Transactions

### FinTechBanking.API.Interna
- **Admin**: List Users, Get User Details
- **Transactions**: List, Get Report
- **Dashboard**: Statistics, Metrics

## 📝 Exemplo de Uso

### 1. Login
```bash
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "password123"
}
```

### 2. Usar Token
```bash
GET /api/transactions
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## 🔧 Troubleshooting

### Swagger não carrega
- Verifique se a API está rodando
- Confirme a porta correta
- Limpe o cache do navegador

### Token inválido
- Faça login novamente
- Copie o token completo
- Use o formato: `Bearer <token>`

### CORS Error
- CORS está habilitado para todas as origens
- Verifique se a API está respondendo

## 📚 Documentação Adicional

- [Swashbuckle GitHub](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
- [OpenAPI Specification](https://spec.openapis.org/oas/v3.0.3)
- [Swagger UI](https://swagger.io/tools/swagger-ui/)

---

**Desenvolvido com ❤️ para FinTech Banking**

