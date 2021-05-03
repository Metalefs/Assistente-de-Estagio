using ADE.Aplicacao.Services;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using Assistente_de_Estagio.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using ADE.Apresentacao.Models;

namespace Assistente_de_Estagio.Views.Shared.Components.MainLayoutViewComponent
{
    public class MainLayoutViewComponent : ViewComponent
    {
        UnitOfWork unitOfWork;
        private ServicoUsuario servicoUsuario;
        UserManager<UsuarioADE> userManager;
        public MainLayoutViewComponent(UserManager<UsuarioADE> userManager,
            ApplicationDbContext context
            ) : base()
        {
            this.userManager = userManager;
            unitOfWork = new UnitOfWork(context);
        }

        public async Task<IViewComponentResult> InvokeAsync(IHtmlContent body)
        {
            servicoUsuario = new ServicoUsuario(unitOfWork, userManager);
            UsuarioADE usuario = await servicoUsuario.ObterDetalhesUsuario(UserClaimsPrincipal);
            UserInterface UI = new UserInterface(body, usuario);
            return View(UI);
        }
    }
}
