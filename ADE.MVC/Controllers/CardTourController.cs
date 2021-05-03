using System.Collections.Generic;
using System.Threading.Tasks;
using ADE.Aplicacao.Services;
using ADE.Apresentacao.Areas.Shared;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using Assistente_de_Estagio.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using Newtonsoft.Json;
using ADE.Dominio.Models.Enums;
using ADE.Apresentacao.Models;

namespace Assistente_de_Estagio.Areas.Shared
{
    [Authorize]
    public class CardTourController : BaseController
    {
        readonly ApplicationDbContext context;
        static UnitOfWork unitOfWork;
        private ServicoTourStep ServicoTourStep;

        public CardTourController(
            UserManager<UsuarioADE> userManager,
            SignInManager<UsuarioADE> signInManager,
            ApplicationDbContext _context
            ) : base(new UnitOfWork(_context), userManager, signInManager)
        {
            context = _context;
        }

        [HttpGet]
        public async Task<IActionResult> ObterCardTourPagina(string pathname)
        {
            using (unitOfWork = new UnitOfWork(context))
            {
                try
                {
                    ServicoTourStep = new ServicoTourStep(ref unitOfWork);
                    string Area = string.Empty;
                    string Controller = string.Empty;
                    string View = string.Empty;
                    string[] pathparts = pathname.TrimStart('/').Split('/');
                    if (pathparts.Length < 3)
                    {
                        Area = pathparts[0];
                        Controller = pathparts[1];
                        View = "Index";
                    }
                    else
                    {
                        Area = pathparts[0];
                        Controller = pathparts[1];
                        View = pathparts[2];
                    }
                    List<TourStep> steps = await ServicoTourStep.BuscarTourStepParaView(View, Controller, Area);
                    List<TourStepJs> tourStepJs = new List<TourStepJs>();
                    foreach(TourStep step in steps)
                    {
                        tourStepJs.Add(step.ToJs());
                    }
                    return Json(JsonConvert.SerializeObject(tourStepJs));
                }
                catch (Exception ex)
                {
                    await LogError(ex.Message, ex.Source, EnumTipoLog.ErroInterno);
                    return Json("{\"Erro\": \"Erro ao obter Card tour\"}");
                };
            }
        }
    }
}