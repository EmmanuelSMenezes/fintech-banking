// ============================================================================
// TESTE E2E - FLUXO COMPLETO DO ADMIN DASHBOARD
// ============================================================================

describe('Admin Dashboard - Fluxo Completo', () => {
  const adminEmail = 'admin@owaypay.com';
  const adminPassword = 'Admin@123';

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
    cy.contains('Bem-vindo ao Painel Administrativo').should('be.visible');
  });

  // ========================================================================
  // TESTE 2: Login com credenciais válidas
  // ========================================================================
  it('Deve fazer login com credenciais válidas', () => {
    cy.visit('/auth/login');
    
    // Preencher formulário
    cy.get('input[name="email"]').type(adminEmail);
    cy.get('input[name="password"]').type(adminPassword);
    
    // Clicar em Entrar
    cy.contains('button', 'Entrar').click();
    
    // Verificar redirecionamento para dashboard
    cy.url().should('eq', 'http://localhost:3000/');
    cy.contains('Dashboard').should('be.visible');
  });

  // ========================================================================
  // TESTE 3: Acessar página de clientes
  // ========================================================================
  it('Deve acessar página de clientes após login', () => {
    // Login
    cy.visit('/auth/login');
    cy.get('input[name="email"]').type(adminEmail);
    cy.get('input[name="password"]').type(adminPassword);
    cy.contains('button', 'Entrar').click();
    
    // Aguardar dashboard carregar
    cy.url().should('eq', 'http://localhost:3000/');
    
    // Clicar em Clientes no menu
    cy.contains('Clientes').click();
    
    // Verificar que estamos na página de clientes
    cy.url().should('include', '/clientes');
    cy.contains('Gerenciamento de Clientes').should('be.visible');
  });

  // ========================================================================
  // TESTE 4: Acessar página de transações
  // ========================================================================
  it('Deve acessar página de transações após login', () => {
    // Login
    cy.visit('/auth/login');
    cy.get('input[name="email"]').type(adminEmail);
    cy.get('input[name="password"]').type(adminPassword);
    cy.contains('button', 'Entrar').click();
    
    // Aguardar dashboard carregar
    cy.url().should('eq', 'http://localhost:3000/');
    
    // Clicar em Transações no menu
    cy.contains('Transações').click();
    
    // Verificar que estamos na página de transações
    cy.url().should('include', '/transacoes');
    cy.contains('Relatório de Transações').should('be.visible');
  });

  // ========================================================================
  // TESTE 5: Logout
  // ========================================================================
  it('Deve fazer logout e redirecionar para login', () => {
    // Login
    cy.visit('/auth/login');
    cy.get('input[name="email"]').type(adminEmail);
    cy.get('input[name="password"]').type(adminPassword);
    cy.contains('button', 'Entrar').click();
    
    // Aguardar dashboard carregar
    cy.url().should('eq', 'http://localhost:3000/');
    
    // Clicar no avatar do usuário
    cy.get('[role="button"]').first().click();
    
    // Clicar em Sair
    cy.contains('Sair').click();
    
    // Verificar redirecionamento para login
    cy.url().should('include', '/auth/login');
  });

  // ========================================================================
  // TESTE 6: Verificar que dados carregam no dashboard
  // ========================================================================
  it('Deve carregar dados do dashboard corretamente', () => {
    // Login
    cy.visit('/auth/login');
    cy.get('input[name="email"]').type(adminEmail);
    cy.get('input[name="password"]').type(adminPassword);
    cy.contains('button', 'Entrar').click();
    
    // Aguardar dashboard carregar
    cy.url().should('eq', 'http://localhost:3000/');
    
    // Verificar que os cards de estatísticas estão visíveis
    cy.contains('Total de Transações').should('be.visible');
    cy.contains('Valor Total').should('be.visible');
    cy.contains('Transações Pendentes').should('be.visible');
    cy.contains('Usuários Ativos').should('be.visible');
  });

  // ========================================================================
  // TESTE 7: Verificar que tabela de clientes carrega
  // ========================================================================
  it('Deve carregar tabela de clientes com dados', () => {
    // Login
    cy.visit('/auth/login');
    cy.get('input[name="email"]').type(adminEmail);
    cy.get('input[name="password"]').type(adminPassword);
    cy.contains('button', 'Entrar').click();
    
    // Ir para página de clientes
    cy.url().should('eq', 'http://localhost:3000/');
    cy.contains('Clientes').click();
    cy.url().should('include', '/clientes');
    
    // Verificar que a tabela está visível
    cy.contains('Gerenciamento de Clientes').should('be.visible');
    cy.get('table').should('be.visible');
  });

  // ========================================================================
  // TESTE 8: Verificar que tabela de transações carrega
  // ========================================================================
  it('Deve carregar tabela de transações com dados', () => {
    // Login
    cy.visit('/auth/login');
    cy.get('input[name="email"]').type(adminEmail);
    cy.get('input[name="password"]').type(adminPassword);
    cy.contains('button', 'Entrar').click();
    
    // Ir para página de transações
    cy.url().should('eq', 'http://localhost:3000/');
    cy.contains('Transações').click();
    cy.url().should('include', '/transacoes');
    
    // Verificar que a tabela está visível
    cy.contains('Relatório de Transações').should('be.visible');
    cy.get('table').should('be.visible');
  });
});

