using System.Collections.Generic;
using System.Threading.Tasks;
using ADE.Aplicacao.Services;
using ADE.Apresentacao.Areas.Shared;
using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using Assistente_de_Estagio.Areas.Principal.Models;
using Assistente_de_Estagio.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ADE.Apresentacao.Areas.Principal.Controllers
{
    [Authorize]
    [Area("Principal")]
    public class AtividadesController : BaseController
    {
        private ApplicationDbContext context;
        private UnitOfWork unitOfWork;
        private ServicoAtividadeEstagio _ServicoAtividadeEstagio;
        private ServicoAtividadeUsuario _ServicoAtividadeUsuario;

        public AtividadesController(
            UserManager<UsuarioADE> userManager,
            ApplicationDbContext _context
        ) : base(new UnitOfWork(_context), userManager)
        {
            context = _context;
            unitOfWork = new UnitOfWork(context);
            _ServicoAtividadeEstagio = new ServicoAtividadeEstagio(ref unitOfWork);
            _ServicoAtividadeUsuario = new ServicoAtividadeUsuario(ref unitOfWork);
        }

        public async Task<IActionResult> Index(bool Partial = false)
        {
            UsuarioADE usuario = await ObterUsuarioComDadosPessoais();
            AtividadesViewModel model = new AtividadesViewModel();
            try
            {
                List<AtividadeEstagio> atividadeEstagio = await _ServicoAtividadeEstagio.BuscarPorCursoDoUsuario(usuario);
                List<AtividadeUsuario> atividadeUsuario = await _ServicoAtividadeUsuario.BuscarPorIdUsuario(usuario.Id);
                model = new AtividadesViewModel(usuario, atividadeEstagio, atividadeUsuario);
            }
            catch(System.Exception ex)
            {
                model = new AtividadesViewModel(usuario, new List<AtividadeEstagio>(), new List<AtividadeUsuario>());
            }

            if (Partial)
                return PartialView("_Index", model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarAtividadeUsuario(AtividadeUsuario atividade)
        {
            try
            {
                UsuarioADE usuario = await ObterUsuarioLogado();
                atividade.IdCurso = usuario.IdCurso;
                atividade.IdUsuario = usuario.Id;
                atividade.Usuario = usuario;
                await _ServicoAtividadeUsuario.CadastrarAtividadeUsuarioAsync(usuario, atividade);
                ViewBag.Retorno = $"A atividade '{atividade.Descricao}' foi cadastrada com sucesso.";
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("Falha", $"Ocorreu um erro ao Cadastrar a sua atividade: {ex.Message}");
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.CriacaoAtividadeUsuario);
            }
            return RedirectToAction("Index", "Atividades");
        }
        [HttpPost]
        public async Task<IActionResult> ConcluirAtividade(int IdAtividade)
        {
            UsuarioADE usuario = await ObterUsuarioLogado();
            try
            {
                AtividadeUsuario atividade = await _ServicoAtividadeUsuario.BuscarPorId(IdAtividade);
                EnumEstadoAtividade estado = atividade.VerificarEstado();
                atividade.Concluir();
                await _ServicoAtividadeUsuario.AtualizarAsync(usuario, atividade);
                ViewBag.Retorno = $"Sucesso ao alterar atividade!";
                return Json(new { retorno = "Sucesso ao alterar atividade!" });
            }
            catch (System.Exception ex)
            {
                await LogError(ex.Message, "AtualizarAtividadeUsuario", Dominio.Models.Enums.EnumTipoLog.AlteracaoAtividadeUsuario);
                ModelState.AddModelError("Falha","Erro ao alterar atividade! Contate o suporte caso esse erro volte a acontecer. - {ex.Message}");
            }
            return RedirectToAction("Index", "Atividades");
        }

        [HttpPost]
        public async Task<IActionResult> ConcluirAtividades(List<int> IdAtividade)
        {
            UsuarioADE usuario = await ObterUsuarioLogado();
            try
            {
                foreach(int id in IdAtividade)
                {
                    AtividadeUsuario atividade = await _ServicoAtividadeUsuario.BuscarPorId(id);
                    EnumEstadoAtividade estado = atividade.VerificarEstado();
                    atividade.Concluir();
                    await _ServicoAtividadeUsuario.AtualizarAsync(usuario, atividade);
                }
                return Json(new { retorno = "Sucesso ao alterar atividade!" });
            }
            catch (System.Exception ex)
            {
                await LogError(ex.Message, "AtualizarAtividadeUsuario", Dominio.Models.Enums.EnumTipoLog.AlteracaoAtividadeUsuario);
                //return Json(new { retorno = $"Erro ao alterar atividade! Contate o suporte caso esse erro volte a acontecer. - {ex.Message}" });
            }
            return RedirectToAction("Index", "Atividades");
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarAtividade(AtividadeUsuario atividade)
        {
            UsuarioADE usuario = await ObterUsuarioLogado();
            try
            {
                AtividadeUsuario atividadeAntiga = await _ServicoAtividadeUsuario.BuscarPorId(atividade.Identificador);
                atividadeAntiga.Clonar(atividade);

                await _ServicoAtividadeUsuario.AtualizarAtividadeUsuarioAsync(usuario, atividadeAntiga);
                ViewBag.Retorno = "Sucesso ao alterar atividade!";
            }
            catch (System.Exception ex)
            {
                await LogError(ex.Message, "AtualizarAtividadeUsuario", Dominio.Models.Enums.EnumTipoLog.AlteracaoAtividadeUsuario);
                return Json(new { retorno = $"Erro ao alterar atividade! Contate o suporte caso esse erro volte a acontecer. - {ex.Message}" });
            }
            return RedirectToAction("Index", "Atividades");
        }
        public async Task<IActionResult> AlterarAtividadeUsuario(AtividadeUsuario atividade)
        {
            try
            {
                UsuarioADE usuario = await ObterUsuarioLogado();
                AtividadeUsuario at1 = await _ServicoAtividadeUsuario.BuscarPorId(atividade.Identificador);
                await _ServicoAtividadeUsuario.AtualizarAtividadeUsuarioAsync(usuario,atividade);
                ViewBag.Retorno = $"A atividade '{at1.Descricao}' foi alterada para <br> '{atividade.Descricao}'.";
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("Falha", $"Ocorreu um erro ao Alterar a sua atividade: {ex.Message}");
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.AlteracaoAtividadeUsuario);
            }
            return RedirectToAction("Index", "Atividades");
        }
        public async Task<IActionResult> RemoverAtividadeUsuario(int IdAtividade)
        {
            try
            {
                UsuarioADE usuario = await ObterUsuarioLogado();
                AtividadeUsuario atividade = await _ServicoAtividadeUsuario.BuscarPorId(IdAtividade);
                await _ServicoAtividadeUsuario.RemoverAtividadeUsuarioAsync(usuario,atividade);
                ViewBag.Retorno = $"A atividade '{atividade.Descricao}' foi removida.";
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("Falha", $"Ocorreu um erro ao Remover a sua atividade: {ex.Message}");
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.DelecaoAtividadeUsuario);
            }
            return RedirectToAction("Index", "Atividades");
        }
        
    }
}