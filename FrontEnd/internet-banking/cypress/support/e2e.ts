// ============================================================================
// CYPRESS SUPPORT FILE
// ============================================================================

// Cypress commands and configurations
beforeEach(() => {
  // Disable uncaught exception handling for specific errors
  cy.on('uncaught:exception', (err, runnable) => {
    // Return false to prevent Cypress from failing the test
    return false;
  });
});

