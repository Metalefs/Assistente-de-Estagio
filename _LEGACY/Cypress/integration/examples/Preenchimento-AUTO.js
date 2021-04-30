describe('My First Test', function() {
    cy.visit('https://localhost:44359/')
	    // Get an input, type into it and verify that the value has been updated
    cy.get('#Nome-Do-Aluno')
      .type('Metalefs')
      .should('have.value', 'Metalefs')
    })
})