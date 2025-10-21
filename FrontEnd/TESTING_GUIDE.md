# 🧪 Guia de Testes - Páginas Reais Owaypay

**Data:** 2025-10-21  
**Status:** ✅ Pronto para Testes

---

## 📋 Pré-requisitos

Antes de testar, certifique-se de que:

1. ✅ Backend está rodando (API Interna na porta 5036)
2. ✅ PostgreSQL está rodando (porta 5432)
3. ✅ RabbitMQ está rodando (porta 5672)
4. ✅ Dados de teste foram inseridos no banco

---

## 🚀 Como Iniciar os Frontends

### Admin Dashboard (Porta 3000)

```bash
cd FrontEnd/admin-dashboard
npm install  # Se necessário
npm run dev
```

Acesse: http://localhost:3000

### Internet Banking (Porta 3001)

```bash
cd FrontEnd/internet-banking
npm install  # Se necessário
npm run dev
```

Acesse: http://localhost:3001

---

## 🧪 Testes por Página

### 1. Admin Dashboard (Home)

**URL:** http://localhost:3000

**Credenciais:**
- Email: `admin@owaypay.com`
- Senha: `Admin@123`

**O que testar:**
- [ ] Login com credenciais corretas
- [ ] 4 cards com estatísticas aparecem
- [ ] Valores estão formatados em R$
- [ ] Tabela de transações recentes carrega
- [ ] Status das transações mostram chips coloridos
- [ ] Datas estão formatadas em pt-BR
- [ ] Loading spinner aparece durante requisição
- [ ] Erro é exibido se API falhar

**Esperado:**
```
Total de Transações: 150
Valor Total: R$ 500.000,00
Pendentes: 12
Usuários Ativos: 45
```

---

### 2. Gerenciamento de Clientes

**URL:** http://localhost:3000/clientes

**O que testar:**
- [ ] Tabela carrega com lista de clientes
- [ ] Busca por nome funciona em tempo real
- [ ] Busca por email funciona em tempo real
- [ ] Paginação funciona (5, 10, 25 linhas)
- [ ] Status ativo/inativo mostra chips corretos
- [ ] Menu de ações abre ao clicar no ícone
- [ ] Datas de criação estão formatadas
- [ ] Contador total de clientes está correto

**Esperado:**
- Mínimo 5 clientes na lista
- Busca filtra resultados em tempo real
- Paginação funciona corretamente

---

### 3. Relatório de Transações

**URL:** http://localhost:3000/transacoes

**O que testar:**
- [ ] Tabela carrega com todas as transações
- [ ] Filtro por Status funciona
  - [ ] Todos
  - [ ] Pendente
  - [ ] Concluída
  - [ ] Falha
- [ ] Filtro por Tipo funciona
  - [ ] Todos
  - [ ] PIX
  - [ ] Saque
  - [ ] Transferência
- [ ] Valores estão formatados em R$
- [ ] Chips de status têm cores corretas
- [ ] Paginação funciona
- [ ] Combinação de filtros funciona

**Esperado:**
- Mínimo 10 transações na lista
- Filtros reduzem resultados corretamente
- Valores em R$ com 2 casas decimais

---

### 4. Cliente Dashboard (Home)

**URL:** http://localhost:3001

**Credenciais:**
- Email: `cliente@owaypay.com`
- Senha: `Cliente@123`

**O que testar:**
- [ ] Login com credenciais corretas
- [ ] Card de saldo carrega com gradiente azul
- [ ] Saldo Total, Disponível e Bloqueado aparecem
- [ ] Valores estão formatados em R$
- [ ] Cards de ações rápidas (Pix, Saque) aparecem
- [ ] Tabela de transações carrega
- [ ] Transações mostram tipo (ENTRADA/SAÍDA)
- [ ] Cores indicam entrada (verde) e saída (vermelho)
- [ ] Datas estão formatadas em pt-BR
- [ ] Loading spinners aparecem durante requisições

**Esperado:**
```
Saldo Total: R$ 500.000,00
Disponível: R$ 450.000,00
Bloqueado: R$ 50.000,00
```

---

### 5. Meu Perfil

**URL:** http://localhost:3001/perfil

**O que testar:**
- [ ] Dados do cliente carregam
- [ ] Email está desabilitado
- [ ] CPF está desabilitado
- [ ] Data de criação está desabilitada
- [ ] Nome completo pode ser editado
- [ ] Telefone pode ser editado
- [ ] Validação funciona (nome obrigatório)
- [ ] Botão Salvar funciona
- [ ] Mensagem de sucesso aparece
- [ ] Dados são atualizados após salvar
- [ ] Botão Cancelar reseta formulário

**Esperado:**
- Formulário carrega com dados do cliente
- Edições são salvas com sucesso
- Mensagem de sucesso aparece por 3 segundos

---

## 🔍 Verificações Técnicas

### Console do Navegador

Abra DevTools (F12) e verifique:

- [ ] Nenhum erro em vermelho no console
- [ ] Requisições HTTP estão sendo feitas (aba Network)
- [ ] Status 200 para requisições bem-sucedidas
- [ ] Tokens JWT estão sendo enviados (Authorization header)
- [ ] Respostas JSON estão corretas

### Requisições HTTP

**Admin Dashboard:**
```
GET http://localhost:5036/api/admin/dashboard
Authorization: Bearer <token>
```

**Clientes:**
```
GET http://localhost:5036/api/admin/users?page=1&pageSize=10
Authorization: Bearer <token>
```

**Transações:**
```
GET http://localhost:5036/api/admin/transactions?page=1&pageSize=10
Authorization: Bearer <token>
```

**Cliente Saldo:**
```
GET http://localhost:5036/api/cliente/saldo
Authorization: Bearer <token>
```

**Cliente Transações:**
```
GET http://localhost:5036/api/cliente/transacoes?page=1&limit=10
Authorization: Bearer <token>
```

**Cliente Perfil:**
```
GET http://localhost:5036/api/cliente/perfil
Authorization: Bearer <token>
```

---

## 🐛 Troubleshooting

### Erro: "Erro ao carregar dashboard"

**Solução:**
1. Verifique se API está rodando na porta 5036
2. Verifique se token JWT é válido
3. Verifique logs do backend

### Erro: "Network Error"

**Solução:**
1. Verifique CORS no backend
2. Verifique se API está acessível
3. Verifique firewall

### Dados não aparecem

**Solução:**
1. Verifique se há dados no banco de dados
2. Verifique se usuário tem permissão
3. Verifique logs do backend

---

## ✅ Checklist Final

- [ ] Admin Dashboard carrega com dados reais
- [ ] Clientes página funciona com paginação
- [ ] Transações página funciona com filtros
- [ ] Cliente Dashboard carrega com saldo
- [ ] Perfil página carrega e permite edição
- [ ] Todos os valores estão em R$
- [ ] Todas as datas estão em pt-BR
- [ ] Nenhum erro no console
- [ ] Todas as requisições retornam 200
- [ ] Loading states funcionam
- [ ] Mensagens de erro aparecem

---

**Última atualização:** 2025-10-21  
**Repositório:** https://github.com/EmmanuelSMenezes/fintech-banking.git

