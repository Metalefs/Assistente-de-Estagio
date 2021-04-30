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
using ADE.Utilidades.Extensions;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Assistente_de_Estagio.Controllers
{
    [Authorize(Roles = "Admin,CriadorConteudo")]
    [Area("Administracao")]
    public class GerenciamentoInstituicao : BaseController
    {
        static UnitOfWork unitOfWork;
        private IServicoBase<Curso> _servicoCurso;
        private readonly ServicoAlteracaoEntidadesSistema _servicoAlteracoes;
        private readonly ServicoInstituicao _servicoInstituicao;

        public GerenciamentoInstituicao(
            UserManager<UsuarioADE> userManager,
            ApplicationDbContext context
            ) : base(unitOfWork = new UnitOfWork(context), userManager)
        {
            _servicoCurso = new ServicoCurso(ref unitOfWork);
            _servicoAlteracoes = new ServicoAlteracaoEntidadesSistema(ref unitOfWork);
            _servicoInstituicao = new ServicoInstituicao(ref unitOfWork);
        }

        [ActionName("Index")]
        public async Task<IActionResult> Index(bool Partial = false, int? pageNumber = 1)
        {
            int pageSize = 7;

            List<Instituicao> ListaInstituicoes = await _servicoInstituicao.ListarAsync();
            PaginatedList<Instituicao> lista = PaginatedList<Instituicao>.Create(ListaInstituicoes.AsQueryable(), pageNumber ?? 1, pageSize);
            if (Partial)
                return PartialView("_Index", lista);

            return View(lista);
        }
      
        //TODO
        public async Task<IActionResult> Filtrar(FiltroCurso filtroCurso)
        {
            List<Curso> CursosFiltrados = await _servicoCurso.Filtrar(x => x.CoordenadorCurso == filtroCurso.NomeOrientador && x.TipoCurso == filtroCurso.enumTipo);
            PaginatedList<Curso> lista = PaginatedList<Curso>.Create(CursosFiltrados.AsQueryable(),  1 , 4);
            return View("Index", lista);
        }

        [HttpGet]
        public async Task<IActionResult> Criar()
        {
            try
            {
                return View(new Instituicao());
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source + "-" + ex.TargetSite.Name, EnumTipoLog.CriacaoDocumento);
                ModelState.AddModelError("Falha", "Ocorreu um erro ao montar a página de criação -" + ex.Message);
                ViewBag.Retorno = "Ocorreu um erro ao montar a página de criação, contate o suporte para maior exclarecimento";
                return RedirectToAction(nameof(Index));
            }
        }
        private static byte[] GetByteArrayFromFile(IFormFile file)
        {
            using (MemoryStream target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Criar(Instituicao instituicao)
        {
            try
            {
                UsuarioADE usuario = await ObterUsuarioLogado();
                if (ModelState.IsValid)
                {
                    instituicao.Logo = GetByteArrayFromFile(instituicao.LogoFile);
                    await _servicoInstituicao.CadastrarAsync(usuario, instituicao);
                    await unitOfWork.Commit();
                    ViewBag.Retorno = "Instituicao criada com sucesso!";
                }
                return View("Criar", instituicao);
            }
            catch(Exception ex)
            {
                await LogError(ex.Message, ex.Source + "-" + ex.TargetSite.Name, EnumTipoLog.CriacaoInstituicao);
                ModelState.AddModelError("Falha", "Ocorreu um erro ao criar a instituição, contate o suporte para maior exclarecimento");
                return View("Criar", instituicao);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExcluirInstituicaoConfirmed(int id)
        {
            try
            {
                UsuarioADE usuario = await ObterUsuarioLogado();
                var instituicao = await _servicoInstituicao.BuscarPorId(id);
                await _servicoInstituicao.RemoverAsync(usuario, instituicao);
            }
            catch(Exception ex)
            {
                await LogError(ex.Message, ex.Source + "-" + ex.TargetSite.Name, EnumTipoLog.DelecaoInstituicao);
                ModelState.AddModelError("Falha", "Ocorreu um erro ao excluir a instituição, contate o suporte para maior exclarecimento");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> VisualizarEntidade(int id)
        {
            try
            {
                VisualizarEntidadeViewmodel<Instituicao> model = new VisualizarEntidadeViewmodel<Instituicao>()
                {
                    Entidade = await _servicoInstituicao.BuscarPorId(id),
                    ListaAlteracoes = await _servicoAlteracoes.Filtrar(x=>x.Entidade == EnumEntidadesSistema.Instituicao && x.IdentificadorEntidade == id)
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
                Instituicao instituicao = await _servicoInstituicao.BuscarPorId(id);
                AlteracoesEntidadeViewModel model = new AlteracoesEntidadeViewModel()
                {
                    DescricaoEntidade = instituicao.ToString(),
                    Alteracoes = await _servicoAlteracoes.Filtrar(x => x.Entidade == EnumEntidadesSistema.Instituicao && x.IdentificadorEntidade == id)
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
        public async Task<IActionResult> Editar(int idInstituicao)
        {
            Instituicao instituicao = await _servicoInstituicao.BuscarPorId(idInstituicao);
            return  View("Editar", instituicao);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Instituicao instituicao, string Mensagem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        UsuarioADE usuario = await ObterUsuarioLogado();
                        instituicao.Logo = GetByteArrayFromFile(instituicao.LogoFile);
                        await _servicoInstituicao.AtualizarAsync(usuario, instituicao, Mensagem);
                        await unitOfWork.Commit();
                        ViewBag.Retorno = "Instituicao alterada com sucesso!";
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (_servicoInstituicao.ListarAsync().Result.Any(e => e.Identificador == instituicao.Identificador))
                            return NotFound();
                        else
                            throw;
                    }
                    return await Editar(instituicao.Identificador);
                }
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source + "-" + ex.TargetSite.Name, EnumTipoLog.AlteracaoInstituicao);
                ModelState.AddModelError("Falha", "Ocorreu um erro ao alterar a Instituição, contate o suporte para maior exclarecimento");
            }

            return await Editar(instituicao.Identificador);
        }
    }
}
