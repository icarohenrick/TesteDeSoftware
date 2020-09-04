using Features.Tests;
using NerdStore.WebApp.MVC;
using NerdStore.WebApp.MVC.Models;
using NerdStore.WebApp.Tests.Config;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.Tests
{
    [TestCaseOrderer("Feature.Tests.PriorityOrderer", "Feature.Tests")]
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class PedidoAPITests
    {
        private readonly IntegrationTestsFixture<StartupApiTests> _testsFixture;
        public PedidoAPITests(IntegrationTestsFixture<StartupApiTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }


        [Fact(DisplayName = "Adicionar item em novo pedido"), TestPriority(1)]
        [Trait("Categoria", "Integração API - Pedido")]
        public async Task AdicionarItem_NovoPedido_DeveRetornarComSucesso()
        {
            // Arrange
            var itemInfo = new ItemViewModel
            {
                Id = new Guid("ccb50036-209d-4f03-a456-dc68a5d4171a"),
                Quantidade = 2
            };
            await _testsFixture.RealizarLoginAPI();

            // Act
            var postResponse = await _testsFixture.Client.PostAsJsonAsync("api/carrinho", itemInfo, _testsFixture.UsuarioToken);

            // Assert
            postResponse.EnsureSuccessStatusCode();

        }

        [Fact(DisplayName = "Remover item em pedido existente"), TestPriority(2)]
        [Trait("Categoria", "Integração API - Pedido")]
        public async Task RemoverItem_PedidoExistente_DeveRetornarComSucesso()
        {
            // Arrange
            var produtoId = new Guid("ccb50036-209d-4f03-a456-dc68a5d4171a");
            await _testsFixture.RealizarLoginAPI();

            // Act
            var deletetResponse = await _testsFixture.Client.DeleteAsync($"api/carrinho/{produtoId}", _testsFixture.UsuarioToken);

            // Assert
            deletetResponse.EnsureSuccessStatusCode();
        }
    }
}
