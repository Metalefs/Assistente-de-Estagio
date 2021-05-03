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
using Newtonsoft.Json;
using ADE.Dominio.Models.Enums;

namespace Assistente_de_Estagio.Areas.Shared
{
    [Authorize]
    public class ChatController : BaseController
    {
        readonly ApplicationDbContext context;
        static UnitOfWork unitOfWork;
        private ServicoMensagemIndividual _servicoMensagemIndividual;

        public ChatController(
            UserManager<UsuarioADE> userManager,
            SignInManager<UsuarioADE> signInManager,
            ApplicationDbContext _context
            ) : base(new UnitOfWork(_context), userManager, signInManager)
        {
            context = _context;
            unitOfWork = new UnitOfWork(context);
            _servicoMensagemIndividual = new ServicoMensagemIndividual(ref unitOfWork);
        }

        [HttpGet]
        public async Task<IActionResult> ObterHistoricoConversaComUsuario(string idUsuario)
        {
            using (unitOfWork = new UnitOfWork(context))
            {
                try
                {
                    UsuarioADE usuario = await ObterUsuarioLogado();
                    List<MensagemIndividual> mensagens = await _servicoMensagemIndividual.BuscarConversaComUsuario(usuario, idUsuario);
                    return Json(new { retorno = "Sucesso ao obter histórico." });
                }
                catch (Exception ex)
                {
                    await LogError(ex.Message, ex.Source, EnumTipoLog.ErroInterno);
                    return Json(new { retorno = "Falha ao obter histórico." + ex.Message });
                };
            }
        }
        [HttpGet]
        public async Task<IActionResult> SalvarMensagem(MensagemIndividual mensagem)
        {
            using (unitOfWork = new UnitOfWork(context))
            {
                try
                {
                    UsuarioADE usuario = await ObterUsuarioLogado();
                    await _servicoMensagemIndividual.CadastrarAsync(usuario, mensagem);
                    return Json( new { retorno = "Sucesso ao enviar mensagem." });
                }
                catch (Exception ex)
                {
                    await LogError(ex.Message, ex.Source, EnumTipoLog.ErroInterno);
                    return Json(new { retorno = "Falha ao enviar mensagem." + ex.Message });
                };
            }
        }

    }
}