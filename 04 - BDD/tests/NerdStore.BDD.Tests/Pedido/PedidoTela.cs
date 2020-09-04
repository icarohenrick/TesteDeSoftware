using NerdStore.BDD.Tests.Config;
using System;

namespace NerdStore.BDD.Tests.Pedido
{
    public class PedidoTela : PageObjectModel
    {
        public object VoltarNaveg { get; internal set; }

        public PedidoTela(SeleniumHelper helper) : base(helper) { }

        public void AcessarVitrineDeProdutos()
        {
            Helper.IrParaUrl(Helper.Configuration.VitrineUrl);
        }

        public void ObterDetalhesDoProduto(int posicao = 1)
        {
            Helper.ClicarPorXPath($"html/body/div/main/div/div/div[{posicao}]/span/a");
        }

        public bool ValidarProdutoDisponivel()
        {
            return Helper.ValidarConteudoUrl(Helper.Configuration.ProdutoUrl);
        }

        public int ObterQuantidadeNoEstoque()
        {
            var elemento = Helper.ObterElementoPorXPath("/html/body/div/main/div/div/div[2]/p[1]");
            var quantidade = elemento.Text.ApenasNumeros();

            if (char.IsNumber(quantidade.ToString(), 0)) return quantidade;

            return 0;
        }

        public void ClicarEmComprarAgora()
            => Helper.ClicarPorXPath("/html/body/div/main/div/div/div[2]/form/div[2]/button");

        public bool ValidarSeEstaNoCarrinhoDeCompras()
            => Helper.ValidarConteudoUrl(Helper.Configuration.CarrinhoUrl);

        public decimal ObterValorUnitarioProdutoCarrinho()
            => Convert.ToDecimal(Helper.ObterTextoElementoPorId("valorUnitario").Replace("R$", "").Trim());

        public decimal ObterValorTotalCarrinho()
        {
            string elementoTexto = string.Empty;
            var xPathOpcao01 = "/html/body/div/main/div/div/div/table/tbody/tr[4]/td[5]/h3/strong";
            var xPathOpcao02 = "/html/body/div/main/div/div/div/table/tbody/tr[3]/td[5]/h3/strong";

            if (Helper.ValidarSeOElementoExistePorXPath(xPathOpcao01))
                elementoTexto = Helper.ObterElementoPorXPath(xPathOpcao01).Text;
            else if (Helper.ValidarSeOElementoExistePorXPath(xPathOpcao02))
                elementoTexto = Helper.ObterElementoPorXPath(xPathOpcao02).Text;

            return Convert.ToDecimal(elementoTexto.Replace("R$", "").Trim());
        }

        public void ClicarAdicionarQuantidadeItens(int quantidade)
        {
            var botaoAdicionar = Helper.ObterElementoPorClase("btn-plus");
            if (botaoAdicionar == null) return;

            for(var i = 1; i < quantidade; i++)
            {
                botaoAdicionar.Click();
            }
        }

        public string ObterMensagemDeErroProduto()
            => Helper.ObterTextoElementoPorClasseCss("alert-danger");

        public void NavegarParaCarrinhoCompras()
            =>Helper.ObterElementoPorXPath("/html/body/header/nav/div/div/ul/li[3]/a").Click();

        public void GarantirQueOPrimeiroItemDaVitrineEstejaAdicionado()
        {
            NavegarParaCarrinhoCompras();
            if (ObterValorTotalCarrinho() > 0) return;

            AcessarVitrineDeProdutos();
            ObterDetalhesDoProduto();
            ClicarEmComprarAgora();
        }

        public string ObterIdPrimeiroProdutoCarrinho()
            => Helper.ObterElementoPorXPath("/html/body/div/main/div/div/div/table/tbody/tr[1]/td[1]/div/div/h4/a")
            .GetAttribute("href");

        public int ObterQuantidadeDeItensPrimeiroProdutoCarrinho()
            => Convert.ToInt32(Helper.ObterValorTextBoxPorId("quantidade"));
        
        public void VoltarNavegacao(int vezes = 1)
            => Helper.VoltarNavegacao(vezes);
    }
}
