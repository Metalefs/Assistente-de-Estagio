using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ADE.Dominio.Models.Individuais;
using Assistente_de_Estagio.Areas.Shared;
using Assistente_de_Estagio.Data;
using ADE.Infra.Data.UOW;
using Microsoft.AspNetCore.Identity;
using Assistente_de_Estagio.Areas.Acesso.Models;
using ADE.Utilidades.Extensions;

namespace Assistente_de_Estagio.Areas.Acesso.Notificacoes.Components.VisualizacaoNotificacaoViewComponent
{
    public class VisualizacaoNotificacaoViewComponent : ViewComponent
    {
        static UnitOfWork unitOfWork;
        ApplicationDbContext context;
        NotificacaoController NotificacaoController;

        public VisualizacaoNotificacaoViewComponent(
            ApplicationDbContext _context,
            UserManager<UsuarioADE> userManager,
            SignInManager<UsuarioADE> signInManager
        )
        {
            context = _context;
            unitOfWork = new UnitOfWork(_context);
            NotificacaoController = new NotificacaoController(userManager, signInManager, _context);
        }

        public async Task<IViewComponentResult> InvokeAsync(EnumTipoNotificacao TipoNotificacao = EnumTipoNotificacao.Ambos, object Notificacao = null)
        {
            string view = TipoNotificacao.GetDescription();
            switch(TipoNotificacao)
            {
                case EnumTipoNotificacao.Geral:
                    return View(view,(Notificacao)Notificacao);

                case EnumTipoNotificacao.Individual:
                    return View(view,(NotificacaoIndividual)Notificacao);

                default:
                    UsuarioADE usuario = await NotificacaoController.ObterUsuarioLogado();
                    NotificacaoViewmodel model = new NotificacaoViewmodel();
                    if (usuario.ReceberNotificacaoFocado() || usuario.ReceberNotificacaoGeral())
                    {
                        model.NotificacacoesIndividuais = await NotificacaoController.ListaNotificacoesUsuario(usuario, unitOfWork);
                    }
                    else if (!usuario.ReceberNotificacaoFocado())
                    {
                        model.NotificacacoesGerais = await NotificacaoController.NotificacoesGeraisUsuario(unitOfWork, usuario);
                    }
                    return View(view,model);
            }

        }

    }
}
