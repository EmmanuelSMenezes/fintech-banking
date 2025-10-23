# 🎯 FLUXO DE NEGÓCIO ESPERADO - FINTECH BANKING

## 📋 VISÃO GERAL

Você é o **dono da FinTech** que vende **soluções de pagamento** para seus clientes (empresas).

---

## 🔄 FLUXO COMPLETO

### **FASE 1: ONBOARDING DO CLIENTE (BackOffice - Admin)**

1. **Admin entra no BackOffice**
   - Login: admin@fintech.com / Admin@123
   - Vai para: `/clientes`

2. **Admin cadastra novo cliente**
   - Preenche dados: Email, Nome, Documento, Telefone
   - Sistema gera automaticamente:
     - **Agência**: 0001 (fixa)
     - **Conta**: 000001, 000002, 000003... (sequencial)
   - Sistema envia **email** para o cliente com:
     - Email de acesso
     - Link para cadastrar senha
     - Agência e Conta

3. **Cliente recebe email**
   - Clica no link
   - Vai para: InternetBanking `/register`
   - Cadastra a senha
   - **Cai logado automaticamente**

---

### **FASE 2: CLIENTE USA INTERNET BANKING**

#### **Menu Principal - Operações**
Cliente pode fazer 4 operações principais:

1. **Gerar QR Code PIX**
   - Acessa: `/pix`
   - Clica em "Gerar Cobrança"
   - Preenche: Valor + Descrição
   - Sistema:
     - Registra transação no BD (status: PENDING)
     - Bate no Sicoob
     - Gera QR Code
     - Devolve QR Code para cliente

2. **Gerar Boleto**
   - Acessa: `/boletos` (NOVO)
   - Clica em "Gerar Boleto"
   - Preenche: Valor + Descrição
   - Sistema:
     - Registra transação no BD (status: PENDING)
     - Bate no Sicoob
     - Gera Boleto
     - Devolve Boleto para cliente

3. **Solicitar Saque**
   - Acessa: `/saques`
   - Clica em "Solicitar Saque"
   - Preenche: Valor + Descrição
   - Sistema:
     - Registra transação no BD (status: PENDING)
     - **Coloca na fila** (RabbitMQ)
     - Devolve confirmação

4. **Solicitar TED**
   - Acessa: `/teds` (NOVO)
   - Clica em "Solicitar TED"
   - Preenche: Valor + Banco + Agência + Conta + CPF/CNPJ
   - Sistema:
     - Registra transação no BD (status: PENDING)
     - **Coloca na fila** (RabbitMQ)
     - Devolve confirmação

#### **Menu Integrações**
Cliente pode gerar credenciais para integrar seu sistema:

1. **Gerar SecretKey + ClientID**
   - Acessa: `/integracoes`
   - Clica em "Gerar Credenciais"
   - Sistema gera:
     - **ClientID**: Identificador único
     - **SecretKey**: Chave secreta
   - Cliente copia e guarda com segurança

2. **Registrar Webhook**
   - Acessa: `/integracoes`
   - Clica em "Registrar Webhook"
   - Preenche: URL do webhook
   - Sistema registra no BD
   - Quando houver notificação, dispara para essa URL

---

### **FASE 3: CLIENTE USA API (Integração)**

Cliente integra seu sistema com nossa API Cliente (porta 5167):

#### **1. Autenticação**
```
POST /api/auth/login
Body: {
  "clientId": "xxx",
  "secretKey": "yyy"
}
Response: {
  "accessToken": "jwt-token",
  "expiresIn": 3600
}
```

#### **2. Operações Disponíveis**
```
POST   /api/cliente/cobrancas      - Gerar QR Code PIX
POST   /api/cliente/boletos        - Gerar Boleto
POST   /api/cliente/saques         - Solicitar Saque
POST   /api/cliente/teds           - Solicitar TED
GET    /api/cliente/saldo          - Consultar Saldo
GET    /api/cliente/transacoes     - Acompanhar Extrato
GET    /api/cliente/transacoes/{id} - Detalhes da Transação
```

#### **3. Registrar Webhook (via API)**
```
POST   /api/cliente/webhooks       - Registrar webhook
GET    /api/cliente/webhooks       - Listar webhooks
DELETE /api/cliente/webhooks/{id}  - Deletar webhook
```

---

### **FASE 4: PROCESSAMENTO NO BACKEND**

#### **Quando Cliente Gera QR Code ou Boleto (Síncrono)**
```
1. API Cliente recebe requisição
2. Registra transação no BD (status: PENDING)
3. Bate no Sicoob (Banking Hub)
4. Sicoob retorna QR Code/Boleto
5. API retorna para cliente
6. Cliente recebe QR Code/Boleto imediatamente
```

#### **Quando Cliente Solicita Saque ou TED (Assíncrono)**
```
1. API Cliente recebe requisição
2. Registra transação no BD (status: PENDING)
3. Coloca mensagem na fila (RabbitMQ)
4. API retorna confirmação para cliente
5. Worker processa a fila
6. Worker bate no Sicoob
7. Sicoob processa e retorna resultado
```

---

### **FASE 5: WEBHOOK DO SICOOB**

Quando Sicoob conclui uma operação, ele aciona nosso webhook:

```
POST /api/webhooks/sicoob
Body: {
  "transactionId": "xxx",
  "status": "COMPLETED",
  "type": "PIX_QRCODE | BOLETO | SAQUE | TED",
  "amount": 100.00,
  "result": "SUCCESS | FAILED"
}
```

**Fluxo:**
1. Webhook API recebe notificação do Sicoob
2. Coloca mensagem na fila (RabbitMQ)
3. Consumer processa a fila
4. Consumer atualiza transação no BD (status: COMPLETED/FAILED)
5. Consumer coloca mensagem na fila de notificação
6. Consumer de notificação lê a fila
7. Consumer dispara webhook para o cliente (na URL que ele cadastrou)

---

### **FASE 6: NOTIFICAÇÃO PARA CLIENTE**

Cliente recebe notificação no webhook que cadastrou:

```
POST {webhook_url_do_cliente}
Body: {
  "transactionId": "xxx",
  "status": "COMPLETED",
  "type": "PIX_QRCODE | BOLETO | SAQUE | TED",
  "amount": 100.00,
  "result": "SUCCESS | FAILED",
  "timestamp": "2025-10-23T10:30:00Z"
}
```

Cliente pode:
- Atualizar seu sistema
- Notificar seu usuário final
- Registrar em seu BD

---

## 📊 RESUMO DO FLUXO

```
┌─────────────────────────────────────────────────────────────┐
│ ADMIN (BackOffice)                                          │
│ - Cadastra cliente                                          │
│ - Gera Agência + Conta                                      │
│ - Envia email com link                                      │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ CLIENTE (Email)                                             │
│ - Recebe email com link                                     │
│ - Clica no link                                             │
│ - Cadastra senha                                            │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ CLIENTE (InternetBanking)                                   │
│ - Gera QR Code PIX / Boleto (Síncrono)                     │
│ - Solicita Saque / TED (Assíncrono - Fila)                 │
│ - Gera SecretKey + ClientID                                │
│ - Registra Webhook                                          │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ CLIENTE (API - Integração)                                  │
│ - Autentica com ClientID + SecretKey                        │
│ - Aciona APIs (QR Code, Boleto, Saque, TED)               │
│ - Consulta Saldo e Extrato                                 │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ BACKEND (API + Worker)                                      │
│ - Registra transação no BD                                  │
│ - Bate no Sicoob                                            │
│ - Coloca na fila (se assíncrono)                           │
│ - Worker processa fila                                      │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ SICOOB (Instituição Financeira)                             │
│ - Processa operação                                         │
│ - Retorna resultado                                         │
│ - Aciona webhook com resultado                              │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ BACKEND (Webhook + Consumer)                                │
│ - Recebe webhook do Sicoob                                  │
│ - Atualiza transação no BD                                  │
│ - Coloca na fila de notificação                             │
│ - Consumer dispara webhook para cliente                     │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ CLIENTE (Webhook)                                           │
│ - Recebe notificação                                        │
│ - Atualiza seu sistema                                      │
│ - Notifica seu usuário final                                │
└─────────────────────────────────────────────────────────────┘
```

---

## ✅ CHECKLIST DO QUE PRECISA SER IMPLEMENTADO

### BackOffice
- [x] Login
- [x] Dashboard
- [x] Cadastro de Clientes (com Agência + Conta automática)
- [x] Listar Clientes
- [x] Editar Cliente
- [ ] Envio de email com link de cadastro de senha
- [ ] Visualizar Transações
- [ ] Relatórios

### InternetBanking
- [x] Login
- [x] Cadastro de Senha (via link do email)
- [x] Dashboard
- [x] Gerar QR Code PIX
- [ ] Gerar Boleto
- [x] Solicitar Saque
- [ ] Solicitar TED
- [ ] Menu Integrações (Gerar SecretKey + ClientID)
- [ ] Menu Integrações (Registrar Webhook)
- [ ] Acompanhar Extrato
- [ ] Consultar Saldo

### API Cliente
- [ ] Autenticação com ClientID + SecretKey
- [ ] Gerar QR Code PIX
- [ ] Gerar Boleto
- [ ] Solicitar Saque
- [ ] Solicitar TED
- [ ] Consultar Saldo
- [ ] Acompanhar Extrato
- [ ] Registrar Webhook (via API)

### Backend
- [ ] Envio de email com link de cadastro
- [ ] Autenticação com ClientID + SecretKey
- [ ] Integração com Boleto (Sicoob)
- [ ] Integração com TED (Sicoob)
- [ ] Webhook do Sicoob para Boleto
- [ ] Webhook do Sicoob para TED
- [ ] Consumer para processar Boleto
- [ ] Consumer para processar TED
- [ ] Consumer para notificar cliente

---

**Status**: 📋 Fluxo de negócio documentado e pronto para implementação!

