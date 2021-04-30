using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using ADE.Dominio.Models;
using System.Linq;

namespace Assistente_de_Estagio.Areas.Principal.UserHome.Components.MesHistoricoRegistroViewComponent
{
    public class MesHistoricoRegistroViewComponent : ViewComponent
    {

        public MesHistoricoRegistroViewComponent()
        {
            
        }

        public async Task<IViewComponentResult> InvokeAsync(List<RegistroDeHoras> Registros)
        {
            MesHistoricoRegistroVM model;
            try
            {
                model = new MesHistoricoRegistroVM()
                {
                    Registros = Registros,
                    MesReferencia = Registros.First().DataHoraUltimaAlteracao
                };
            }
            catch (Exception)
            {
                model = new MesHistoricoRegistroVM()
                {
                    Registros = Registros,
                    MesReferencia = DateTime.Now
                };
            }
            return View(model);
        }
    }

    public class MesHistoricoRegistroVM
    {
        public List<RegistroDeHoras> Registros { get; set; }
        public DateTime MesReferencia { get; set; }
    }

}
