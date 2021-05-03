using ADE.Aplicacao.Services;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using Assistente_de_Estagio.Areas.Shared;
using Assistente_de_Estagio.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Views.Shared.Components.ModalTermosCompromissoViewComponent
{
    public class ModalTermosCompromissoViewComponent : ViewComponent
    {
        UnitOfWork unitOfWork;
        ServicoTermoCompromisso ServicoTermoCompromisso;

        public ModalTermosCompromissoViewComponent(ApplicationDbContext context, UserManager<UsuarioADE> userManager)
        {
            unitOfWork = new UnitOfWork(context);
            ServicoTermoCompromisso = new ServicoTermoCompromisso(ref unitOfWork);
        }

        public async Task<IViewComponentResult> InvokeAsync(bool CanClose = true, bool DefaultOpen = true)
        {
            TermoCompromisso Termo = await ServicoTermoCompromisso.BuscarUm(x => x.DataHoraUltimaAlteracao <= System.DateTime.Now);
            ModalCardViewModel Modal = new ModalCardViewModel("Modal-Termo-Compromisso",Termo.Titulo);
            Modal.CanClose = CanClose;
            Modal.DefaultOpen = DefaultOpen;
            Modal.Texto = Termo.Termos;
            return View(Modal);
        }
    }
}
