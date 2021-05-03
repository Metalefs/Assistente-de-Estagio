using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADE.Aplicacao.Services;
using ADE.Apresentacao.Areas.Shared;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using Assistente_de_Estagio.Areas.Acesso.Models;
using Assistente_de_Estagio.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ADE.Apresentacao.Areas.Acesso.Controllers
{
    [Area("Acesso")]
    public class DadosPessoaisController : BaseController
    {
        static UnitOfWork unitOfWork;
        private static ServicoRequisitoUsuario servicoRequisitoDeUsuario;
        private static ServicoHistoricoGeracaoDocumento servicoHistoricoGeracaoDocumento;

        public bool RequirePassword { get; set; }

        public DadosPessoaisController(
            ApplicationDbContext context,
            UserManager<UsuarioADE> userManager
        ) : 
        base(
            unitOfWork = new UnitOfWork(context),
            userManager,
            null,
            servicoRequisitoDeUsuario = new ServicoRequisitoUsuario(ref unitOfWork),
            servicoHistoricoGeracaoDocumento = new ServicoHistoricoGeracaoDocumento(ref unitOfWork)
        )
        {}

        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult BaixarDadosPessoais() => View();

        [HttpGet]
        public IActionResult DeletarDadosPessoais() => View();

        [HttpPost]
        public async Task<IActionResult> BaixarDadosPessoaisAcao()
        {
            var usuario = await ObterUsuarioComDadosPessoais();
            string IdUsuario = ObterIdUsuario();
            if (null == usuario)
               return RedirectToAction("HttpStatusCodeHandler", "Error", new { message = $"Não foi possivel recuperar o Usuário com ID '{IdUsuario}'." });

            await RegistrarDownloadDeDadosPessoais(usuario);
            try
            {
                foreach(LogAcoesEspeciais log in usuario.LogAcoesEspeciais)
                {
                    log.IdUsuarioNavigation = null;
                }
                string SerializedUser = JsonConvert.SerializeObject(usuario, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                Response.Headers.Add("Content-Disposition", "attachment; filename=DadosPessoais.json");
                return new FileContentResult(Encoding.UTF8.GetBytes(SerializedUser), "text/json");
            }
            catch (JsonSerializationException ex)
            {
                await LogError(ex.Message, ex.TargetSite.Name, Dominio.Models.Enums.EnumTipoLog.CriacaoArquivo);
                ModelState.AddModelError("Falha","Ocorreu um erro ao serializar seus dados, contate o suporte para mais informações");
                return View("Index",ModelState);
            }
        }

        private Dictionary<string, string> ObterDadosPessoais(UsuarioADE usuario)
        {
            var personalData = new Dictionary<string, string>();
            var personalDataProps = typeof(UsuarioADE).GetProperties().Where(
                            prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
            foreach (var p in personalDataProps)
            {
                personalData.Add(p.Name, p.GetValue(usuario)?.ToString() ?? "null");
            }
            return personalData;
        }

        private async Task RegistrarDownloadDeDadosPessoais(UsuarioADE usuario) => await SalvarLog($"Usuario '{usuario.ToString()}' Requisitou download dos dados pessoais.", "DadosPessoais", Dominio.Models.Enums.EnumTipoLog.DownloadDadosPessoais,Dominio.Models.Enums.TipoEvento.Download, usuario);

        [HttpPost]
        public async Task<IActionResult> DeletarDadosPessoais(DadosPessoaisModel model)
        {
            var usuario = await ObterUsuarioLogado();
            string IdUsuario = ObterIdUsuario();
            if (null == usuario)
                return NotFound($"Não foi possivel recuperar o ID '{IdUsuario}'.");

            if (!await ValidarSenhaDigitada(usuario, model.Password))
            {
                ModelState.AddModelError(string.Empty, "Senha incorreta.");
                return View();
            }

            var result = await RemoverUsuario(usuario);
            var userId = await ObterIdUsuario(usuario);
            if (!result.Succeeded)
                throw new InvalidOperationException($"Erro inexperado ao deletar o usuário com ID '{userId}'.");
            
            await Logout();
            await RegistrarDelecaoDeConta(IdUsuario);
            return Redirect("~/");
        }
        
        private async Task RegistrarDelecaoDeConta(string IdUsuario) => await SalvarLog($"Usuario com ID '{IdUsuario}' deletou sua conta.", "DadosPessoais", Dominio.Models.Enums.EnumTipoLog.DelecaoConta, Dominio.Models.Enums.TipoEvento.Delecao);

    }
}