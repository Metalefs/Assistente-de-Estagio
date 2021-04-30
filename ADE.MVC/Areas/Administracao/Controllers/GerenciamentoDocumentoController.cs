using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ADE.Aplicacao.Services;
using ADE.Apresentacao.Areas.Shared;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using ADE.Utilidades.Handlers;
using Assistente_de_Estagio.Areas.Administracao.Models;
using Assistente_de_Estagio.Areas.Shared;
using Assistente_de_Estagio.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ADE.Apresentacao.Areas.Administracao.Controllers
{
    [Authorize(Roles = "Admin,CriadorConteudo")]
    [Area("Administracao")]
    public class GerenciamentoDocumentoController : BaseController
    {
        static UnitOfWork unitOfWork;
        private ServicoDocumento _documentoServices;
        private ServicoCurso _cursoServices;
        private ServicoRequisito _requisitoServices;
        private ServicoRequisitoDeDocumento _requisitoDocumentoServices;
        private ServicoInstituicao _servicoInstituicao;
        private readonly ServicoAlteracaoEntidadesSistema _servicoAlteracoes;

        private readonly int pageSize = 45;
        public GerenciamentoDocumentoController(
            IHostingEnvironment env,
            UserManager<UsuarioADE> userManager,
            SignInManager<UsuarioADE> signInManager,
            ApplicationDbContext _context
            ) : base(unitOfWork = new UnitOfWork(_context), userManager, signInManager)
        {
            _documentoServices = new ServicoDocumento(ref unitOfWork, env);
            _cursoServices = new ServicoCurso(ref unitOfWork);
            _requisitoServices = new ServicoRequisito(ref unitOfWork);
            _requisitoDocumentoServices = new ServicoRequisitoDeDocumento(ref unitOfWork);
            _servicoAlteracoes = new ServicoAlteracaoEntidadesSistema(ref unitOfWork);
            _servicoInstituicao = new ServicoInstituicao(ref unitOfWork);
        }

        [HttpGet]
        public async Task<IActionResult> Index(bool Partial = false)
        {
            
            List<Documento> ListaDocumentos = await _documentoServices.ListarAsync();
            ListaDocumentos = await _documentoServices.ListarAsync();
            ListaDocumentos = ListaDocumentos.OrderByDescending(x => x.PosicaoDocumento).ToList();
            GerenciamentoDocumentoViewmodel GDVM = await ParseGerenciamentoDocumento(ListaDocumentos, 1);

            if(Partial)
                return PartialView("_Index",GDVM);

            return View(GDVM);
        }

        private async Task<GerenciamentoDocumentoViewmodel> ParseGerenciamentoDocumento(List<Documento> Documentos, int? pageNumber)
        {
            PaginatedList<Documento> documentos = PaginatedList<Documento>.Create(Documentos.AsQueryable(), pageNumber ?? 1, pageSize);
            List<Curso> cursos = await _cursoServices.ListarAsync();
            List<Instituicao> instituicoes = await _servicoInstituicao.ListarAsync();
            return new GerenciamentoDocumentoViewmodel(documentos, cursos, instituicoes);
        }

        private async Task<GerenciamentoDocumentoViewmodel> ParseGerenciamentoDocumento()
        {
            List<Documento> ListaDocumentos = await _documentoServices.ListarAsync();
            PaginatedList<Documento> documentos = PaginatedList<Documento>.Create(ListaDocumentos.AsQueryable(), 1, pageSize);
            List<Curso> cursos = await _cursoServices.ListarAsync();
            List<Instituicao> instituicoes = await _servicoInstituicao.ListarAsync();
            return new GerenciamentoDocumentoViewmodel(documentos, cursos, instituicoes);
        }

        [HttpGet]
        public async Task<IActionResult> Criar(int IdInstituicao, int IdCurso)
        {
            try
            {
                List<Curso> listaCursos = await _cursoServices.ListarAsync();
                List<Instituicao> listaInstituicoes = await _servicoInstituicao.ListarAsync();
                CriarDocumentoViewModel CDVM = new CriarDocumentoViewModel(new Documento(), IdCurso, listaCursos, IdInstituicao, listaInstituicoes);
                return View("Criar",CDVM);
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source + "-" + ex.TargetSite.Name, EnumTipoLog.CriacaoDocumento);
                ModelState.AddModelError("Falha", "Ocorreu um erro ao montar a página de criação -"+ ex.Message);
                ViewBag.Retorno = "Ocorreu um erro ao montar a página de criação, contate o suporte para maior exclarecimento";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> VisualizarEntidade(int id)
        {
            try
            {
                VisualizarEntidadeViewmodel<Documento> model = new VisualizarEntidadeViewmodel<Documento>()
                {
                    Entidade = await _documentoServices.BuscarPorId(id),
                    ListaAlteracoes = await _servicoAlteracoes.Filtrar(x => x.Entidade == EnumEntidadesSistema.Documento && x.IdentificadorEntidade == id)
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
                Documento documento = await _documentoServices.BuscarPorId(id);
                AlteracoesEntidadeViewModel model = new AlteracoesEntidadeViewModel()
                {
                    DescricaoEntidade = documento.ToString(),
                    Alteracoes = await _servicoAlteracoes.Filtrar(x => x.Entidade == EnumEntidadesSistema.Documento && x.IdentificadorEntidade == id)
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
        public async Task<IActionResult> Editar(int idDocumento,int idCurso)
        {
            try
            {
                List<Curso> Cursos = await _cursoServices.ListarAsync();
                List<RequisitoDeDocumento> Requisitos = await ObterRequisitosDeDocumento(idDocumento);
                Documento documento = await _documentoServices.BuscarPorId(idDocumento);
                AlterarDocumentoViewModel CDVM = new AlterarDocumentoViewModel(documento, idCurso, Cursos, Requisitos);
                return View("Editar",CDVM);
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source + "-" + ex.TargetSite.Name, EnumTipoLog.CriacaoDocumento);
                ModelState.AddModelError("Falha", "Ocorreu um erro ao montar a página de edição -" + ex.Message);
                ViewBag.Retorno = "Ocorreu um erro ao montar a página de edição, contate o suporte para maior exclarecimento";
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task<List<RequisitoDeDocumento>> ObterRequisitosDeDocumento(int idDocumento)
        {
            List<RequisitoDeDocumento> lista = await _requisitoDocumentoServices.ListarRegistros(idDocumento);
            foreach (RequisitoDeDocumento req in lista)
            {
                if(req.IdRequisitoNavigation == null)
                req.IdRequisitoNavigation = await _requisitoServices.BuscarPorId(req.IdRequisito);
            }
            return lista;
        }

        [HttpPost]
        public async Task<IActionResult> RemoverRequisitoDocumento(int IdDocumento, int IdRequisito)
        {
            try
            {
                Documento CursoDocumento = await _documentoServices.BuscarPorId(IdDocumento);
                try
                {
                    UsuarioADE usuario = await ObterUsuarioLogado();
                    RequisitoDeDocumento requisitoDeDocumento = await _requisitoDocumentoServices.BuscarPorId(IdDocumento, IdRequisito);
                    await _requisitoDocumentoServices.RemoverAsync(usuario, requisitoDeDocumento);
                    ViewBag.Retorno = $"Requisito de documento removido com sucesso.";
                    return await Editar(CursoDocumento.Identificador, CursoDocumento.IdCurso);
                }
                catch (Exception ex)
                {
                    await LogError(ex.Message, ex.Source + "-" + ex.TargetSite.Name, EnumTipoLog.AlteracaoDocumento);
                    ModelState.AddModelError("Falha", "Ocorreu um erro ao remover o requisito, contate o suporte para maior exclarecimento");
                    ViewBag.Retorno = "Ocorreu um erro ao remover o requisito, contate o suporte para maior exclarecimento";
                    return await Editar(CursoDocumento.Identificador, CursoDocumento.IdCurso);
                }
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source + "-" + ex.TargetSite.Name, EnumTipoLog.AlteracaoDocumento);
                ModelState.AddModelError("Falha", "Ocorreu um erro ao remover o requisito, contate o suporte para maior exclarecimento");
                ViewBag.Retorno = "Ocorreu um erro ao remover o requisito, contate o suporte para maior exclarecimento";
                return RedirectToRoute("Error","400");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditarDocumento(AlterarDocumentoViewModel model, string Mensagem)
        {
            try
            {
                if (!ModelState.HasReachedMaxErrors)
                {
                    UsuarioADE ADEUser = await ObterUsuarioLogado();
                    await _documentoServices.AtualizarAsync(ADEUser, model.Documento, Mensagem);
                    await unitOfWork.Commit();
                    ViewBag.Retorno = "Documento alterado com sucesso!";
                }
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source + "-" + ex.TargetSite.Name, EnumTipoLog.AlteracaoDocumento);
                ModelState.AddModelError("Falha", "Ocorreu um erro ao alterar o documento, contate o suporte para maior exclarecimento");
                return await Editar(model.Documento.Identificador, Convert.ToInt32(model.IdCurso));
            }
            return await Editar(model.Documento.Identificador,Convert.ToInt32(model.IdCurso));
        }
        
        [HttpPost]
        public async Task<IActionResult> CriarDocumento(CriarDocumentoViewModel model)
        {
            try
            {
                UsuarioADE ADEUser = await ObterUsuarioLogado();
                
                if (ArquivoDocXValido(model.Arquivo))
                {
                    model.Documento.IdCurso = Convert.ToInt32(model.IdCurso);

                    try
                    {
                        await _documentoServices.AdcionarArquivoAoSistemaECadastrarAsync(model.Arquivo, model.ArquivoPDF, model.Documento, ADEUser);
                        await unitOfWork.Commit();
                        ViewBag.Retorno = "Documento cadastrado com sucesso!";
                    }
                    catch (FileFormatException ex)
                    {
                        await LogError(ex.Message, ex.Source + "-" + ex.TargetSite.Name, EnumTipoLog.AlteracaoDocumento);
                        ModelState.AddModelError("Falha", "O Formato do arquivo enviado foi disputado");
                        ViewBag.Retorno = "O Formato do arquivo enviado foi disputado";
                    }
                }
                return View("Criar", new { IdInstituicao = model.IdInstituicao, IdCurso = model.IdCurso });

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Erro", ex.Message);
                return View("Criar", new { model.IdInstituicao, model.IdCurso });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChecarDocumento(IFormFile file)
        {
            Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            var Documento = file ?? Request.Form.Files.First();
            try
            {
                if (ArquivoDocXValido(Documento))
                {
                    string JsonComparacaoRequisitos = await CreateDocumentFileStreamFromIFormFileAndChecarRequisitos(Documento);
                    return Json(JsonComparacaoRequisitos);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Erro", ex.Message);
                return Json("{\"Erro\": \"Erro ao Checar documento : "+ ex.Message + "\"}");
            }
            return Json("{\"Erro\": \"Erro ao Checar documento\"}");
        }

        private async Task<string> CreateDocumentFileStreamFromIFormFileAndChecarRequisitos(IFormFile Documento)
        {
            using (Stream fileStream = new MemoryStream())
            {
                await Documento.CopyToAsync(fileStream);
                ComparacaoRequisitos comparacao = await _documentoServices.ChecarRequisitosDocumento(fileStream);
                string texto = _documentoServices.ObterTextoDocumento(fileStream);
                string textoHTML = _documentoServices.ObterTextoDocumentoHTML(fileStream);
                RetornoUploadDocumento retornoUpload = new RetornoUploadDocumento(texto,textoHTML,comparacao);
                return JsonConvert.SerializeObject(retornoUpload);
            }
        }

        private bool ArquivoDocXValido(IFormFile Documento) => Documento != null && FileHandler.ArquivoDocXValido(Documento);

        public async Task<IActionResult> ExcluirDocumentoConfirmed(int id)
        {
            UsuarioADE ADEUser = await ObterUsuarioLogado();

            var documento = await _documentoServices.BuscarPorId(id);
            await _documentoServices.RemoverAsync(ADEUser, documento);
            return RedirectToAction(nameof(Index));
        }
    }
}