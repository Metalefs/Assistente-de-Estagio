using System.Threading.Tasks;
using ADE.Apresentacao.Areas.Shared;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using Assistente_de_Estagio.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using ADE.Aplicacao.Services;
using ADE.Dominio.Models;

namespace ADE.Apresentacao.Areas.Ajuda.Controllers
{
    [Authorize]
    [Area("Ajuda")]
    public class RegulamentacaoController : BaseController
    {
        ServicoRegulamentacaoCurso servicoRegulamentacaoCurso;
        ServicoCurso servicoCurso;
        ServicoInstituicao servicoInstituicao;
        UnitOfWork unitOfWork;

        public RegulamentacaoController(
            UserManager<UsuarioADE> userManager,
            SignInManager<UsuarioADE> signInManager,
            ApplicationDbContext _context
            ) : base(new UnitOfWork(_context), userManager, signInManager)
        {
            unitOfWork = new UnitOfWork(_context);
            servicoRegulamentacaoCurso = new ServicoRegulamentacaoCurso(ref unitOfWork);
            servicoCurso = new ServicoCurso(ref unitOfWork);
            servicoInstituicao = new ServicoInstituicao(ref unitOfWork);
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                if (UsuarioValido())
                {
                    UsuarioADE usuario = await ObterUsuarioLogado();
                    RegulamentacaoCurso regulamentacao = await servicoRegulamentacaoCurso.RecuperarRegulamentacao(usuario.IdCurso);
                    regulamentacao.IdCursoNavigation = await servicoCurso.BuscarPorId(usuario.IdCurso);
                    regulamentacao.IdCursoNavigation.Instituicao = await servicoInstituicao.BuscarPorId(regulamentacao.IdCursoNavigation.IdInstituicao);
                    return View(regulamentacao);
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