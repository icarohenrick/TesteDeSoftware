using NerdStore.BDD.Tests.Config;
using System;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.BDD.Tests.Usuario
{
    [Binding]
    [CollectionDefinition(nameof(AutomocaoWebFixtureCollection))]
    public class Usuario_LoginSteps
    {
        private readonly LoginUsuarioTela _loginUsuarioTela;
        private readonly AutomocaoWebTestsFixture _testsFixture;

        public Usuario_LoginSteps(AutomocaoWebTestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
            _loginUsuarioTela = new LoginUsuarioTela(testsFixture.BrowserHelper);
        }

        [When(@"Ele clicar em login")]
        public void QuandoEleClicarEmLogin()
        {
            // Act
            _loginUsuarioTela.ClicarNoLogin();

            // Assert
            Assert.Contains(_testsFixture.Configuration.LoginUrl, _loginUsuarioTela.ObterUrl());
        }
        
        [When(@"Preencher os dados do formulário de login")]
        public void QuandoPreencherOsDadosDoFormularioDeLogin()
        {
            // Arrange
            var usuario = new Usuario
            {
                Email = "teste@teste.com",
                Senha = "Teste@123"
            };
            _testsFixture.Usuario = usuario;

            // Act
            _loginUsuarioTela.PreencherFormularioLogin(usuario);

            // Assert
            Assert.True(_loginUsuarioTela.ValidarPreenchimentoFormularioLogin(usuario));
        }
        
        [When(@"Clicar no botão login")]
        public void QuandoClicarNoBotaoLogin()
        {
            // Act
            _loginUsuarioTela.ClicarNoBotaoLogin();

            // Assert
            Assert.True(_loginUsuarioTela.ValidarSaudacaoUsuarioLogado(_testsFixture.Usuario));
        }
    }
}
