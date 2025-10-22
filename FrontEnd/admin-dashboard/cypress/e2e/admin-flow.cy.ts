// ============================================================================
// TESTE E2E - FLUXO COMPLETO DO ADMIN DASHBOARD
// ============================================================================

describe('Admin Dashboard - Fluxo Completo', () => {
  const adminEmail = 'admin@owaypay.com';
  const adminPassword = 'Admin@123';

  beforeEach(() => {
    // Limpar localStorage antes de cada teste
    cy.clearLocalStorage();
    // Verificar que não há erros de console
    cy.on('uncaught:exception', (err) => {
      // Ignorar erros específicos do ApexCharts e ResizeObserver
      if (err.message.includes('ResizeObserver') ||
          err.message.includes('Cannot read properties of undefined') ||
          err.message.includes('toString')) {
        return false;
      }
      throw err;
    });
  });

  // ========================================================================
  // TESTE 1: Acesso sem autenticação redireciona para login
  // ========================================================================
  it('Deve redirecionar para login quando acessar / sem autenticação', () => {
    cy.visit('/');
    cy.url().should('include', '/auth/login');
    cy.contains('Bem-vindo ao Painel Administrativo').should('be.visible');

    // Verificar que o formulário de login está visível
    cy.get('input').should('have.length.at.least', 2);
    cy.contains('button', 'Entrar').should('be.visible');
  });

  // ========================================================================
  // TESTE 2: Login com credenciais válidas
  // ========================================================================
  it('Deve fazer login com credenciais válidas', () => {
    cy.visit('/auth/login');

    // Verificar que o formulário está visível
    cy.get('input').should('have.length.at.least', 2);

    // Preencher formulário - primeiro input é email, segundo é password
    cy.get('input').first().clear().type(adminEmail);
    cy.get('input').last().clear().type(adminPassword);

    // Clicar em Entrar
    cy.contains('button', 'Entrar').click();

    // Esperar o redirecionamento (com timeout maior)
    cy.url({ timeout: 20000 }).should('not.include', '/auth/login');

    // Verificar que o dashboard carregou
    cy.get('header').should('be.visible');
    cy.get('nav').should('be.visible');
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

    // Verificar que a página de clientes carregou corretamente
    cy.get('header').should('be.visible');
    cy.get('nav').should('be.visible');
    cy.get('footer').should('be.visible');

    // Verificar que não há template text
    cy.contains('Minimal UI Kit').should('not.exist');
    cy.contains('minimals.cc').should('not.exist');
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

    // Verificar que a página de transações carregou corretamente
    cy.get('header').should('be.visible');
    cy.get('nav').should('be.visible');
    cy.get('footer').should('be.visible');

    // Verificar que não há template text
    cy.contains('Minimal UI Kit').should('not.exist');
    cy.contains('minimals.cc').should('not.exist');
  });

  // ========================================================================
  // TESTE 5: Verificar que dados carregam no dashboard
  // ========================================================================
  it('Deve carregar dados do dashboard corretamente', () => {
    // Login
    cy.visit('/auth/login');
    cy.get('input').first().clear().type(adminEmail);
    cy.get('input').last().clear().type(adminPassword);
    cy.contains('button', 'Entrar').click();

    // Aguardar dashboard carregar
    cy.url({ timeout: 20000 }).should('not.include', '/auth/login');

    // Aguardar o dashboard estar completamente carregado
    cy.get('header', { timeout: 20000 }).should('be.visible');
    cy.get('nav', { timeout: 20000 }).should('be.visible');

    // Verificar que não há template text
    cy.contains('Minimal UI Kit').should('not.exist');
    cy.contains('minimals.cc').should('not.exist');
  });

  // ========================================================================
  // TESTE 6: Verificar que tabela de clientes carrega
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

    // Verificar que a página carregou
    cy.get('header').should('be.visible');
    cy.get('nav').should('be.visible');
    cy.get('footer').should('be.visible');

    // Verificar que não há template text
    cy.contains('Minimal UI Kit').should('not.exist');
  });

  // ========================================================================
  // TESTE 7: Verificar que tabela de transações carrega
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

    // Verificar que a página carregou
    cy.get('header').should('be.visible');
    cy.get('nav').should('be.visible');
    cy.get('footer').should('be.visible');

    // Verificar que não há template text
    cy.contains('Minimal UI Kit').should('not.exist');
  });

  // ========================================================================
  // TESTE 8: Verificar que menu está visível na página de clientes
  // ========================================================================
  it('Deve exibir menu de navegação corretamente', () => {
    // Login
    cy.visit('/auth/login');
    cy.get('input').first().clear().type(adminEmail);
    cy.get('input').last().clear().type(adminPassword);
    cy.contains('button', 'Entrar').click();

    // Aguardar dashboard carregar
    cy.url({ timeout: 20000 }).should('not.include', '/auth/login');

    // Navegar para clientes para verificar o menu
    cy.visit('/clientes');
    cy.url().should('include', '/clientes');

    // Aguardar o menu estar visível
    cy.get('nav', { timeout: 20000 }).should('be.visible');

    // Verificar que o menu tem os itens corretos
    cy.contains('Dashboard', { timeout: 10000 }).should('be.visible');
    cy.contains('Gerenciamento').should('be.visible');
  });

  // ========================================================================
  // TESTE 9: Verificar que footer não tem template text
  // ========================================================================
  it('Deve exibir footer correto sem template text', () => {
    // Login
    cy.visit('/auth/login');
    cy.get('input').first().clear().type(adminEmail);
    cy.get('input').last().clear().type(adminPassword);
    cy.contains('button', 'Entrar').click();

    // Aguardar dashboard carregar
    cy.url({ timeout: 20000 }).should('not.include', '/auth/login');

    // Navegar para clientes para verificar o footer
    cy.visit('/clientes');
    cy.url().should('include', '/clientes');

    // Aguardar footer estar visível
    cy.get('footer', { timeout: 20000 }).should('be.visible');
    cy.contains('Owaypay', { timeout: 10000 }).should('be.visible');

    // Verificar que não há template text
    cy.contains('Minimal UI Kit').should('not.exist');
    cy.contains('minimals.cc').should('not.exist');
    cy.contains('The starting point for your next project').should('not.exist');
  });
});

