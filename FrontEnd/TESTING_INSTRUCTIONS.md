# 🧪 Instruções de Teste - Auditoria de Rotas

**Data:** 2025-10-21  
**Objetivo:** Validar que todas as rotas de API e React estão funcionando corretamente

---

## 🚀 Pré-requisitos

### 1. Banco de Dados
```bash
# PostgreSQL deve estar rodando na porta 5432
# Banco: fintech_banking
# Usuário: postgres
# Senha: postgres
```

### 2. RabbitMQ
```bash
# RabbitMQ deve estar rodando na porta 5672
# Management UI: http://localhost:15672
```

### 3. Backend
```bash
# Compilar e rodar a API Interna
cd Backend/src/FinTechBanking.API.Interna
dotnet run
# Deve estar rodando em http://localhost:5036
```

---

## 📋 Cenários de Teste

### Cenário 1: Acesso à Página Inicial Sem Autenticação

**Admin Dashboard**
```
1. Abrir http://localhost:3000
2. Esperado: Redirecionar para http://localhost:3000/auth/login
3. Resultado: ✅ ou ❌
```

**Internet Banking**
```
1. Abrir http://localhost:3001
2. Esperado: Redirecionar para http://localhost:3001/auth/login
3. Resultado: ✅ ou ❌
```

### Cenário 2: Login com Credenciais Válidas

**Admin Dashboard**
```
1. Ir para http://localhost:3000/auth/login
2. Email: admin@owaypay.com
3. Senha: Admin@123
4. Clicar em "Entrar"
5. Esperado: Redirecionar para http://localhost:3000 (dashboard)
6. Resultado: ✅ ou ❌
```

**Internet Banking**
```
1. Ir para http://localhost:3001/auth/login
2. Email: cliente@owaypay.com
3. Senha: Cliente@123
4. Clicar em "Entrar"
5. Esperado: Redirecionar para http://localhost:3001 (dashboard)
6. Resultado: ✅ ou ❌
```

### Cenário 3: Verificar Endpoints de API

**Admin Dashboard - Dashboard**
```
GET http://localhost:5036/api/admin/dashboard
Esperado: 200 OK com dados de dashboard
```

**Admin Dashboard - Clientes**
```
GET http://localhost:5036/api/admin/users
Esperado: 200 OK com lista de usuários
```

**Admin Dashboard - Transações**
```
GET http://localhost:5036/api/admin/transactions
Esperado: 200 OK com lista de transações
```

**Internet Banking - Saldo**
```
GET http://localhost:5036/api/cliente/saldo
Esperado: 200 OK com dados de saldo
```

**Internet Banking - Transações**
```
GET http://localhost:5036/api/cliente/transacoes
Esperado: 200 OK com lista de transações
```

### Cenário 4: Navegação Entre Páginas

**Admin Dashboard**
```
1. Login bem-sucedido
2. Clicar em "Clientes" (se houver link)
3. Esperado: Ir para http://localhost:3000/clientes
4. Resultado: ✅ ou ❌

5. Clicar em "Transações" (se houver link)
6. Esperado: Ir para http://localhost:3000/transacoes
7. Resultado: ✅ ou ❌
```

**Internet Banking**
```
1. Login bem-sucedido
2. Clicar em "Meu Perfil" (se houver link)
3. Esperado: Ir para http://localhost:3001/perfil
4. Resultado: ✅ ou ❌
```

### Cenário 5: Logout

**Admin Dashboard**
```
1. Estar autenticado
2. Clicar em "Logout" (se houver)
3. Esperado: Redirecionar para http://localhost:3000/auth/login
4. Resultado: ✅ ou ❌
```

**Internet Banking**
```
1. Estar autenticado
2. Clicar em "Logout" (se houver)
3. Esperado: Redirecionar para http://localhost:3001/auth/login
4. Resultado: ✅ ou ❌
```

---

## 🔍 Verificação de Console

### Erros Esperados: NENHUM

Abrir DevTools (F12) e verificar:
- ❌ Nenhum erro de rede (404, 500, etc)
- ❌ Nenhum erro de JavaScript
- ❌ Nenhum aviso de CORS

### Logs Esperados

```
✅ Login bem-sucedido
✅ Token armazenado
✅ Dados carregados
✅ Redirecionamento funcionando
```

---

## 📊 Checklist de Teste

- [ ] Admin Dashboard - Acesso sem autenticação redireciona para login
- [ ] Admin Dashboard - Login com credenciais válidas funciona
- [ ] Admin Dashboard - Dashboard carrega dados corretamente
- [ ] Admin Dashboard - Página de clientes carrega dados
- [ ] Admin Dashboard - Página de transações carrega dados
- [ ] Internet Banking - Acesso sem autenticação redireciona para login
- [ ] Internet Banking - Login com credenciais válidas funciona
- [ ] Internet Banking - Dashboard carrega saldo e transações
- [ ] Internet Banking - Página de perfil carrega dados
- [ ] Nenhum erro de rede no console
- [ ] Nenhum erro de JavaScript no console
- [ ] Logout funciona em ambos frontends

---

## 🐛 Troubleshooting

### Erro: "Cannot GET /"
**Solução:** Verificar se o frontend está rodando (`npm run dev`)

### Erro: "API connection refused"
**Solução:** Verificar se o backend está rodando (`dotnet run`)

### Erro: "401 Unauthorized"
**Solução:** Verificar se o token JWT está sendo enviado corretamente

### Erro: "CORS error"
**Solução:** Verificar se CORS está habilitado no backend

---

## 📝 Relatório de Teste

Após completar todos os testes, preencher:

```
Data: ___/___/_____
Testador: ________________

Testes Passados: ___/12
Testes Falhados: ___/12

Problemas Encontrados:
1. ___________________________
2. ___________________________
3. ___________________________

Observações:
_________________________________
_________________________________
```

---

**Próximos Passos:** Após todos os testes passarem, implementar navegação e ações reais.

