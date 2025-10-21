# 🔧 CORS e Email - Configuração Completa

## ✅ O Que Foi Implementado

### 1. **Correção de CORS**

#### Problema
- CORS estava configurado mas a ordem do middleware estava incorreta
- `UseHttpsRedirection()` em desenvolvimento causava problemas

#### Solução Implementada
- ✅ Adicionada política CORS específica para frontends locais
- ✅ CORS agora vem **antes** de Authentication no pipeline
- ✅ `UseHttpsRedirection()` desabilitado em desenvolvimento
- ✅ Headers de CORS expostos corretamente

#### Frontends Permitidos
```
http://localhost:3000   (Internet Banking)
http://localhost:3001   (Backoffice)
http://localhost:5173   (Frontend React antigo)
```

---

### 2. **Serviço de Email (SMTP)**

#### Arquivos Criados

**`src/FinTechBanking.Services/Email/IEmailService.cs`**
```csharp
public interface IEmailService
{
    Task SendFirstAccessEmailAsync(string email, string fullName, string tempPassword);
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
    Task SendConfirmationEmailAsync(string email, string confirmationLink);
    Task SendPasswordResetEmailAsync(string email, string resetLink);
}
```

**`src/FinTechBanking.Services/Email/SmtpEmailService.cs`**
- Implementação completa com SMTP
- Suporte a SSL/TLS
- Templates HTML para emails
- Logging de erros

#### Métodos Disponíveis

1. **SendFirstAccessEmailAsync** - Envia credenciais de primeiro acesso
2. **SendEmailAsync** - Envia email genérico
3. **SendConfirmationEmailAsync** - Envia link de confirmação
4. **SendPasswordResetEmailAsync** - Envia link de reset de senha

---

### 3. **Endpoint de Cadastro no Backoffice**

#### Novo Endpoint
```
POST /api/admin/users
Authorization: Bearer {token}
Role: admin
```

#### Request Body
```json
{
  "email": "cliente@example.com",
  "fullName": "João Silva",
  "document": "12345678900",
  "phoneNumber": "11999999999"
}
```

#### Response
```json
{
  "message": "Usuário criado com sucesso",
  "data": {
    "id": "uuid",
    "email": "cliente@example.com",
    "fullName": "João Silva",
    "isActive": true,
    "emailSent": true
  }
}
```

#### Fluxo
1. Admin cria usuário no Backoffice
2. Sistema gera senha temporária aleatória
3. Senha é hasheada com BCrypt
4. Usuário é salvo no banco
5. Email é enviado com credenciais
6. Cliente recebe email com primeiro acesso

---

## 📧 Configuração de Email (SMTP)

### Opção 1: Gmail (Recomendado para Testes)

1. **Ativar 2FA no Gmail**
   - Acesse: https://myaccount.google.com/security
   - Ative "Verificação em duas etapas"

2. **Gerar Senha de App**
   - Acesse: https://myaccount.google.com/apppasswords
   - Selecione: Mail e Windows Computer
   - Copie a senha gerada

3. **Configurar em `appsettings.json`**
```json
{
  "Email": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUsername": "seu-email@gmail.com",
    "SmtpPassword": "sua-senha-app-gerada",
    "FromEmail": "seu-email@gmail.com",
    "FromName": "FinTech Banking"
  }
}
```

### Opção 2: Outlook/Hotmail

```json
{
  "Email": {
    "SmtpServer": "smtp-mail.outlook.com",
    "SmtpPort": 587,
    "SmtpUsername": "seu-email@outlook.com",
    "SmtpPassword": "sua-senha",
    "FromEmail": "seu-email@outlook.com",
    "FromName": "FinTech Banking"
  }
}
```

### Opção 3: SendGrid (Produção)

```json
{
  "Email": {
    "SmtpServer": "smtp.sendgrid.net",
    "SmtpPort": 587,
    "SmtpUsername": "apikey",
    "SmtpPassword": "SG.sua-api-key",
    "FromEmail": "noreply@fintech.com",
    "FromName": "FinTech Banking"
  }
}
```

---

## 🧪 Testando

### 1. Testar CORS

```bash
# Fazer requisição do frontend
curl -X GET http://localhost:5066/api/admin/dashboard \
  -H "Authorization: Bearer {token}" \
  -H "Origin: http://localhost:3001"
```

### 2. Testar Cadastro de Usuário

```bash
# 1. Fazer login como admin
curl -X POST http://localhost:5066/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@example.com",
    "password": "admin123"
  }'

# 2. Copiar o token retornado

# 3. Criar novo usuário
curl -X POST http://localhost:5066/api/admin/users \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "novo-cliente@example.com",
    "fullName": "Novo Cliente",
    "document": "12345678900",
    "phoneNumber": "11999999999"
  }'
```

### 3. Testar Email

Verifique a caixa de entrada do email configurado. Você deve receber um email com:
- Bem-vindo ao FinTech Banking
- Email do cliente
- Senha temporária
- Instruções de primeiro acesso

---

## 📝 Notas Importantes

### Segurança
- ⚠️ **Nunca** commitar credenciais SMTP no Git
- Use variáveis de ambiente em produção
- Altere a senha temporária no primeiro acesso
- Use HTTPS em produção

### Desenvolvimento
- Credenciais SMTP devem estar em `appsettings.json` (local)
- Use `appsettings.Development.json` para override local
- Não commitar `appsettings.json` com credenciais reais

### Produção
- Use variáveis de ambiente
- Configure em `appsettings.Production.json`
- Use serviço de email profissional (SendGrid, AWS SES, etc)
- Implemente rate limiting para envio de emails

---

## 🔍 Troubleshooting

### Erro: "CORS policy: No 'Access-Control-Allow-Origin' header"
- ✅ Verificar se frontend está na lista de origens permitidas
- ✅ Verificar se CORS vem antes de Authentication
- ✅ Verificar se `AllowCredentials()` está configurado

### Erro: "Email não foi enviado"
- ✅ Verificar credenciais SMTP
- ✅ Verificar se 2FA está ativado (Gmail)
- ✅ Verificar se porta SMTP está correta (587 ou 465)
- ✅ Verificar logs da aplicação

### Erro: "Usuário já existe"
- ✅ Email já foi cadastrado
- ✅ Usar email diferente

---

## 📊 Arquivos Modificados

| Arquivo | Mudança |
|---------|---------|
| `src/FinTechBanking.API.Cliente/Program.cs` | CORS corrigido |
| `src/FinTechBanking.API.Interna/Program.cs` | CORS corrigido + Email registrado |
| `src/FinTechBanking.API.Interna/appsettings.json` | Configuração de Email |
| `src/FinTechBanking.API.Interna/Controllers/AdminController.cs` | Novo endpoint POST /users |
| `src/FinTechBanking.Services/Email/IEmailService.cs` | Interface criada |
| `src/FinTechBanking.Services/Email/SmtpEmailService.cs` | Implementação criada |
| `src/FinTechBanking.Services/FinTechBanking.Services.csproj` | Pacotes adicionados |
| `src/FinTechBanking.API.Interna/FinTechBanking.API.Interna.csproj` | Referências adicionadas |

---

## ✅ Status

- ✅ CORS corrigido
- ✅ Serviço de Email implementado
- ✅ Endpoint de cadastro criado
- ✅ Compilação 100% sucesso
- ✅ Pronto para testes

---

## 🚀 Próximos Passos

1. Configurar credenciais SMTP em `appsettings.json`
2. Testar CORS com frontend
3. Testar cadastro de usuário
4. Testar envio de email
5. Implementar confirmação de email
6. Implementar reset de senha


