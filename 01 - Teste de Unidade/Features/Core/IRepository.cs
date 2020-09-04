using System.Collections.Generic;

namespace Features.Core
{
    public interface IRepository<T>
    {
        IEnumerable<T> ObterTodos();
        void Adicionar(T entity);
        void Atualizar(T entity);
        void Remover(T entity);
        void Dispose();
    }
}
