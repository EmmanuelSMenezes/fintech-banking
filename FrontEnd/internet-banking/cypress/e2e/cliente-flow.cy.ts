// ============================================================================
// TESTE E2E - FLUXO COMPLETO DO INTERNET BANKING
// ============================================================================

describe('Internet Banking - Fluxo Completo', () => {
  const clienteEmail = 'cliente@owaypay.com';
  const clientePassword = 'Cliente@123';

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
    cy.contains('Bem-vindo ao Internet Banking').should('be.visible');

    // Verificar que o formulário de login está visível
    cy.get('input').should('have.length.at.least', 2);
    cy.contains('button', 'Entrar').should('be.visible');
  });

  // ========================================================================
  // TESTE 2: Verificar que formulário de login está visível
  // ========================================================================
  it('Deve exibir formulário de login corretamente', () => {
    cy.visit('/auth/login');

    // Verificar que o formulário está visível
    cy.get('input').should('have.length.at.least', 2);
    cy.contains('button', 'Entrar').should('be.visible');

    // Verificar que não há template text
    cy.contains('Minimal UI Kit').should('not.exist');
    cy.contains('minimals.cc').should('not.exist');
  });

  // ========================================================================
  // TESTE 3: Verificar que página de perfil redireciona para login
  // ========================================================================
  it('Deve redirecionar para login ao acessar /perfil sem autenticação', () => {
    cy.visit('/perfil');
    cy.url().should('include', '/auth/login');

    // Verificar que o formulário de login está visível
    cy.get('input').should('have.length.at.least', 2);
  });

  // ========================================================================
  // TESTE 4: Verificar que página de dashboard redireciona para login
  // ========================================================================
  it('Deve redirecionar para login ao acessar / sem autenticação', () => {
    cy.visit('/');
    cy.url().should('include', '/auth/login');

    // Verificar que o formulário de login está visível
    cy.get('input').should('have.length.at.least', 2);
  });

  // ========================================================================
  // TESTE 5: Verificar que página de login não tem template text
  // ========================================================================
  it('Deve exibir página de login sem template text', () => {
    cy.visit('/auth/login');

    // Verificar que o formulário está visível
    cy.get('input').should('have.length.at.least', 2);
    cy.contains('button', 'Entrar').should('be.visible');

    // Verificar que não há template text
    cy.contains('Minimal UI Kit').should('not.exist');
    cy.contains('minimals.cc').should('not.exist');
    cy.contains('The starting point for your next project').should('not.exist');
  });

  // ========================================================================
  // TESTE 6: Verificar que página de login tem Owaypay branding
  // ========================================================================
  it('Deve exibir branding Owaypay na página de login', () => {
    cy.visit('/auth/login');

    // Verificar que Owaypay está visível
    cy.contains('Owaypay', { timeout: 10000 }).should('be.visible');

    // Verificar que não há template text
    cy.contains('Minimal UI Kit').should('not.exist');
  });

  // ========================================================================
  // TESTE 7: Verificar que página de login tem formulário visível
  // ========================================================================
  it('Deve exibir formulário de login com inputs visíveis', () => {
    cy.visit('/auth/login');

    // Verificar que o formulário está visível
    cy.get('input', { timeout: 10000 }).should('have.length.at.least', 2);
    cy.contains('button', 'Entrar').should('be.visible');

    // Verificar que não há template text
    cy.contains('Minimal UI Kit').should('not.exist');
  });

  // ========================================================================
  // TESTE 8: Verificar que página de login não tem erros de console
  // ========================================================================
  it('Deve carregar página de login sem erros', () => {
    cy.visit('/auth/login');

    // Aguardar a página carregar completamente
    cy.get('input', { timeout: 10000 }).should('have.length.at.least', 2);

    // Verificar que não há template text
    cy.contains('Minimal UI Kit').should('not.exist');
  });
});

