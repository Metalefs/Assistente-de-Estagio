using System;
using System.Threading.Tasks;
using ADE.Aplicacao.Services;
using ADE.Apresentacao.Areas.Shared;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using Assistente_de_Estagio.Areas.Acesso.Models;
using Assistente_de_Estagio.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ADE.Apresentacao.Areas.Acesso.Controllers
{
    [Area("Acesso")]
    public class SenhaController : BaseController
    {
        static UnitOfWork unitOfWork;

        public SenhaController(
            UserManager<UsuarioADE> userManager,
            SignInManager<UsuarioADE> signInManager,
            ApplicationDbContext context
        ) : base(unitOfWork = new UnitOfWork(context), userManager, signInManager) { }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TrocarSenha(SenhaModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(nameof(Index),model);
                }
                var usuario = await ObterUsuarioLogado();
                string IdUsuario = ObterIdUsuario();
                if (null == usuario)
                    return NotFound($"Não foi possivel recuperar o ID '{IdUsuario}'.");
                var TrocarSenhaResult = await TrocarSenha(usuario,model.Input.OldPassword,model.Input.NewPassword);
                if (!TrocarSenhaResult.Succeeded)
                {
                    foreach (var error in TrocarSenhaResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View("Index", model);
                }
                await RefreshSignIn(usuario);
                await RegistrarTrocaDeSenha(usuario);
                ViewBag.Retorno = "Sua senha foi alterada.";
                return View("Index",model);
            }
            catch(Exception ex)
            {
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.AlteracaoSenha);
                return View("Index", model);
            }
        }

        private async Task RegistrarTrocaDeSenha(UsuarioADE usuario)
        {
            string Mensagem = $"Usuario {usuario.UserName} ({usuario.Id}) trocou sua senha com sucesso";
            await SalvarLog(Mensagem,"DadosPessoais",Dominio.Models.Enums.EnumTipoLog.AlteracaoSenha,Dominio.Models.Enums.TipoEvento.Alteracao);
        }
    }
}