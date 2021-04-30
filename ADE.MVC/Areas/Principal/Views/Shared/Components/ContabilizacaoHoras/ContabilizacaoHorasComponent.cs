using ADE.Aplicacao.Services;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using Assistente_de_Estagio.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Areas.Principal.Shared.Components.ContabilizacaoHorasViewComponent
{
    public class ContabilizacaoHorasViewComponent : ViewComponent
    {
        UnitOfWork unitOfWork;
        ServicoRegistroDeHoras ServicoRegistroDeHoras;
        ServicoUsuario ServicoUsuario;

        public ContabilizacaoHorasViewComponent(ApplicationDbContext context, UserManager<UsuarioADE> userManager)
        {
            unitOfWork = new UnitOfWork(context);
            ServicoRegistroDeHoras = new ServicoRegistroDeHoras(ref unitOfWork);
            ServicoUsuario = new ServicoUsuario(unitOfWork, userManager);
        }

        public async Task<IViewComponentResult> InvokeAsync(UsuarioADE usuario = null)
        {
            ContabilizacaoHorasVM model;
            try
            {
                usuario = usuario ?? await ServicoUsuario.ObterUsuarioLogado(UserClaimsPrincipal);
                int total = await ServicoRegistroDeHoras.ObterTotalCargaHorariaCursoUsuario(usuario);
                float progress = await ServicoRegistroDeHoras.ObterContabilizacaoHorasUsuario(usuario);
                model = new ContabilizacaoHorasVM(total,progress/60);
            }
            catch (System.Exception)
            {
                model = new ContabilizacaoHorasVM(0, 0);
            }
            return View(model);
        }
    }

    public class ContabilizacaoHorasVM
    {
        public int total { get; set; }
        public float progress { get; set; }

        public ContabilizacaoHorasVM(int total, float progress)
        {
            this.total = total;
            this.progress = progress;
        }
    }
}
