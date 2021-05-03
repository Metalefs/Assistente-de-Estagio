using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using ADE.Dominio.Models;
using System.Linq;

namespace Assistente_de_Estagio.Areas.Principal.UserHome.Components.AnoHistoricoRegistroViewComponent
{
    public class AnoHistoricoRegistroViewComponent : ViewComponent
    {

        public AnoHistoricoRegistroViewComponent()
        {
            
        }

        public async Task<IViewComponentResult> InvokeAsync(List<RegistroDeHoras> Registros)
        {
            AnoHistoricoRegistroVM model;
            try
            {
                model = new AnoHistoricoRegistroVM()
                {
                    Registros = Registros,
                    AnoReferencia = Registros.First().DataHoraUltimaAlteracao
                };
            }
            catch (Exception)
            {
                model = new AnoHistoricoRegistroVM()
                {
                    Registros = Registros,
                    AnoReferencia = DateTime.Now
                };
            }
            return View(model);
        }
    }

    public class AnoHistoricoRegistroVM
    {
        public List<RegistroDeHoras> Registros { get; set; }
        public DateTime AnoReferencia { get; set; }
    }

}
