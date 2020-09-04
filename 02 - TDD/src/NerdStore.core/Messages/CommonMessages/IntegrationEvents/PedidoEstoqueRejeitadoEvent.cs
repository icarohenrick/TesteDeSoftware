using System;

namespace NerdStore.Core.Messages.CommonMessages.IntegrationEvents
{
    public class PedidoEstoqueRejeitadoEvent : IntegrationEvent
    {
        public Guid PedidoId { get; set; }
        public Guid ClienteId { get; set; }

        public PedidoEstoqueRejeitadoEvent(Guid pedidoId, Guid clienteId)
        {
            AggregateId = pedidoId;
            PedidoId = pedidoId;
            ClienteId = clienteId;
        }
    }
}