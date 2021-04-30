using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADE.Aplicacao.Services;
using ADE.Apresentacao.Areas.Shared;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using Assistente_de_Estagio.Areas.Shared;
using Assistente_de_Estagio.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Assistente_de_Estagio.Areas.Principal.Models;
using Assistente_de_Estagio.Controllers;
using Microsoft.AspNetCore.Hosting;

namespace ADE.Apresentacao.Areas.Principal.Controllers
{
    [Authorize]
    [Area("Principal")]
    public class ListagemCursosController : ListagemDocumentosController
    {
        readonly ApplicationDbContext context;
        private UnitOfWork unitOfWork;
        private ServicoCurso _cursoServices;
        private ServicoDocumento _servicoDocumento;
        private ServicoInstituicao _servicoInstituicao;
        private readonly int pageSize = 6;

        public ListagemCursosController(
            UserManager<UsuarioADE> userManager,
            SignInManager<UsuarioADE> signInManager,
            ApplicationDbContext _context,
            IHostingEnvironment _env
            ) : base(_env, userManager, _context)
        {
            context = _context;
            unitOfWork = new UnitOfWork(context);
        }
        public async Task<IActionResult> ListagemCursos(int? pageNumber, int? idInstituicao, string ErrorMessage = null)
        {
            try
            {
                if(ErrorMessage != null)
                {
                    ModelState.AddModelError("Falha",ErrorMessage);
                }
                if (User != null && User.Identity.IsAuthenticated)
                {
                    UsuarioADE usuario = await ObterUsuarioLogado();
                    idInstituicao = idInstituicao == null ? usuario.IdInstituicao : idInstituicao;
                    PaginatedList<InformacaoCursoVM> model = await ParseInformacaoCursoVM(usuario, idInstituicao, pageNumber);
                    return View("VisualizacaoCursosInstituicao", model);
                }
                else
                {
                    ModelState.AddModelError("Falha", "Usuário não está autenticado");
                    return RedirectToAction("Logout", "Account");
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Falha", "Ocorreu um erro ao listar os cursos disponiveis");
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                return RedirectToAction("Index", "Account");
            }
        }

        public async Task<PaginatedList<InformacaoCursoVM>> ParseInformacaoCursoVM(UsuarioADE usuario,int? idInstituicao, int? pageNumber)
        {
            _cursoServices = new ServicoCurso(ref unitOfWork);
            _servicoInstituicao = new ServicoInstituicao(ref unitOfWork);
            _servicoDocumento = new ServicoDocumento(ref unitOfWork);
            List<Curso> ListaCursos = await _cursoServices.Filtrar(x => x.IdInstituicao == idInstituicao);
            List<InformacaoCursoVM> model = new List<InformacaoCursoVM>();
            foreach (Curso curso in ListaCursos)
            {
                curso.Instituicao = await _servicoInstituicao.BuscarPorId(curso.IdInstituicao);
                int QuantidadeAlunosCurso = await CountUsuarioByCurso(curso.Identificador);
                int QuantidadeDocumentosCurso = await _servicoDocumento.CountByCurso(curso.Identificador);
                InformacaoCursoVM InfoCurso = new InformacaoCursoVM(curso, QuantidadeAlunosCurso, QuantidadeDocumentosCurso);
                if (curso.Identificador == usuario.IdCurso)
                    InfoCurso.CursoDoUsuario = true;
                model.Add(InfoCurso);
            }

            PaginatedList<InformacaoCursoVM> lista = PaginatedList<InformacaoCursoVM>.Create(model.AsQueryable(), pageNumber ?? 1, pageSize);
            return lista;
        }

        public async Task<IActionResult> ObterResultadoPesquisaCurso(string NomeCurso)
        {
            UsuarioADE usuario = await ObterUsuarioLogado();
            _cursoServices = new ServicoCurso(ref unitOfWork);
            _servicoDocumento = new ServicoDocumento(ref unitOfWork);
            ServicoInstituicao _servicoInstituicao = new ServicoInstituicao(ref unitOfWork);
            Instituicao instituicaoDoUsuario = await _servicoInstituicao.BuscarPorId(usuario.IdInstituicao);
            try
            {
                List<Curso> ListaCursos = await _cursoServices.Filtrar(x=> x.NomeCurso.Contains(NomeCurso));
                List<InformacaoCursoVM> model = new List<InformacaoCursoVM>();
                foreach (Curso curso in ListaCursos)
                {
                    int quantidadeDocumentos = 0;
                    int quantidadeAlunos = 0;

                    try
                    {
                        quantidadeDocumentos = await _servicoDocumento.CountByCurso(curso.Identificador);
                    }
                    catch(Exception ex)
                    {
                        await LogError($"Erro ao obter contagem de documentos para o curso {curso.ToString()} : {ex.Message}", ex.Source, Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                    }

                    try
                    {
                        quantidadeAlunos = await CountUsuarioByCurso(curso.Identificador);
                    }
                    catch (Exception ex)
                    {
                        await LogError($"Erro ao obter contagem de usuários para o curso {curso.ToString()} : {ex.Message}", ex.Source, Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                    }
                    curso.Instituicao = await _servicoInstituicao.BuscarPorId(curso.IdInstituicao);
                    InformacaoCursoVM InfoCurso = new InformacaoCursoVM(curso, quantidadeAlunos, quantidadeDocumentos);
                    if (curso.Identificador == usuario.IdCurso)
                        InfoCurso.CursoDoUsuario = true;
                    model.Add(InfoCurso);
                }
                if (ListaCursos.Count > 0)
                {
                    PaginatedList<InformacaoCursoVM> lista = PaginatedList<InformacaoCursoVM>.Create(model.AsQueryable(), 1, pageSize);
                    return PartialView("_SelecaoCurso_CursoOption", lista);
                }
                else
                    throw new Exception($"O resultado da pesquisa por curso está vazio : {NomeCurso}");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Falha", $"Ocorreu um erro ao buscar cursos compatíveis com a pesquisa : {NomeCurso}");
                await LogError(ex.Message, "ObterResultadoPesquisaCurso", Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                return Json("{\"Erro\": \"Pesquisa não retornou nenhum curso\"}");
            }
        }

        public async Task<IActionResult> TrocarCurso(int idCurso)
        {
            try
            {
                _cursoServices = new ServicoCurso(ref unitOfWork);
                Curso curso = await _cursoServices.BuscarPorId(idCurso);
                UsuarioADE usuario = await ObterUsuarioLogado();
                usuario.Email = User.Identity.Name;
                usuario.Id = usuario.Id;
                usuario.UserName = usuario.UserName;
                usuario.IdCurso = curso.Identificador;
                if (usuario.IdInstituicao == curso.IdInstituicao)
                {
                    await AtualizarUsuario(usuario);
                    return RedirectToAction("Principal/RedirecionarNovoCurso", "Instituicao", new { Area = "Principal", idCurso, idUsuario = usuario.Id });
                }
                else
                {
                    ModelState.AddModelError("Falha","Usuário não está cadastrado nesta instituição");
                    ViewBag.Retorno = "Usuário não está cadastrado nesta instituição";
                    return RedirectToAction("ListagemCursos", "Instituicao", new { area = "Principal", pageNumber = 1, idInstituicao = curso.IdInstituicao, ErrorMessage = ViewBag.Retorno });
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Falha", "Ocorreu um erro alterar o curso do usuário");
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.AlteracaoCursoUsuario);
                return RedirectToAction("Index", "Account");
            }
        }
    }
}