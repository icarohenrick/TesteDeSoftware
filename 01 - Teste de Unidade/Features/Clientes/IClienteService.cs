using System;
using System.Collections.Generic;
using System.Text;

namespace Features.Clientes
{
    public interface IClienteService
    {
        IEnumerable<Cliente> ObterTodosAtivos();
        void Adicionar(Cliente cliente);
        void Atualizar(Cliente cliente);
        void Inativar(Cliente cliente);
        void Remover(Cliente cliente);
    }
}
