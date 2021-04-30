using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ADE.Dominio.Models;
using ADE.Aplicacao.Services;
using ADE.Infra.Data.UOW;
using Assistente_de_Estagio.Data;
using Microsoft.AspNetCore.Identity;
using ADE.Dominio.Models.Individuais;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assistente_de_Estagio.Areas.Principal.UserHome.Components.BuscaHistoricoViewComponent
{
    public class BuscaHistoricoViewComponent : ViewComponent
    {
        UnitOfWork unitOfWork;
        ServicoRegistroDeHoras ServicoRegistroDeHoras;
        ServicoUsuario ServicoUsuario;

        public BuscaHistoricoViewComponent(ApplicationDbContext context, UserManager<UsuarioADE> userManager)
        {
            unitOfWork = new UnitOfWork(context);
            ServicoRegistroDeHoras = new ServicoRegistroDeHoras(ref unitOfWork);
            ServicoUsuario = new ServicoUsuario(unitOfWork, userManager);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<DateTime> model = new List<DateTime>();
            try
            {
                UsuarioADE usuario = await ServicoUsuario.ObterUsuarioLogado(UserClaimsPrincipal);
                List<RegistroDeHoras> Registros = await ServicoRegistroDeHoras.ObterRegistrosUsuario(usuario);

                var atividadesAgrupadas = Registros.GroupBy(
                dataAtividade => dataAtividade.DataHoraUltimaAlteracao.Date,
                id => id.Identificador,
                (dataAtividade, id) => new
                {
                    Key = dataAtividade,
                    Count = id.Count()
                });

                foreach (var result in atividadesAgrupadas)
                {
                    model.Add(result.Key);
                }
            }
            catch (System.Exception)
            {
                
            }
            return View(model);
        }
    }
}
