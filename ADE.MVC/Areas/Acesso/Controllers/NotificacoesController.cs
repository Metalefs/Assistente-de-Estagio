using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using Assistente_de_Estagio.Areas.Acesso.Models;
using Assistente_de_Estagio.Areas.Shared;
using Assistente_de_Estagio.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ADE.Apresentacao.Areas.Acesso.Controllers
{
    [Area("Acesso")]
    public class NotificacoesController : NotificacaoController
    {
        static UnitOfWork unitOfWork;
        ApplicationDbContext context;
        public NotificacoesController(
            ApplicationDbContext _context,
            UserManager<UsuarioADE> userManager,
            SignInManager<UsuarioADE> signInManager
        ) : 
        base(
            userManager,
            signInManager,
            _context
        )
        {
            context = _context;
            unitOfWork = new UnitOfWork(_context);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            AlterarTipoNotificacaoViewmodel model = await ParseAlterarTipoNotificacaoViewmodel();
            return View(model);
        }

        private async Task<AlterarTipoNotificacaoViewmodel> ParseAlterarTipoNotificacaoViewmodel()
        {
            UsuarioADE usuario = await ObterUsuarioLogado();
            AlterarTipoNotificacaoViewmodel model = new AlterarTipoNotificacaoViewmodel();
            if (usuario.ReceberNotificacaoFocado() || usuario.ReceberNotificacaoGeral())
            {
                model.NotificacacoesIndividuais = await base.ListaNotificacoesUsuario(usuario, unitOfWork);
            }
            else if (!usuario.ReceberNotificacaoFocado())
            {
                model.NotificacacoesGerais = await base.NotificacoesGeraisUsuario(unitOfWork, usuario);
            }
            model.TipoRecebimentoNotificacao = usuario.TipoRecebimentoNotificacao;
            return model;
        }

        [HttpPost]
        public async Task<IActionResult> AlterarTipoNotificacao(EnumTipoRecebimentoNotificacao TipoRecebimentoNotificacao)
        {
            try
            {
                UsuarioADE usuario = await ObterUsuarioLogado();
                usuario.TipoRecebimentoNotificacao = TipoRecebimentoNotificacao;
                await AtualizarUsuario(usuario);
                ViewBag.Retorno = $"O Tipo de recebimento de notificações foi alterado para {Enum.GetName(typeof(EnumTipoRecebimentoNotificacao), TipoRecebimentoNotificacao)}";
                return View("Index", await ParseAlterarTipoNotificacaoViewmodel());
            }
            catch (JsonSerializationException ex)
            {
                await LogError(ex.Message, ex.TargetSite.Name, EnumTipoLog.AlteracaoUsuario);
                ModelState.AddModelError("Falha","Ocorreu um erro ao serializar seus dados, contate o suporte para mais informações");
                return View("Index", await ParseAlterarTipoNotificacaoViewmodel());
            }
        }

    }
}