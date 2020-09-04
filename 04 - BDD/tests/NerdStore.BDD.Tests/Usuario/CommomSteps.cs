using NerdStore.BDD.Tests.Config;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.BDD.Tests.Usuario
{
    [Binding]
    [CollectionDefinition(nameof(AutomocaoWebFixtureCollection))]
    public class CommomSteps
    {
        private readonly AutomocaoWebTestsFixture _testsFixture;
        private readonly CadastroDeUsuarioTela _cadastroDeUsuarioTela;

        public CommomSteps(AutomocaoWebTestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
            _cadastroDeUsuarioTela = new CadastroDeUsuarioTela(testsFixture.BrowserHelper);
        }

        [Given(@"Que um visitante está acessando o site da loja")]
        public void DadoQueUmVisitanteEstaAcessandoOSiteDaLoja()
        {
            //act
            _cadastroDeUsuarioTela.AcessarSiteLoja();

            //Assert
            Assert.Contains(_testsFixture.Configuration.DomainUrl, _cadastroDeUsuarioTela.ObterUrl());
        }

        [Then(@"Ele será redirecionado para a vitrine")]
        public void EntaoEleSeraRedirecionadoParaAVitrine()
        {
            // Assert
            Assert.Equal(_testsFixture.Configuration.VitrineUrl, _cadastroDeUsuarioTela.ObterUrl());
        }

        [Then(@"Uma saudação com seu e-mail será exibida no menu superior")]
        public void EntaoUmaSaudacaoComSeuE_MailSeraExibidaNoMenuSuperior()
        {
            // Assert
            Assert.True(_cadastroDeUsuarioTela.ValidarSaudacaoUsuarioLogado(_testsFixture.Usuario));
        }
    }
}
