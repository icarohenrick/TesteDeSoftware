using NerdStore.BDD.Tests.Config;
using NerdStore.BDD.Tests.Usuario;
using System;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.BDD.Tests.Pedido
{
    [Binding]
    [CollectionDefinition(nameof(AutomocaoWebFixtureCollection))]
    public class Pedido_AdicionarItemAoCarrinhoSteps
    {

        private readonly AutomocaoWebTestsFixture _testsFixture;
        private readonly PedidoTela _pedidoTela;
        private readonly LoginUsuarioTela _loginUsuarioTela;

        private string _urlProduto;

        public Pedido_AdicionarItemAoCarrinhoSteps(AutomocaoWebTestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
            _pedidoTela = new PedidoTela(testsFixture.BrowserHelper);
            _loginUsuarioTela = new LoginUsuarioTela(testsFixture.BrowserHelper);
        }

        [Given(@"O usuário esteja logado")]
        public void DadoOUsuarioEstejaLogado()
        {
            // Arrange
            var usuario = new Usuario.Usuario
            {
                Email = "teste@teste.com",
                Senha = "Teste@123"
            };
            _testsFixture.Usuario = usuario;

            // Act
            var login = _loginUsuarioTela.Login(usuario);

            // Assert
            Assert.True(login);
        }

        [Given(@"que um produto esteja na vitrine")]
        public void DadoQueUmProdutoEstejaNaVitrine()
        {
            // Arrange
            _pedidoTela.AcessarVitrineDeProdutos();

            // Act
            _pedidoTela.ObterDetalhesDoProduto();
            _urlProduto = _pedidoTela.ObterUrl();

            // Assert
            Assert.True(_pedidoTela.ValidarProdutoDisponivel());
        }

        [Given(@"Esteja disponível no estoque")]
        public void DadoEstejaDisponivelNoEstoque()
        {
            // Assert
            Assert.True(_pedidoTela.ObterQuantidadeNoEstoque() > 0);
        }

        [When(@"O usuário adicionar uma unidade ao carrinho")]
        public void QuandoOUsuarioAdicionarUmaUnidadeAoCarrinho()
        {
            // Act
            _pedidoTela.ClicarEmComprarAgora();
        }

        [Then(@"O usuário será redirecionado ao resumo da compra")]
        public void EntaoOUsuarioSeraRedirecionadoAoResumoDaCompra()
        {
            // Assert
            Assert.True(_pedidoTela.ValidarSeEstaNoCarrinhoDeCompras());
        }

        [Then(@"O valor total do pedido será exatamente o valor do item adicionado")]
        public void EntaoOValorTotalDoPedidoSeraExatamenteOValorDoItemAdicionado()
        {
            // Arrange
            var valorUnitario = _pedidoTela.ObterValorUnitarioProdutoCarrinho();
            var valorCarrinho = _pedidoTela.ObterValorTotalCarrinho();

            // Assert
            Assert.Equal(valorUnitario, valorCarrinho);
        }

        [When(@"O usuário adicionar um item acima da quantidade máxima permitida")]
        public void QuandoOUsuarioAdicionarUmItemAcimaDaQuantidadeMaximaPermitida()
        {
            // Arrange
            _pedidoTela.ClicarAdicionarQuantidadeItens(Vendas.Domain.Pedido.MAX_UNIDADES_ITEM + 1);

            // Act
            _pedidoTela.ClicarEmComprarAgora();
        }

        [Then(@"Receberá uma mensagem de erro mencionando que foi ultrapassada a quantidade limite")]
        public void EntaoReceberaUmaMensagemDeErroMencionandoQueFoiUltrapassadaAQuantidadeLimite()
        {
            // Arrange
            var mensagem = _pedidoTela.ObterMensagemDeErroProduto();

            // Assert
            Assert.Contains($"A quantidade máxima de um item é {Vendas.Domain.Pedido.MAX_UNIDADES_ITEM}", mensagem);
        }

        [Given(@"O mesmo produto já tenha sido adicionado ao carrinho anteriormente")]
        public void DadoOMesmoProdutoJaTenhaSidoAdicionadoAoCarrinhoAnteriormente()
        {
            // Act
            _pedidoTela.NavegarParaCarrinhoCompras();
            _pedidoTela.GarantirQueOPrimeiroItemDaVitrineEstejaAdicionado();
            var produtoId = _pedidoTela.ObterIdPrimeiroProdutoCarrinho();

            // Assert
            Assert.Contains(_urlProduto, produtoId);
            Assert.True(_pedidoTela.ValidarSeEstaNoCarrinhoDeCompras());
            Assert.True(_pedidoTela.ObterQuantidadeDeItensPrimeiroProdutoCarrinho() >= 1);

            _pedidoTela.VoltarNavegacao();
        }

        [Then(@"A quantidade de itens daquele produto terá sido acrescida em uma unidade a mais")]
        public void EntaoAQuantidadeDeItensDaqueleProdutoTeraSidoAcrescidaEmUmaUnidadeAMais()
        {
            // Assert
            Assert.True(_pedidoTela.ObterQuantidadeDeItensPrimeiroProdutoCarrinho() > 1);
        }

        [Then(@"O valor total do pedido será a multiplicação da quantidade de itens pelo valor unitário")]
        public void EntaoOValorTotalDoPedidoSeraAMultiplicacaoDaQuantidadeDeItensPeloValorUnitario()
        {
            // Arrange
            var valorUnitario = _pedidoTela.ObterValorUnitarioProdutoCarrinho();
            var valorCarrinho = _pedidoTela.ObterValorTotalCarrinho();
            var quantidadeUnidades = _pedidoTela.ObterQuantidadeDeItensPrimeiroProdutoCarrinho();

            // Assert
            Assert.Equal(valorUnitario * quantidadeUnidades, valorCarrinho);
        }

        [When(@"O usuário adicionar a quantidade máxima permitida ao carrinho")]
        public void QuandoOUsuarioAdicionarAQuantidadeMaximaPermitidaAoCarrinho()
        {
            // Arrange
            // Act
            // Assert
        }

    }
}
