
using Features.Clientes;
using FluentAssertions;
using FluentAssertions.Extensions;
using MediatR;
using Moq;
using System.Linq;
using System.Threading;
using Xunit;

namespace Features.Tests
{
    [Collection(nameof(ClienteAutoMockerCollection))]
    public class ClienteServiceFluentAssertionsTests
    {
        readonly ClienteTestsAutoMockerFixture _clienteTestsAutoMockerFixture;
        readonly ClienteService _clienteService;

        public ClienteServiceFluentAssertionsTests(ClienteTestsAutoMockerFixture clienteTestsAutoMockerFixture)
        {
            _clienteTestsAutoMockerFixture = clienteTestsAutoMockerFixture;
            _clienteService = _clienteTestsAutoMockerFixture.ObterClienteService();
        }

        [Fact(DisplayName = "Adicionar Cliente com Sucesso")]
        [Trait("Categoria", "Cliente Service Fluent Assertion Tests")]
        public void ClienteService_Adicionar_DeveExecutarComSucesso()
        {
            // Arrange
            var cliente = _clienteTestsAutoMockerFixture.GerarClienteValido();

            // Act
            _clienteService.Adicionar(cliente);

            //Assert
            //Assert.True(cliente.EhValido());

            //Assert
            cliente.EhValido().Should().BeTrue();

            _clienteTestsAutoMockerFixture.Mocker.GetMock<IClienteRepository>().Verify(r => r.Adicionar(cliente), Times.Once);
            _clienteTestsAutoMockerFixture.Mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }

        [Fact(DisplayName = "Adicionar Cliente com Falha")]
        [Trait("Categoria", "Cliente Service Fluent Assertion Tests")]
        public void ClienteService_Adicionar_DeveFalharDevidoClienteInvalido()
        {
            // Arrange
            var cliente = _clienteTestsAutoMockerFixture.GerarClienteInvalido();

            // Act
            _clienteService.Adicionar(cliente);

            //Assert
            //Assert.False(cliente.EhValido());

            //Assert
            cliente.EhValido().Should().BeFalse("Possui inconsistências");

            _clienteTestsAutoMockerFixture.Mocker.GetMock<IClienteRepository>().Verify(r => r.Adicionar(cliente), Times.Never);
            _clienteTestsAutoMockerFixture.Mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never);
        }

        [Fact(DisplayName = "Obter Clientes Ativos")]
        [Trait("Categoria", "Cliente Service Fluent Assertion Tests")]
        public void ClienteService_Obter_DeveRetornarApenasClientesAtivos()
        {
            _clienteTestsAutoMockerFixture.Mocker.GetMock<IClienteRepository>().Setup(c => c.ObterTodos())
                .Returns(_clienteTestsAutoMockerFixture.ObterClientesVariados());

            //Act
            var clientes = _clienteService.ObterTodosAtivos();

            //Assert
            //Assert.True(clientes.Any());
            //Assert.False(clientes.Count(c => !c.Ativo) > 0);

            clientes.Should().HaveCountGreaterOrEqualTo(1).And.OnlyHaveUniqueItems();
            clientes.Should().NotContain(c => !c.Ativo);

            _clienteTestsAutoMockerFixture.Mocker.GetMock<IClienteRepository>().Verify(r => r.ObterTodos(), Times.Once);

            _clienteService.ExecutionTimeOf(c => c.ObterTodosAtivos())
                .Should()
                .BeLessOrEqualTo(50.Milliseconds(), "Porque é executado milhares de vezes por segundo");
        }
    }
}
