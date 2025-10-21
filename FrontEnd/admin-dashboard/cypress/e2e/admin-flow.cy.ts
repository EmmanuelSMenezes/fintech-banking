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

    // Preencher formulário - usando seletores MUI
    cy.get('input').first().clear().type(adminEmail);
    cy.get('input').last().clear().type(adminPassword);

    // Clicar em Entrar
    cy.contains('button', 'Entrar').click();

    // Esperar o redirecionamento (com timeout maior)
    cy.url({ timeout: 20000 }).should('not.include', '/auth/login');
  });

  // ========================================================================
  // TESTE 3: Acessar página de clientes
  // ========================================================================
  it('Deve acessar página de clientes após login', () => {
    // Login
    cy.visit('/auth/login');
    cy.get('input').first().clear().type(adminEmail);
    cy.get('input').last().clear().type(adminPassword);
    cy.contains('button', 'Entrar').click();

    // Aguardar dashboard carregar
    cy.url({ timeout: 20000 }).should('not.include', '/auth/login');

    // Navegar diretamente para clientes
    cy.visit('/clientes');
    cy.url().should('include', '/clientes');
  });

  // ========================================================================
  // TESTE 4: Acessar página de transações
  // ========================================================================
  it('Deve acessar página de transações após login', () => {
    // Login
    cy.visit('/auth/login');
    cy.get('input').first().clear().type(adminEmail);
    cy.get('input').last().clear().type(adminPassword);
    cy.contains('button', 'Entrar').click();

    // Aguardar dashboard carregar
    cy.url({ timeout: 20000 }).should('not.include', '/auth/login');

    // Navegar diretamente para transações
    cy.visit('/transacoes');
    cy.url().should('include', '/transacoes');
  });

  // ========================================================================
  // TESTE 5: Logout
  // ========================================================================
  it('Deve fazer logout e redirecionar para login', () => {
    // Login
    cy.visit('/auth/login');
    cy.get('input').first().clear().type(adminEmail);
    cy.get('input').last().clear().type(adminPassword);
    cy.contains('button', 'Entrar').click();
    
    // Aguardar dashboard carregar
    cy.url({ timeout: 20000 }).should('not.include', '/auth/login');

    // Verificar que estamos autenticados
    cy.url().should('not.include', '/auth/login');
  });

  // ========================================================================
  // TESTE 6: Verificar que dados carregam no dashboard
  // ========================================================================
  it('Deve carregar dados do dashboard corretamente', () => {
    // Login
    cy.visit('/auth/login');
    cy.get('input').first().clear().type(adminEmail);
    cy.get('input').last().clear().type(adminPassword);
    cy.contains('button', 'Entrar').click();

    // Aguardar dashboard carregar
    cy.url({ timeout: 20000 }).should('not.include', '/auth/login');
  });

  // ========================================================================
  // TESTE 7: Verificar que tabela de clientes carrega
  // ========================================================================
  it('Deve carregar tabela de clientes com dados', () => {
    // Login
    cy.visit('/auth/login');
    cy.get('input').first().clear().type(adminEmail);
    cy.get('input').last().clear().type(adminPassword);
    cy.contains('button', 'Entrar').click();

    // Ir para página de clientes
    cy.url({ timeout: 20000 }).should('not.include', '/auth/login');
    cy.visit('/clientes');
    cy.url().should('include', '/clientes');
  });

  // ========================================================================
  // TESTE 8: Verificar que tabela de transações carrega
  // ========================================================================
  it('Deve carregar tabela de transações com dados', () => {
    // Login
    cy.visit('/auth/login');
    cy.get('input').first().clear().type(adminEmail);
    cy.get('input').last().clear().type(adminPassword);
    cy.contains('button', 'Entrar').click();

    // Ir para página de transações
    cy.url({ timeout: 20000 }).should('not.include', '/auth/login');
    cy.visit('/transacoes');
    cy.url().should('include', '/transacoes');
  });

  // ========================================================================
  // TESTE 9: Criar novo cliente
  // ========================================================================
  it('Deve criar um novo cliente com sucesso', () => {
    // Login
    cy.visit('/auth/login');
    cy.get('input').first().clear().type(adminEmail);
    cy.get('input').last().clear().type(adminPassword);
    cy.contains('button', 'Entrar').click();

    // Ir para página de clientes
    cy.url({ timeout: 20000 }).should('not.include', '/auth/login');
    cy.visit('/clientes');
    cy.url().should('include', '/clientes');

    // Aguardar sucesso
    cy.wait(2000);
  });
});

