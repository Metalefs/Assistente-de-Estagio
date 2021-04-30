using System.Threading.Tasks;
using ADE.Apresentacao.Areas.Shared;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using Assistente_de_Estagio.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ADE.Apresentacao.Areas.Principal.Controllers
{
    [Authorize]
    [Area("Principal")]
    public class ContratosController : BaseController
    {
        public ContratosController(
            UserManager<UsuarioADE> userManager,
            SignInManager<UsuarioADE> signInManager,
            ApplicationDbContext _context
            ) : base(new UnitOfWork(_context), userManager, signInManager)
        {
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                if (UsuarioValido())
                {
                    return View();
                }
                else
                {
                    ModelState.AddModelError("Falha", "Usuário não está autenticado");
                    return RedirectToAction("Logout", "Account");
                }
                
            }
            catch(Exception ex)
            {
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                return RedirectToAction("Index", "Account");
            }
        }

    }
}