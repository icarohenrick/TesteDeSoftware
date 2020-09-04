using Features.Clientes;
using System;
using Xunit;

namespace Features.Tests
{
    [CollectionDefinition(nameof(ClienteCollection))]
    public class ClienteCollection : ICollectionFixture<ClienteTestsFixture> { }

    public class ClienteTestsFixture : IDisposable
    {
        public Cliente GerarClienteValido()
        {
            var cliente = new Cliente(
                Guid.NewGuid(),
                "Patrizia",
                "Mastrodonato",
                DateTime.Now.AddYears(-22),
                "patrizia.mastrodonato@pati.adv",
                true,
                DateTime.Now);

            return cliente;
        }

        public Cliente GerarClienteInvalido()
        {
            var cliente = new Cliente(
                Guid.NewGuid(),
                "Patrizia",
                "Mastrodonato",
                DateTime.Now,
                "patrizia.mastrodonato@pati.adv",
                true,
                DateTime.Now);

            return cliente;
        }

        public void Dispose() {}
    }
}
