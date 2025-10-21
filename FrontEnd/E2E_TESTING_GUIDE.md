# 🧪 Guia de Testes E2E - Owaypay

**Data:** 2025-10-21  
**Status:** ✅ **PRONTO PARA TESTES**

---

## 📋 Pré-requisitos

### 1. Backend Rodando
```bash
cd Backend/src/FinTechBanking.API.Interna
dotnet run
# Deve estar em http://localhost:5036
```

### 2. Admin Dashboard Rodando
```bash
cd FrontEnd/admin-dashboard
npm run dev
# Deve estar em http://localhost:3000
```

### 3. Internet Banking Rodando
```bash
cd FrontEnd/internet-banking
npm run dev
# Deve estar em http://localhost:3001
```

---

## 🚀 Executar Testes E2E

### Admin Dashboard - Modo Interativo
```bash
cd FrontEnd/admin-dashboard
npm run cypress:open
```

Isso abrirá o Cypress UI onde você pode:
- Ver todos os testes disponíveis
- Executar testes individuais
- Executar todos os testes
- Ver o resultado em tempo real

### Admin Dashboard - Modo Headless
```bash
cd FrontEnd/admin-dashboard
npm run cypress:run
```

Isso executará todos os testes sem interface gráfica.

### Internet Banking - Modo Interativo
```bash
cd FrontEnd/internet-banking
npm run cypress:open
```

### Internet Banking - Modo Headless
```bash
cd FrontEnd/internet-banking
npm run cypress:run
```

---

## 📝 Testes Disponíveis

### Admin Dashboard (8 testes)

1. **Acesso sem autenticação redireciona para login**
   - Verifica se acessar `/` sem token redireciona para `/auth/login`

2. **Login com credenciais válidas**
   - Email: `admin@owaypay.com`
   - Senha: `Admin@123`
   - Verifica redirecionamento para dashboard

3. **Acessar página de clientes**
   - Verifica se consegue navegar para `/clientes`
   - Verifica se a tabela de clientes está visível

4. **Acessar página de transações**
   - Verifica se consegue navegar para `/transacoes`
   - Verifica se a tabela de transações está visível

5. **Logout**
   - Verifica se consegue fazer logout
   - Verifica redirecionamento para login

6. **Carregar dados do dashboard**
   - Verifica se os cards de estatísticas carregam
   - Verifica se os dados são exibidos corretamente

7. **Carregar tabela de clientes**
   - Verifica se a tabela de clientes carrega com dados

8. **Carregar tabela de transações**
   - Verifica se a tabela de transações carrega com dados

### Internet Banking (8 testes)

1. **Acesso sem autenticação redireciona para login**
   - Verifica se acessar `/` sem token redireciona para `/auth/login`

2. **Login com credenciais válidas**
   - Email: `cliente@owaypay.com`
   - Senha: `Cliente@123`
   - Verifica redirecionamento para dashboard

3. **Acessar página de perfil**
   - Verifica se consegue navegar para `/perfil`
   - Verifica se o formulário de perfil está visível

4. **Logout**
   - Verifica se consegue fazer logout
   - Verifica redirecionamento para login

5. **Carregar dados do dashboard**
   - Verifica se os cards de saldo carregam
   - Verifica se as transações recentes carregam

6. **Carregar formulário de perfil**
   - Verifica se o formulário de perfil carrega com dados

7. **Atualizar perfil do cliente**
   - Atualiza nome e telefone
   - Verifica mensagem de sucesso

8. **Acesso negado a /perfil sem autenticação**
   - Verifica se redireciona para login

---

## ✅ Checklist de Testes

### Admin Dashboard
- [ ] Teste 1: Redirecionamento para login
- [ ] Teste 2: Login bem-sucedido
- [ ] Teste 3: Navegação para clientes
- [ ] Teste 4: Navegação para transações
- [ ] Teste 5: Logout
- [ ] Teste 6: Dashboard carrega dados
- [ ] Teste 7: Tabela de clientes carrega
- [ ] Teste 8: Tabela de transações carrega

### Internet Banking
- [ ] Teste 1: Redirecionamento para login
- [ ] Teste 2: Login bem-sucedido
- [ ] Teste 3: Navegação para perfil
- [ ] Teste 4: Logout
- [ ] Teste 5: Dashboard carrega dados
- [ ] Teste 6: Formulário de perfil carrega
- [ ] Teste 7: Atualizar perfil
- [ ] Teste 8: Acesso negado sem autenticação

---

## 🐛 Troubleshooting

### Erro: "Cannot connect to localhost:3000"
**Solução:** Verificar se o frontend está rodando com `npm run dev`

### Erro: "Cannot connect to localhost:5036"
**Solução:** Verificar se o backend está rodando com `dotnet run`

### Erro: "Login failed"
**Solução:** Verificar se as credenciais estão corretas:
- Admin: `admin@owaypay.com / Admin@123`
- Cliente: `cliente@owaypay.com / Cliente@123`

### Erro: "Element not found"
**Solução:** Aumentar o timeout em `cypress.config.ts`:
```typescript
defaultCommandTimeout: 15000, // aumentar de 10000
```

---

## 📊 Relatório de Testes

Após executar todos os testes, você verá um relatório com:
- ✅ Testes passados
- ❌ Testes falhados
- ⏱️ Tempo de execução
- 📸 Screenshots de falhas

---

## 🔄 Executar Testes Continuamente

Para executar testes sempre que há mudanças no código:

```bash
# Admin Dashboard
cd FrontEnd/admin-dashboard
npm run cypress:watch

# Internet Banking
cd FrontEnd/internet-banking
npm run cypress:watch
```

---

## 📚 Documentação Adicional

- [Cypress Documentation](https://docs.cypress.io/)
- [Cypress Best Practices](https://docs.cypress.io/guides/references/best-practices)
- [Cypress API](https://docs.cypress.io/api/table-of-contents)

---

**Status:** ✅ **PRONTO PARA TESTES**

Todos os testes estão configurados e prontos para serem executados!

