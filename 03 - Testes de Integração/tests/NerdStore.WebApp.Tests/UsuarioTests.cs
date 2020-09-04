﻿using Features.Tests;
using NerdStore.WebApp.MVC;
using NerdStore.WebApp.Tests.Config;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.Tests
{
    [TestCaseOrderer("Features.Tests.PriorityOrderer", "Features.Tests")]
    [Collection(nameof(IntegrationWebTestsFixtureCollection))]
    public class UsuarioTests
    {
        private readonly IntegrationTestsFixture<StartupWebTests> _testsFixture;

        public UsuarioTests(IntegrationTestsFixture<StartupWebTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }

		[Fact(DisplayName = "Realizar cadastro com sucesso"), TestPriority(1)]
		[Trait("Categoria", "Integração Web - Usuário")]
		public async Task Usuario_RealizarCadastro_DeveExecutarComSucesso()
		{
            // Arrange
            var initialResponse = await _testsFixture.Client.GetAsync("/Identity/Account/Register");
            initialResponse.EnsureSuccessStatusCode();

            var antiForgeryToken = _testsFixture.ObterAntiForgeryToken(await initialResponse.Content.ReadAsStringAsync());

            _testsFixture.GerarUserSenha();

            var formData = new Dictionary<string, string>
            {
                {_testsFixture.AntiForgeryFieldName, antiForgeryToken },
                {"Input.Email",  _testsFixture.UsuarioEmail},
                {"Input.Password", _testsFixture.UsuarioSenha },
                {"Input.ConfirmPassword", _testsFixture.UsuarioSenha }
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Identity/Account/Register")
            {
                Content = new FormUrlEncodedContent(formData)
            };

            // Act
            var postResponse = await _testsFixture.Client.SendAsync(postRequest);

            // Assert
            var responseStream = await postResponse.Content.ReadAsStreamAsync();

            string responseString = new StreamReader(responseStream).ReadToEnd();

            postResponse.EnsureSuccessStatusCode();
            Assert.Contains($"Hello {_testsFixture.UsuarioEmail}!", responseString);
		}

        [Fact(DisplayName = "Realizar cadastro com sucesso"), TestPriority(3)]
        [Trait("Categoria", "Integração Web - Usuário")]
        public async Task Usuario_RealizarCadastroComSenhaFraca_DeveRetornarMensagemDeErro()
        {
            // Arrange
            var initialResponse = await _testsFixture.Client.GetAsync("/Identity/Account/Register");
            initialResponse.EnsureSuccessStatusCode();

            var antiForgeryToken = _testsFixture.ObterAntiForgeryToken(await initialResponse.Content.ReadAsStringAsync());

            _testsFixture.GerarUserSenha();
            const string senhaFraca = "123456";

            var formData = new Dictionary<string, string>
            {
                {_testsFixture.AntiForgeryFieldName, antiForgeryToken },
                {"Input.Email",  _testsFixture.UsuarioEmail},
                {"Input.Password", senhaFraca },
                {"Input.ConfirmPassword", senhaFraca }
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Identity/Account/Register")
            {
                Content = new FormUrlEncodedContent(formData)
            };

            // Act
            var postResponse = await _testsFixture.Client.SendAsync(postRequest);

            // Assert
            var responseStream = await postResponse.Content.ReadAsStreamAsync();

            string responseString = new StreamReader(responseStream).ReadToEnd();

            postResponse.EnsureSuccessStatusCode();

            var MensagensDeErroDeSenha = new string[]
            {
                "Passwords must have at least one non alphanumeric character.",
                "Passwords must have at least one lowercase (&#x27;a&#x27;-&#x27;z&#x27;).",
                "Passwords must have at least one uppercase (&#x27;A&#x27;-&#x27;Z&#x27;)."
            };

            foreach (var mensagem in MensagensDeErroDeSenha)
            {
                Assert.Contains(mensagem , responseString);
            }
        }

        [Fact(DisplayName = "Realizar Login com sucesso"), TestPriority(2)]
        [Trait("Categoria", "Integração Web - Usuário")]
        public async Task Usuario_RealizarLogin_DeveExecutarComSucesso()
        {
            // Arrange
            var initialResponse = await _testsFixture.Client.GetAsync("/Identity/Account/Login");
            initialResponse.EnsureSuccessStatusCode();

            var antiForgeryToken = _testsFixture.ObterAntiForgeryToken(await initialResponse.Content.ReadAsStringAsync());

            var formData = new Dictionary<string, string>
            {
                {_testsFixture.AntiForgeryFieldName, antiForgeryToken },
                {"Input.Email",  _testsFixture.UsuarioEmail},
                {"Input.Password", _testsFixture.UsuarioSenha }
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Identity/Account/Login")
            {
                Content = new FormUrlEncodedContent(formData)
            };

            // Act
            var postResponse = await _testsFixture.Client.SendAsync(postRequest);

            // Assert
            var responseStream = await postResponse.Content.ReadAsStreamAsync();

            string responseString = new StreamReader(responseStream).ReadToEnd();

            postResponse.EnsureSuccessStatusCode();
            Assert.Contains($"Hello {_testsFixture.UsuarioEmail}!", responseString);
        }
    }
}