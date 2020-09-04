using Features.Clientes;
using System;
using Xunit;

namespace Features.Tests
{
    public class ClienteTests
    {
        [Fact(DisplayName ="Novo Cliente Válido")]
        [Trait("Categoria", "Cliente Traits Testes")]
        public void Cliente_NovoCliente_DeveEstarValido()
        {
            //Arrange
            var cliente = new Cliente(
                Guid.NewGuid(),
                "Patrizia",
                "Mastrodonato",
                DateTime.Now.AddYears(-22),
                "patrizia.mastrodonato@pati.adv",
                true,
                DateTime.Now);

            //Act
            var result = cliente.EhValido();

            //Assert
            Assert.True(result);
            Assert.Equal(0, cliente.ValidationResult.Errors.Count);
        }

        [Fact(DisplayName = "Novo Cliente Inválido")]
        [Trait("Categoria", "Cliente Traits Testes")]
        public void Cliente_NovoCliente_DeveEstarInValido()
        {
            //Arrange
            var cliente = new Cliente(
                Guid.NewGuid(),
                "Patrizia",
                "Mastrodonato",
                DateTime.Now,
                "patrizia.mastrodonato@pati.adv",
                true,
                DateTime.Now);

            //Act
            var result = cliente.EhValido();

            //Assert
            Assert.False(result);
            Assert.NotEqual(0, cliente.ValidationResult.Errors.Count);
        }
    }
}
