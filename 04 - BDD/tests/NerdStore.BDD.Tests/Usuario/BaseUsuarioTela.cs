using NerdStore.BDD.Tests.Config;

namespace NerdStore.BDD.Tests.Usuario
{
    public abstract class BaseUsuarioTela : PageObjectModel
    {
        protected BaseUsuarioTela(SeleniumHelper helper) : base(helper) { }

        public void AcessarSiteLoja()
            => Helper.IrParaUrl(Helper.Configuration.DomainUrl);

        public bool ValidarSaudacaoUsuarioLogado(Usuario usuario)
        {
            var saudacao = Helper.ObterTextoElementoPorId("manage");
            return saudacao.Contains(usuario.Email);
        }

        public bool ValidarMensagemDeErroFormulario(string mensagem)
        {
            var dados = Helper.ObterTextoElementoPorClasseCss("text-danger");
            return dados.Contains(mensagem);
        }
    }
}
