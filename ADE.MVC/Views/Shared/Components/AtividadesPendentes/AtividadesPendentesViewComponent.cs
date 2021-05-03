using ADE.Dominio.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Assistente_de_Estagio.Data;
using ADE.Infra.Data.UOW;
using ADE.Aplicacao.Services;
using ADE.Dominio.Models.Individuais;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Assistente_de_Estagio.Areas.Principal.Views.RegistroHoras.Components.AtividadesPendentes;

namespace Assistente_de_Estagio.Areas.Principal.Views.RegistroHoras.Components.AtividadesPendentesViewComponent
{
    public class AtividadesPendentesViewComponent : ViewComponent
    {
        UnitOfWork unitOfWork;
        ServicoAtividadeUsuario ServicoAtividadeUsuario;
        ServicoAtividadeEstagio ServicoAtividadeEstagio;
        ServicoLogins ServicoLogins;
        ServicoUsuario ServicoUsuario;

        public AtividadesPendentesViewComponent(ApplicationDbContext context, UserManager<UsuarioADE> userManager)
        {
            unitOfWork = new UnitOfWork(context);
            ServicoAtividadeUsuario = new ServicoAtividadeUsuario(ref unitOfWork);
            ServicoAtividadeEstagio = new ServicoAtividadeEstagio(ref unitOfWork);
            ServicoLogins = new ServicoLogins(ref unitOfWork);
            ServicoUsuario = new ServicoUsuario(unitOfWork, userManager);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            AtividadesPendentesVM model;
            try
            {
                UsuarioADE usuario = await ServicoUsuario.ObterUsuarioLogado(UserClaimsPrincipal);
                Logins Login = await ServicoLogins.FiltrarUltimo(x => x.IdUsuario == usuario.Id);
                List<AtividadeUsuario> Registros = await ServicoAtividadeUsuario.Filtrar(x=>x.IdUsuario == usuario.Id && x.Concluido == false);
                List<AtividadeEstagio> AtividadesEstagio = await ServicoAtividadeEstagio.BuscarPendentesPorCursoDoUsuario(usuario);
                model = new AtividadesPendentesVM(Registros, AtividadesEstagio, Login);
            }
            catch (System.Exception)
            {
                model = new AtividadesPendentesVM(new List<AtividadeUsuario>(), new List<AtividadeEstagio>(), new Logins());
            }
            return View(model);
        }
    }
}
