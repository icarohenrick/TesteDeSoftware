using NerdStore.Core.Messages;
using System;
using System.Collections.Generic;

namespace NerdStore.Core.DomainObjects
{
    public abstract class Entity
    {
        public Guid Id { get; set; }

        private List<Event> _notifications;
        public IReadOnlyCollection<Event> Notificacoes => _notifications?.AsReadOnly();

        public Entity()
        {
            Id = Guid.NewGuid();
        }

        public void AdicionarEvento(Event evento)
        {
            _notifications ??= new List<Event>();
            _notifications.Add(evento);
        }

        public void RemoverEvento(Event eventItem)
        {
            _notifications?.Remove(eventItem);
        }

        public void LimparEventos()
        {
            _notifications?.Clear();
        }
    }
}
