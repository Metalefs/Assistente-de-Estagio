using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ADE.Aplicacao.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ADE.Dominio.Models;
using Assistente_de_Estagio.Areas.Administracao.Models;
using Assistente_de_Estagio.Areas.Shared;
using ADE.Dominio.Models.Individuais;
using Microsoft.AspNetCore.Identity;
using ADE.Dominio.Interfaces;
using ADE.Apresentacao.Areas.Shared;
using ADE.Infra.Data.UOW;
using Assistente_de_Estagio.Data;
using ADE.Dominio.Models.Enums;

namespace Assistente_de_Estagio.Controllers
{
    [Authorize(Roles = "Admin,CriadorConteudo")]
    [Area("Administracao")]
    public class GerenciamentoCurso : BaseController
    {
        static UnitOfWork unitOfWork;
        private ServicoCurso _servicoCurso;
        private readonly ServicoAlteracaoEntidadesSistema _servicoAlteracoes;
        private readonly ServicoInstituicao _servicoInstituicao;
        private readonly ServicoAreaEstagioCurso _servicoAreaEstagioCurso;
        int pageSize = 7;

        public GerenciamentoCurso(
            UserManager<UsuarioADE> userManager,
            ApplicationDbContext context
            ) : base(unitOfWork = new UnitOfWork(context), userManager)
        {
            _servicoCurso = new ServicoCurso(ref unitOfWork);
            _servicoAlteracoes = new ServicoAlteracaoEntidadesSistema(ref unitOfWork);
            _servicoInstituicao = new ServicoInstituicao(ref unitOfWork);
            _servicoAreaEstagioCurso = new ServicoAreaEstagioCurso(ref unitOfWork);
        }

        [ActionName("Index")]
        public async Task<IActionResult> Index(bool Partial = false, int? pageNumber = 1)
        {
            GerenciamentoCursoViewmodel model = await ParseGerenciamentoCurso(pageNumber);
            if (Partial)
                return PartialView("_Index", model);
            return View(model);
        }

        private async Task<GerenciamentoCursoViewmodel> ParseGerenciamentoCurso(int? pageNumber)
        {
            List<Curso> Cursos = await _servicoCurso.ListarAsync();
            PaginatedList<Curso> cursos = PaginatedList<Curso>.Create(Cursos.AsQueryable(), pageNumber ?? 1, pageSize);
            List<Instituicao> Instituicoes = await _servicoInstituicao.ListarAsync();
            return new GerenciamentoCursoViewmodel(cursos, Instituicoes);
        }

        public async Task<IActionResult> Filtrar(FiltroCurso filtroCurso)
        {
            List<Curso> CursosFiltrados = await _servicoCurso.Filtrar(x => x.CoordenadorCurso == filtroCurso.NomeOrientador && x.TipoCurso == filtroCurso.enumTipo);
            PaginatedList<Curso> lista = PaginatedList<Curso>.Create(CursosFiltrados.AsQueryable(),  1 , 4);
            return View("Index", lista);
        }

        [HttpGet]
        public async Task<IActionResult> Criar(int? IdInstituicao)
        {
            try
            {
                IdInstituicao = IdInstituicao ?? 1;
                List<Instituicao> listaInstituicoes = await _servicoInstituicao.ListarAsync();
                
                CriarCursoViewModel model = new CriarCursoViewModel(new Curso(), IdInstituicao.ToString(), listaInstituicoes);
                return View("Criar", model);
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source + "-" + ex.TargetSite.Name, EnumTipoLog.CriacaoDocumento);
                ModelState.AddModelError("Falha", "Ocorreu um erro ao montar a página de criação -" + ex.Message);
                ViewBag.Retorno = "Ocorreu um erro ao montar a página de criação, contate o suporte para maior exclarecimento";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CriarCurso(Curso curso)
        {
            try
            {
                UsuarioADE usuario = await ObterUsuarioLogado();
                if (ModelState.IsValid)
                {
                    await _servicoCurso.CadastrarAsync(usuario,curso);
                    await unitOfWork.Commit();
                    ViewBag.Retorno = "Curso criado com sucesso!";
                }
                return await Criar(curso.IdInstituicao);
            }
            catch(Exception ex)
            {
                await LogError(ex.Message, ex.Source + "-" + ex.TargetSite.Name, EnumTipoLog.CriacaoCurso);
                ModelState.AddModelError("Falha", "Ocorreu um erro ao criar o curso, contate o suporte para maior exclarecimento");
                return await Criar(curso.IdInstituicao);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExcluirCursoConfirmed(int id)
        {
            try
            {
                UsuarioADE usuario = await ObterUsuarioLogado();
                var curso = await _servicoCurso.BuscarPorId(id);
                await _servicoCurso.RemoverAsync(usuario, curso);
            }
            catch(Exception ex)
            {
                await LogError(ex.Message, ex.Source + "-" + ex.TargetSite.Name, EnumTipoLog.DelecaoCurso);
                ModelState.AddModelError("Falha", "Ocorreu um erro ao excluir o curso, contate o suporte para maior exclarecimento");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> VisualizarEntidade(int id)
        {
            try
            {
                Curso curso = await _servicoCurso.BuscarPorId(id);
                curso.Instituicao = await _servicoInstituicao.BuscarPorId(curso.IdInstituicao);
                VisualizarEntidadeViewmodel<Curso> model = new VisualizarEntidadeViewmodel<Curso>()
                {
                    Entidade = curso,
                    ListaAlteracoes = await _servicoAlteracoes.Filtrar(x=>x.Entidade == EnumEntidadesSistema.Curso && x.IdentificadorEntidade == id)
                };
                return View(model);
            }
            catch(Exception ex)
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
                Curso curso = await _servicoCurso.BuscarPorId(id);
                AlteracoesEntidadeViewModel model = new AlteracoesEntidadeViewModel()
                {
                    DescricaoEntidade = curso.ToString(),
                    Alteracoes = await _servicoAlteracoes.Filtrar(x => x.Entidade == EnumEntidadesSistema.Curso && x.IdentificadorEntidade == id)
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
        public async Task<IActionResult> Editar(int idCurso, int idInstituicao)
        {
            Curso curso = await _servicoCurso.BuscarPorId(idCurso);
            List<Instituicao> instituicoes = await _servicoInstituicao.ListarAsync();
            List<AreaEstagioCurso> areasEstagio = await _servicoAreaEstagioCurso.BuscarPorIdCurso(idCurso);
            AlterarCursoViewModel model = new AlterarCursoViewModel(curso, idInstituicao, instituicoes, areasEstagio);
            return  View("Editar", model);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Curso curso, string Mensagem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        UsuarioADE usuario = await ObterUsuarioLogado();
                        await _servicoCurso.AtualizarAsync(usuario, curso, Mensagem);
                        await unitOfWork.Commit();
                        ViewBag.Retorno = "Curso alterado com sucesso!";
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (_servicoCurso.ListarAsync().Result.Any(e => e.Identificador == curso.Identificador))
                            return NotFound();
                        else
                            throw;
                    }
                    return await Editar(curso.Identificador, curso.IdInstituicao);
                }
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source + "-" + ex.TargetSite.Name, EnumTipoLog.AlteracaoCurso);
                ModelState.AddModelError("Falha", "Ocorreu um erro ao alterar o curso, contate o suporte para maior exclarecimento");
            }

            return await Editar(curso.Identificador, curso.IdInstituicao);
        }
    }
}
