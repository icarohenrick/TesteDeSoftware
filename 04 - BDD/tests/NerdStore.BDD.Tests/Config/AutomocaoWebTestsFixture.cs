using Bogus;
using Xunit;

namespace NerdStore.BDD.Tests.Config
{
    [CollectionDefinition(nameof(AutomocaoWebFixtureCollection))]
    public class AutomocaoWebFixtureCollection : ICollectionFixture<AutomocaoWebTestsFixture> { }

    public class AutomocaoWebTestsFixture
    {
        public SeleniumHelper BrowserHelper;
        public readonly ConfigurationHelper Configuration;

        public Usuario.Usuario Usuario;

        public AutomocaoWebTestsFixture()
        {
            Usuario = new Usuario.Usuario();
            Configuration = new ConfigurationHelper();
            BrowserHelper = new SeleniumHelper(Browser.Chrome, Configuration, false);
        }

        public void GerarDadosUsuario()
        {
            var faker = new Faker("pt_BR");
            Usuario.Email = faker.Internet.Email().ToLower();
            Usuario.Senha = faker.Internet.Password(8, false, "", "@1Ab_");
        }
    }
}
