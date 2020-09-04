using NerdStore.Core;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoTests
    {
		[Fact(DisplayName = "Adicionar Item Novo Pedido")]
		[Trait("Categoria", "Vendas - Pedido")]
		public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor()
		{
			// Arrange
			var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
			var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Teste", 2, 100);

			// Act
			pedido.AdicionarItem(pedidoItem);

			// Assert
			Assert.Equal(200, pedido.ValorTotal);
		}

		[Fact(DisplayName = "Adicionar Item Pedido Existente")]
		[Trait("Categoria", "Vendas - Pedido")]
		public void AdicionarItemPedido_ItemExistente_DeveIncrementarUnidadesSomarValores()
		{
			// Arrange
			var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
			var produtoId = Guid.NewGuid();

			var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 2, 100);
			pedido.AdicionarItem(pedidoItem);

			var pedidoItem2 = new PedidoItem(produtoId, "Produto Teste", 1, 100);

			// Act
			pedido.AdicionarItem(pedidoItem2);

			// Assert
			Assert.Equal(300, pedido.ValorTotal);
			Assert.Equal(1, pedido.PedidoItems.Count);
			Assert.Equal(3, pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == produtoId).Quantidade);
		}

		[Fact(DisplayName = "Adicionar Item Pedido Acima do Permitido")]
		[Trait("Categoria", "Vendas - Pedido")]
		public void AdicionarItemPedido_ItemAcimaDoPermitido_DeveRetornarException()
		{
			// Arrange
			var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
			var produtoId = Guid.NewGuid();
			var pedidoItem = new PedidoItem(produtoId, "Produto Teste", Pedido.MAX_UNIDADES_ITEM + 1, 100);

			// Act && Assert
			Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItem));
		}

		[Fact(DisplayName = "Adicionar Item Pedido Existente Acima do Permitido")]
		[Trait("Categoria", "Vendas - Pedido")]
		public void AdicionarItemPedido_ItemExistenteAcimaDoPermitido_DeveRetornarException()
		{
			// Arrange
			var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
			var produtoId = Guid.NewGuid();
			var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 1, 100);
			var pedidoItem2 = new PedidoItem(produtoId, "Produto Teste", Pedido.MAX_UNIDADES_ITEM, 100);
			pedido.AdicionarItem(pedidoItem);

			// Act && Assert
			Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItem2));
		}

		[Fact(DisplayName = "Atualizar Item Pedido Inexistente")]
		[Trait("Categoria", "Vendas - Pedido")]
		public void AtualizarItemPedido_ItemNaoExistenteNaLista_DeveRetornarException()
		{
			// Arrange
			var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
			var pedidoItemAtualizado = new PedidoItem(Guid.NewGuid(), "Produto Teste", 1, 100);

			// Act && Assert
			Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItemAtualizado));
		}

		[Fact(DisplayName = "Atualizar Item Pedido Válido")]
		[Trait("Categoria", "Vendas - Pedido")]
		public void AtualizarItemPedido_ItemValido_DeveAtualizarQuantidade()
		{
			// Arrange
			var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
			var produtoId = Guid.NewGuid();
			var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 2, 100);
			pedido.AdicionarItem(pedidoItem);
			var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Teste", 5, 100);
			var novaQuantidade = pedidoItemAtualizado.Quantidade;

			// Act
			pedido.AtualizarItem(pedidoItemAtualizado);

			// Assert
			Assert.Equal(novaQuantidade, pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == produtoId).Quantidade);
		}

		[Fact(DisplayName = "Atualizar Item Pedido Validar Total")]
		[Trait("Categoria", "Vendas - Pedido")]
		public void AtualizarItemPedido_PedidosComProdutosDiferentes_DeveAtualizarValorTotal()
		{
			// Arrange
			var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
			var produtoId = Guid.NewGuid();
			var pedidoItemExistente1 = new PedidoItem(Guid.NewGuid(), "Produto Testante", 2, 100);
			var pedidoItemExistente2 = new PedidoItem(produtoId, "Produto Testante", 3, 15);
			pedido.AdicionarItem(pedidoItemExistente1);
			pedido.AdicionarItem(pedidoItemExistente2);

			var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Teste", 5, 15);
			var totalPedido = pedidoItemExistente1.Quantidade * pedidoItemExistente1.ValorUnitario +
				pedidoItemAtualizado.Quantidade * pedidoItemAtualizado.ValorUnitario;

			// Act
			pedido.AtualizarItem(pedidoItemAtualizado);

			// Assert
			Assert.Equal(totalPedido, pedido.ValorTotal);
		}

		[Fact(DisplayName = "Atualizar Item Pedido Validar Total")]
		[Trait("Categoria", "Vendas - Pedido")]
		public void AtualizarItemPedido_ItemUnidadesAcimaDoPermitido_DeveRetornarException()
		{
			// Arrange
			var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
			var produtoId = Guid.NewGuid();
			var pedidoItemExistente1 = new PedidoItem(produtoId, "Produto Testante", 3, 15);
			pedido.AdicionarItem(pedidoItemExistente1);

			var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Teste", Pedido.MAX_UNIDADES_ITEM, 15);

			// Act & Assert
			Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItemAtualizado));
		}

		[Fact(DisplayName = "Remover Item Pedido Inexistente")]
		[Trait("Categoria", "Vendas - Pedido")]
		public void RemoverItemPedido_ItemNaoExisteNaLista_DeveRetornarException()
		{
			// Arrange
			var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
			var pedidoItemRemover = new PedidoItem(Guid.NewGuid(), "Produto Teste", 5, 100);

			// Act && Assert
			Assert.Throws<DomainException>(() => pedido.RemoverItem(pedidoItemRemover));
		}

		[Fact(DisplayName = "Remover Item Pedido Deve Calcular Valor Total")]
		[Trait("Categoria", "Vendas - Pedido")]
		public void RemoverItemPedido_ItemExistente_DeveAtualizarValorTotal()
		{
			// Arrange
			var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
			var produtoId = Guid.NewGuid();
			var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Testante", 2, 100);
			var pedidoItem2 = new PedidoItem(produtoId, "Produto Teste", 3, 15);
			pedido.AdicionarItem(pedidoItem1);
			pedido.AdicionarItem(pedidoItem2);

			var totalPedido = pedidoItem2.Quantidade * pedidoItem2.ValorUnitario;

			// Act
			pedido.RemoverItem(pedidoItem1);

			// Assert
			Assert.Equal(totalPedido, pedido.ValorTotal);
		}

		[Fact(DisplayName = "Aplicar Voucher Válido")]
		[Trait("Categoria", "Vendas - Pedido")]
		public void Pedido_AplicarVoucherValido_DeveRetornarSemErros()
		{
			// Arrange
			var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
			var voucher = new Voucher("PROMO-15-REAIS", null, 15, 1,
				TipoDescontoVoucher.Valor, DateTime.Now.AddDays(15), true, false);

			// Act
			var result = pedido.AplicarVoucher(voucher);

			// Assert
			Assert.True(result.IsValid);
		}

		[Fact(DisplayName = "Aplicar Voucher Inválido")]
		[Trait("Categoria", "Vendas - Pedido")]
		public void Pedido_AplicarVoucherInvalido_DeveRetornarComErros()
		{
			// Arrange
			var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
			var voucher = new Voucher("PROMO-15-REAIS", null, 15, 1,
				TipoDescontoVoucher.Valor, DateTime.Now.AddDays(-1), true, true);

			// Act
			var result = pedido.AplicarVoucher(voucher);

			// Assert
			Assert.False(result.IsValid);
		}

		[Fact(DisplayName = "Aplicar Voucher tipo valor desconto")]
		[Trait("Categoria", "Vendas - Pedido")]
		public void AplicarVoucher_VoucherTipoValorDesconto_DeveDescontarDoValorTotal()
		{
			// Arrange
			var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
			var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Testante", 2, 100);
			var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto Teste", 3, 15);
			pedido.AdicionarItem(pedidoItem1);
			pedido.AdicionarItem(pedidoItem2);

			var voucher = new Voucher("PROMO-15-REAIS", null, 15, 1,
				TipoDescontoVoucher.Valor, DateTime.Now.AddDays(10), true, false);

			var valorComDesconto = pedido.ValorTotal - voucher.ValorDesconto;

			// Act
			pedido.AplicarVoucher(voucher);

			// Assert
			Assert.Equal(valorComDesconto, pedido.ValorTotal);
		}

		[Fact(DisplayName = "Aplicar Voucher tipo percentual desconto")]
		[Trait("Categoria", "Vendas - Pedido")]
		public void AplicarVoucher_VoucherTipoPercentualDesconto_DeveDescontarDoValorTotal()
		{
			// Arrange
			var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
			var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Testante", 2, 100);
			var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto Teste", 3, 15);
			pedido.AdicionarItem(pedidoItem1);
			pedido.AdicionarItem(pedidoItem2);

			var voucher = new Voucher("PROMO-15-REAIS", 15, null, 1,
				TipoDescontoVoucher.Porcentagem, DateTime.Now.AddDays(10), true, false);

			var valorDesconto = (pedido.ValorTotal * voucher.PercentualDesconto) / 100;
			var valorComDesconto = pedido.ValorTotal - valorDesconto;

			// Act
			pedido.AplicarVoucher(voucher);

			// Assert
			Assert.Equal(valorComDesconto, pedido.ValorTotal);
		}

		[Fact(DisplayName = "Aplicar Voucher desconto Excede valor total")]
		[Trait("Categoria", "Vendas - Pedido")]
		public void AplicarVoucher_DescontoExcedeValorTotalPedido_PedidoDeveTerValorZero()
		{
			// Arrange
			var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
			var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Testante", 2, 100);
			pedido.AdicionarItem(pedidoItem);

			var voucher = new Voucher("PROMO-300-REAIS", null, 300, 1,
				TipoDescontoVoucher.Valor, DateTime.Now.AddDays(10), true, false);

			// Act
			pedido.AplicarVoucher(voucher);

			// Assert
			Assert.Equal(0, pedido.ValorTotal);
		}

		[Fact(DisplayName = "Aplicar Voucher Recalcular desconto na modificação do pedido")]
		[Trait("Categoria", "Vendas - Pedido")]
		public void AplicarVoucher_ModificarItensPedido_DeveCalcularDescontoValorTotal()
		{
			// Arrange
			var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
			var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Testante", 2, 100);
			pedido.AdicionarItem(pedidoItem);

			var voucher = new Voucher("PROMO-15-OFF", null, 50, 1,
				TipoDescontoVoucher.Valor, DateTime.Now.AddDays(10), true, false);
			pedido.AplicarVoucher(voucher);

			var outroPedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Teste", 4, 25);
			// Act
			pedido.AdicionarItem(outroPedidoItem);

			// Assert
			var totalEsperado = pedido.PedidoItems.Sum(i => i.Quantidade * i.ValorUnitario) - voucher.ValorDesconto;
			Assert.Equal(totalEsperado, pedido.ValorTotal);
		}
	}
}
