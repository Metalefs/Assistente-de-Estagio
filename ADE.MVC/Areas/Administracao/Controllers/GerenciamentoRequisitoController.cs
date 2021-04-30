using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADE.Aplicacao.Services;
using ADE.Apresentacao.Areas.Shared;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using Assistente_de_Estagio.Areas.Administracao.Models;
using Assistente_de_Estagio.Areas.Shared;
using Assistente_de_Estagio.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace ADE.Apresentacao.Areas.Administracao.Controllers
{
    //[RequireHttps]
    [Authorize(Roles = "Admin,CriadorConteudo")]
    [Area("Administracao")]
    public class GerenciamentoRequisitoController : BaseController
    {
        static UnitOfWork unitOfWork;
        private ServicoRequisito _requisitoServices;
        private ServicoOpcaoRequisito _servicoOpcaoRequisito;
        private readonly ServicoAlteracaoEntidadesSistema _servicoAlteracoes;
        private readonly int pageSize = 25;

        public GerenciamentoRequisitoController(
            UserManager<UsuarioADE> userManager,
            SignInManager<UsuarioADE> signInManager,
            ApplicationDbContext context
            ) :base(unitOfWork = new UnitOfWork(context), userManager, signInManager)
        {
            _requisitoServices = new ServicoRequisito(ref unitOfWork);
            _servicoOpcaoRequisito = new ServicoOpcaoRequisito(ref unitOfWork);
            _servicoAlteracoes = new ServicoAlteracaoEntidadesSistema(ref unitOfWork);
        }

        public async Task<IActionResult> Index(bool Partial = false, int? pageNumber = 1)
        {            
            List<Requisito> ListaRequisito = await _requisitoServices.ListarAsync();
            PaginatedList<Requisito> lista = PaginatedList<Requisito>.Create(ListaRequisito.AsQueryable(), pageNumber ?? 1, pageSize);
            if (Partial)
                return PartialView("_Index", lista);
            return View(lista);
        }

        public IActionResult Criar() => View(new Requisito());

        [HttpPost]
        public async Task<IActionResult> Criar(Requisito requisito)
            {
            UsuarioADE user = await ObterUsuarioLogado();
            if (ModelState.IsValid)
            {
                try
                {
                    await _requisitoServices.CadastrarAsync(user,requisito);
                    return Json("<p class='text-sucess'>Requisito criado com sucesso!</p>");
                }
                catch(Exception ex)
                {
                    await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.CriacaoRequisito);
                    return Json($"<p class='text-danger'>Ocorreu um erro ao criar o requisito</p><p>{ex.Message}</p>");
                }
            }
            else
            {
                string Erros = "<ul>";
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (ModelError modelState in allErrors)
                {
                    Erros += $"<li>{modelState.ErrorMessage}</li>";
                }
                Erros += "</ul>";
                return Json(Erros);
            }
        }

        [HttpGet]
        public async Task<IActionResult> VisualizarEntidade(int id)
        {
            try
            {
                VisualizarEntidadeViewmodel<Requisito> model = new VisualizarEntidadeViewmodel<Requisito>()
                {
                    Entidade = await _requisitoServices.BuscarPorId(id),
                    ListaAlteracoes = await _servicoAlteracoes.Filtrar(x => x.Entidade == EnumEntidadesSistema.Requisito && x.IdentificadorEntidade == id)
                };
                return View(model);
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source + "-" + ex.TargetSite.Name, EnumTipoLog.ErroInterno);
                ModelState.AddModelError("Falha", "Ocorreu um montar a visualização de entidade, contate o suporte para maior exclarecimento" + ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> VisualizarAlteracoesEntidade(int id)
        {
            try
            {
                Requisito requisito = await _requisitoServices.BuscarPorId(id);
                AlteracoesEntidadeViewModel model = new AlteracoesEntidadeViewModel()
                {
                    DescricaoEntidade = requisito.ToString(),
                    Alteracoes = await _servicoAlteracoes.Filtrar(x => x.Entidade == EnumEntidadesSistema.Requisito && x.IdentificadorEntidade == id)
                };
                return View(model);
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source + "-" + ex.TargetSite.Name, EnumTipoLog.ErroInterno);
                ModelState.AddModelError("Falha", "Ocorreu um montar a visualização de entidade, contate o suporte para maior exclarecimento" + ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editar(int IdRequisito)
        {
            Requisito req = await _requisitoServices.BuscarPorId(IdRequisito);
            return View("Editar",req);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Requisito Requisito, string Mensagem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        UsuarioADE usuario = await ObterUsuarioLogado();
                        await _requisitoServices.AtualizarAsync(usuario, Requisito, Mensagem);
                        ViewBag.Retorno = "Requisito alterado com sucesso!";
                        await unitOfWork.Commit();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (_requisitoServices.ListarAsync().Result.Any(req => req.Identificador == Requisito.Identificador))
                            ModelState.AddModelError("Falha", "Esse requisito já está sendo alterado");
                    }
                }
                return await Editar(Requisito.Identificador);
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source + "-" + ex.TargetSite.Name, Dominio.Models.Enums.EnumTipoLog.AlteracaoRequisito);
                ModelState.AddModelError("Falha", "Ocorreu um erro ao alterar o requisito, contate o suporte para maior exclarecimento");
                return await Editar(Requisito.Identificador);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CriarOpcoesRequisito(string nome, string valor)
        {
            UsuarioADE usuario = await ObterUsuarioLogado();
            try
            {
                Requisito requisito = await _requisitoServices.BuscarUm(usuario,x => x.Bookmark == nome);
                OpcaoRequisito opcao = new OpcaoRequisito(requisito.Identificador, valor);
                await _servicoOpcaoRequisito.CadastrarAsync(usuario, opcao);
                return Json($"{{Status:\"Opção de Requisito '{valor}' criada\"}}");
            }
            catch (Exception)
            {
                return Json($"{{Status:\"Falha ao criar opção de requisito {valor} para o requisito {nome}, (tente criar uma opção por vez)\"}}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditarOpcaoRequisito(int IdRequisito, string ValorAntigo, string ValorNovo)
        {
            UsuarioADE usuario = await ObterUsuarioLogado();
            try
            {
                OpcaoRequisito opcao = await _servicoOpcaoRequisito.BuscarPorId(IdRequisito, ValorAntigo);
                opcao.Valor = ValorNovo;
                await _servicoOpcaoRequisito.AtualizarAsync(usuario, opcao);
                ViewBag.Retorno = $"Valor da Opção de Requisito alterada de '{ValorAntigo}' para '{ValorNovo}'";
                return await Editar(IdRequisito);
            }
            catch (Exception ex)
            {
                ViewBag.Retorno = $"Falha ao alterar o valor da opção do requisito de '{ValorAntigo}' para '{ValorNovo}'";
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.AlteracaoRequisito);
                return await Editar(IdRequisito);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeletarOpcaoRequisito(int IdRequisito, string ValorAntigo)
        {
            UsuarioADE usuario = await ObterUsuarioLogado();
            try
            {
                OpcaoRequisito opcao = await _servicoOpcaoRequisito.BuscarPorId(IdRequisito, ValorAntigo);
                await _servicoOpcaoRequisito.RemoverAsync(usuario, opcao);
                ViewBag.Retorno = $"Opção de Requisito '{ValorAntigo}' Removida com sucesso";
                return await Editar(IdRequisito);
            }
            catch (Exception ex)
            {
                ViewBag.Retorno = $"Falha ao remover opção do requisito '{ValorAntigo}'";
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.AlteracaoRequisito);
                return await Editar(IdRequisito);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExcluirRequisitoConfirmed(int id)
        {
            try
            {
                UsuarioADE ADEUser = await ObterUsuarioLogado();
                var curso = await _requisitoServices.BuscarPorId(id);
                await _requisitoServices.RemoverAsync(ADEUser,curso);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.DelecaoRequisito);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}