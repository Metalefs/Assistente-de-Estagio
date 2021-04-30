using ADE.Dominio.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Assistente_de_Estagio.Data;
using ADE.Infra.Data.UOW;
using ADE.Aplicacao.Services;
using ADE.Dominio.Models.Individuais;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Areas.Principal.Views.RegistroHoras.Components.UltimosRegistros
{
    public class UltimosRegistrosViewComponent: ViewComponent
    {
        UnitOfWork unitOfWork;
        ServicoRegistroDeHoras ServicoRegistroDeHoras;
        ServicoUsuario ServicoUsuario;

        public UltimosRegistrosViewComponent(ApplicationDbContext context, UserManager<UsuarioADE> userManager)
        {
            unitOfWork = new UnitOfWork(context);
            ServicoRegistroDeHoras = new ServicoRegistroDeHoras(ref unitOfWork);
            ServicoUsuario = new ServicoUsuario(unitOfWork, userManager);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            UltimosRegistrosVM model;
            try
            {
                UsuarioADE usuario = await ServicoUsuario.ObterUsuarioLogado(UserClaimsPrincipal);
                List<RegistroDeHoras> Registros = await ServicoRegistroDeHoras.ObterUltimosRegistrosUsuario(usuario);
                int TotalRegistros = await ServicoRegistroDeHoras.CountByFilter(x=>x.IdUsuario == usuario.Id);
                model = new UltimosRegistrosVM(Registros, TotalRegistros);
            }
            catch (System.Exception)
            {
                model = new UltimosRegistrosVM(new List<RegistroDeHoras>(), 0);
            }
            return View(model);
        }
    }
}
