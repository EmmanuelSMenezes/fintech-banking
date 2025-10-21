// ============================================================================
// TESTE E2E - FLUXO COMPLETO DO INTERNET BANKING
// ============================================================================

describe('Internet Banking - Fluxo Completo', () => {
  const clienteEmail = 'cliente@owaypay.com';
  const clientePassword = 'Cliente@123';

  beforeEach(() => {
    // Limpar localStorage antes de cada teste
    cy.clearLocalStorage();
  });

  // ========================================================================
  // TESTE 1: Acesso sem autenticação redireciona para login
  // ========================================================================
  it('Deve redirecionar para login quando acessar / sem autenticação', () => {
    cy.visit('/');
    cy.url().should('include', '/auth/login');
    cy.contains('Bem-vindo ao Internet Banking').should('be.visible');
  });

  // ========================================================================
  // TESTE 2: Login com credenciais válidas
  // ========================================================================
  it('Deve fazer login com credenciais válidas', () => {
    cy.visit('/auth/login');
    
    // Preencher formulário
    cy.get('input[name="email"]').type(clienteEmail);
    cy.get('input[name="password"]').type(clientePassword);
    
    // Clicar em Entrar
    cy.contains('button', 'Entrar').click();
    
    // Verificar redirecionamento para dashboard
    cy.url().should('eq', 'http://localhost:3001/');
    cy.contains('Dashboard').should('be.visible');
  });

  // ========================================================================
  // TESTE 3: Acessar página de perfil
  // ========================================================================
  it('Deve acessar página de perfil após login', () => {
    // Login
    cy.visit('/auth/login');
    cy.get('input[name="email"]').type(clienteEmail);
    cy.get('input[name="password"]').type(clientePassword);
    cy.contains('button', 'Entrar').click();
    
    // Aguardar dashboard carregar
    cy.url().should('eq', 'http://localhost:3001/');
    
    // Clicar em Meu Perfil no menu
    cy.contains('Meu Perfil').click();
    
    // Verificar que estamos na página de perfil
    cy.url().should('include', '/perfil');
    cy.contains('Informações Básicas').should('be.visible');
  });

  // ========================================================================
  // TESTE 4: Logout
  // ========================================================================
  it('Deve fazer logout e redirecionar para login', () => {
    // Login
    cy.visit('/auth/login');
    cy.get('input[name="email"]').type(clienteEmail);
    cy.get('input[name="password"]').type(clientePassword);
    cy.contains('button', 'Entrar').click();
    
    // Aguardar dashboard carregar
    cy.url().should('eq', 'http://localhost:3001/');
    
    // Clicar no avatar do usuário
    cy.get('[role="button"]').first().click();
    
    // Clicar em Sair
    cy.contains('Sair').click();
    
    // Verificar redirecionamento para login
    cy.url().should('include', '/auth/login');
  });

  // ========================================================================
  // TESTE 5: Verificar que dados carregam no dashboard
  // ========================================================================
  it('Deve carregar dados do dashboard corretamente', () => {
    // Login
    cy.visit('/auth/login');
    cy.get('input[name="email"]').type(clienteEmail);
    cy.get('input[name="password"]').type(clientePassword);
    cy.contains('button', 'Entrar').click();
    
    // Aguardar dashboard carregar
    cy.url().should('eq', 'http://localhost:3001/');
    
    // Verificar que os cards de saldo estão visíveis
    cy.contains('Saldo Total').should('be.visible');
    cy.contains('Saldo Disponível').should('be.visible');
    cy.contains('Últimas Transações').should('be.visible');
  });

  // ========================================================================
  // TESTE 6: Verificar que formulário de perfil carrega
  // ========================================================================
  it('Deve carregar formulário de perfil com dados', () => {
    // Login
    cy.visit('/auth/login');
    cy.get('input[name="email"]').type(clienteEmail);
    cy.get('input[name="password"]').type(clientePassword);
    cy.contains('button', 'Entrar').click();
    
    // Ir para página de perfil
    cy.url().should('eq', 'http://localhost:3001/');
    cy.contains('Meu Perfil').click();
    cy.url().should('include', '/perfil');
    
    // Verificar que o formulário está visível
    cy.contains('Informações Básicas').should('be.visible');
    cy.get('input[name="fullName"]').should('be.visible');
    cy.get('input[name="phoneNumber"]').should('be.visible');
  });

  // ========================================================================
  // TESTE 7: Atualizar perfil do cliente
  // ========================================================================
  it('Deve atualizar perfil do cliente com sucesso', () => {
    // Login
    cy.visit('/auth/login');
    cy.get('input[name="email"]').type(clienteEmail);
    cy.get('input[name="password"]').type(clientePassword);
    cy.contains('button', 'Entrar').click();
    
    // Ir para página de perfil
    cy.url().should('eq', 'http://localhost:3001/');
    cy.contains('Meu Perfil').click();
    cy.url().should('include', '/perfil');
    
    // Atualizar nome
    cy.get('input[name="fullName"]').clear().type('Cliente Teste Atualizado');
    
    // Atualizar telefone
    cy.get('input[name="phoneNumber"]').clear().type('(11) 98765-4321');
    
    // Clicar em Salvar Alterações
    cy.contains('button', 'Salvar Alterações').click();
    
    // Verificar mensagem de sucesso
    cy.contains('Perfil atualizado com sucesso').should('be.visible');
  });

  // ========================================================================
  // TESTE 8: Verificar acesso negado a /perfil sem autenticação
  // ========================================================================
  it('Deve redirecionar para login ao acessar /perfil sem autenticação', () => {
    cy.visit('/perfil');
    cy.url().should('include', '/auth/login');
  });
});

