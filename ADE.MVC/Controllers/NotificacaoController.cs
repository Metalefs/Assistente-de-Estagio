using System.Collections.Generic;
using System.Linq;
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
using ADE.Dominio.Interfaces;
using ADE.Dominio.Models;
using ADE.Dominio.Models.RelacaoEntidades;
using Newtonsoft.Json;
using ADE.Dominio.Models.Enums;

namespace Assistente_de_Estagio.Areas.Shared
{
    [Authorize]
    public class NotificacaoController : BaseController
    {
        readonly ApplicationDbContext context;
        static UnitOfWork unitOfWork;
        private ServicoAlteracaoEntidadesSistema _servicoAlteracaoEntidades;
        private ServicoVisualizacaoNotificacaoGeral _servicoVisualizacaoNotificacaoGeral;
        private ServicoNotificacaoIndividual _servicoNotificacaoIndividual;

        public NotificacaoController(
            UserManager<UsuarioADE> userManager,
            SignInManager<UsuarioADE> signInManager,
            ApplicationDbContext _context
            ) : base(new UnitOfWork(_context), userManager, signInManager)
        {
            context = _context;
        }

        [HttpGet]
        public async Task<IActionResult> ObterNotificacoesDeUsuario()
        {
            using (unitOfWork = new UnitOfWork(context))
            {
                try
                {
                    UsuarioADE usuario = await ObterUsuarioLogado();
                    if(usuario.ReceberNotificacaoFocado() || usuario.ReceberNotificacaoGeral())
                    {
                        List<NotificacaoIndividual> Notificacoes = await ListaNotificacoesUsuario(usuario, unitOfWork);
                        string NotificacoesJson = JsonConvert.SerializeObject(Notificacoes, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                        return Json(NotificacoesJson);
                    }
                    return Json("{\"Erro\": \"Usuário não recebe notificações gerais\"}");
                }
                catch (Exception ex)
                {
                    await LogError(ex.Message, ex.Source, EnumTipoLog.ErroInterno);
                    return Json("{\"Erro\": \"Erro ao obter notificações\"}");
                };
            }
        }

        public async Task<List<NotificacaoIndividual>> ListaNotificacoesUsuario(UsuarioADE usuario, UnitOfWork unitOfWork)
        {
            _servicoNotificacaoIndividual = new ServicoNotificacaoIndividual(ref unitOfWork);
           return await _servicoNotificacaoIndividual.BuscarPorIdUsuario(usuario.Id);
        }

        [HttpGet]

        public async Task<IActionResult> ObterNotificacoesDeAlteracaoDoSistema()
        {
            using (unitOfWork = new UnitOfWork(context))
            {
                try
                {
                    UsuarioADE usuario = await ObterUsuarioLogado();
                    if (usuario.ReceberNotificacaoGeral())
                    {
                        List<Notificacao> Notificacoes = await NotificacoesGeraisUsuario(unitOfWork, usuario);
                        string NotificacoesJson = JsonConvert.SerializeObject(Notificacoes, Formatting.Indented, new JsonSerializerSettings{ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
                        return Json(NotificacoesJson);
                    }
                    return Json("{\"Erro\": \"Usuário não recebe notificações gerais\"}");
                }
                catch (Exception ex)
                {
                    await LogError(ex.Message, ex.Source, EnumTipoLog.ErroInterno);
                    return Json("{\"Erro\": \"Erro ao obter notificações\"}");
                };
            }
        }

        public async Task <List<Notificacao>> NotificacoesGeraisUsuario(UnitOfWork unitOfWork, UsuarioADE usuario)
        {
            _servicoAlteracaoEntidades = new ServicoAlteracaoEntidadesSistema(ref unitOfWork);
            _servicoVisualizacaoNotificacaoGeral = new ServicoVisualizacaoNotificacaoGeral(ref unitOfWork);
            List<VisualizacaoNotificacaoGeral> NotificacoesVisualizadas = await _servicoVisualizacaoNotificacaoGeral.BuscarPorIdUsuario(usuario.Id);
            List<AlteracaoEntidadesSistema> AlteracoesSistema = await _servicoAlteracaoEntidades.FiltrarNotificacoes(usuario, NotificacoesVisualizadas);
            List<Notificacao> Notificacoes = new List<Notificacao>();
            foreach (AlteracaoEntidadesSistema alteracao in AlteracoesSistema)
            {
                try
                {
                    IModeloADE Entidade = await _servicoAlteracaoEntidades.ObterEntidadeAlteracao(alteracao.Entidade, alteracao.IdentificadorEntidade);
                    UsuarioADE AutorAlteracao = await ObterUsuarioPorEmailOuId(alteracao.IdAutor);
                    alteracao.Autor = AutorAlteracao;
                    Notificacoes.Add(new Notificacao(alteracao, Entidade));
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Notificacoes;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarVisualizacaoNotificacaoIndividual(int idNotificacao)
        {
            try
            {
                UsuarioADE usuario = await ObterUsuarioLogado();
                using (unitOfWork = new UnitOfWork(context))
                {
                    _servicoNotificacaoIndividual = new ServicoNotificacaoIndividual(ref unitOfWork);
                    NotificacaoIndividual notificacao = await _servicoNotificacaoIndividual.BuscarPorId(idNotificacao);
                    await _servicoNotificacaoIndividual.Dismiss(usuario, notificacao);
                    return Json("{\"Sucesso\": \"Visualização de notificação incluida.\"}");
                }
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source, EnumTipoLog.ErroInterno);
                return Json("{\"Erro\": \"Erro ao incluir visualizaçao de notificação\"}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarVisualizacaoNotificacao(int idNotificacao)
        {
            try
            {
                UsuarioADE usuario = await ObterUsuarioLogado();
                using (unitOfWork = new UnitOfWork(context))
                {
                    _servicoVisualizacaoNotificacaoGeral = new ServicoVisualizacaoNotificacaoGeral(ref unitOfWork);
                    await _servicoVisualizacaoNotificacaoGeral.CadastrarAsync(new VisualizacaoNotificacaoGeral(usuario.Id, idNotificacao));
                    return Json("{\"Sucesso\": \"Visualização de notificação incluida.\"}");
                }
            }
            catch(Exception ex)
            {
                await LogError(ex.Message, ex.Source, EnumTipoLog.ErroInterno);
                return Json("{\"Erro\": \"Erro ao incluir visualizaçao de notificação\"}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> VisualizarTodos()
        {
            try
            {
                UsuarioADE usuario = await ObterUsuarioLogado();
                using (unitOfWork = new UnitOfWork(context))
                {
                    List<NotificacaoIndividual> Notificacoes = await ListaNotificacoesUsuario(usuario, unitOfWork);
                    foreach(NotificacaoIndividual not in Notificacoes)
                    {
                        await _servicoVisualizacaoNotificacaoGeral.CadastrarAsync(new VisualizacaoNotificacaoGeral(usuario.Id, not.Identificador));
                    }
                    return Json("{\"Sucesso\": \"Visualização de notificação incluida.\"}");
                }
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source, EnumTipoLog.ErroInterno);
                return Json("{\"Erro\": \"Erro ao incluir visualizaçao de notificação\"}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetarTipoRecebimentoNotificacao(EnumTipoRecebimentoNotificacao TipoRecebimentoNotificacao)
        {
            try
            {
                UsuarioADE usuario = await ObterUsuarioLogado();
                IdentityResult result = await SetarTipoRecebimentoNotificacao(usuario, TipoRecebimentoNotificacao);
                if (result.Succeeded)
                {
                    return Json("{\"Sucesso\": \"Tipo de recebimento de notificação alterado com sucesso.\"}");
                }
                else
                {
                    System.Text.StringBuilder Errors = new System.Text.StringBuilder();
                    foreach(IdentityError error in result.Errors)
                    {
                        Errors.Append($"{error.Code} - {error.Description}");
                    }
                    await LogError(Errors.ToString(), "SetarTipoRecebimentoNotificacao", EnumTipoLog.ErroInterno);
                    return Json("{\"Erro\": \"Erro ao alterar o tipo de recebimento de notificação.\"}");
                }
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source, EnumTipoLog.ErroInterno);
                return Json("{\"Erro\": \"Erro ao alterar o tipo de recebimento de notificação.\"}");
            }
        }


        [HttpGet]
        public IActionResult ObterNotificacoesSistema()
        {
            return View();
        }

    }
}