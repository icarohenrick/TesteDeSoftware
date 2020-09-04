using NerdStore.Core.DomainObjects.DTO;
using System;
using System.Collections.Generic;
using System.Data;

namespace NerdStore.Core.Messages.CommonMessages.IntegrationEvents
{
    public class PedidoProcessamentoCanceladoEvent : Event
    {
        public PedidoProcessamentoCanceladoEvent(Guid pedidoId, Guid clientId, ListaProdutosPedido produtosPedido)
        {
            AggregateId = pedidoId;
            PedidoId = pedidoId;
            ClienteId = clientId;
            ProdutosPedido = produtosPedido;
        }

        public Guid PedidoId { get; private set; }
        public Guid ClienteId { get; private set; }
        public ListaProdutosPedido ProdutosPedido { get; private set; }
    }
}
