using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using System;
using System.Threading.Tasks;

namespace NerdStore.WebApp.MVC.Controllers
{
    public class VitrineController : ControllerBase
    {
        private readonly IProdutoAppService _produtoAppService;

        public VitrineController(
            INotificationHandler<DomainNotification> notifications,
            IProdutoAppService produtoAppService, 
            IMediator _mediatorHandler, 
            IHttpContextAccessor httpContextAccessor) : base(notifications, _mediatorHandler, httpContextAccessor)
        {
            _produtoAppService = produtoAppService;
        }

        [HttpGet]
        [Route("")]
        [Route("vitrine")]
        public async Task<IActionResult> Index()
        {
            return View(await _produtoAppService.ObterTodos());
        }

        [HttpGet]
        [Route("produto-detalhe/{id}")]
        public async Task<IActionResult> ProdutoDetalhe(Guid id)
        {
            return View(await _produtoAppService.ObterPorId(id));
        }
    }
}
