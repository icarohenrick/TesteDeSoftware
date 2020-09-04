using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Features.Core
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
        public ValidationResult ValidationResult { get; protected set; }

        public virtual bool EhValido()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            var comparteTo = obj as Entity;

            if (ReferenceEquals(this, comparteTo)) return true;
            if (ReferenceEquals(null, comparteTo)) return false;

            return Id.Equals(comparteTo.Id);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b) => !(a == b);

        public override int GetHashCode() => (GetType().GetHashCode() * 666) + Id.GetHashCode();

        public override string ToString() => $"{GetType().Name} [Id= {Id}]";

        //public Entity()
        //{
        //    Id = Guid.NewGuid();
        //}
    }
}
