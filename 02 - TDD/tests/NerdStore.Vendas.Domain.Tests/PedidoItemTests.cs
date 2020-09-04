using NerdStore.Core;
using System;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoItemTests
    {
		[Fact(DisplayName = "Novo Item Pedido com unidadade Abaixo do Permitido")]
		[Trait("Categoria", "Vendas - Pedido Item")]
		public void AdicionarItemPedido_ItemAbaixoDoPermitido_DeveRetornarException()
		{
			// Arrange && Act && Assert
			Assert.Throws<DomainException>(() => new PedidoItem(Guid.NewGuid(), "Produto Teste", Pedido.MIN_UNIDADES_ITEM - 1, 100));
		}
	}
}
