using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Assistente_de_Estagio.Models;
using ADE.Dominio.Models;
using ADE.Aplicacao.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using ADE.Principal.Models;
using Microsoft.AspNetCore.Identity;
using ADE.Dominio.Models.Individuais;
using Assistente_de_Estagio.Areas.Shared;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using ADE.Apresentacao.Areas.Shared;
using ADE.Infra.Data.UOW;
using ADE.Dominio.Models.Enums;
using Assistente_de_Estagio.Data;
using ADE.Dominio.Interfaces;
using Assistente_de_Estagio.Areas.Principal.Models;
using Newtonsoft.Json;
using ADE.Dominio.Models.RelacaoEntidades;
using ADE.Utilidades.Constants;

namespace Assistente_de_Estagio.Controllers
{
    [Authorize]
    [Area("Principal")]
    public class ListagemDocumentosController : BaseController
    {
        private UnitOfWork unitOfWork;
        private ApplicationDbContext context;
        private readonly ServicoDocumento _documentoServices;
        private readonly ServicoRequisito _servicoRequisito;
        private readonly ServicoCurso _cursoServices;
        private readonly ServicoInstituicao _servicoInstituicao;
        private readonly IServicoInformacao<InformacaoCurso> _informacaoCursoServices;
        private readonly IServicoInformacao<InformacaoDocumento> _informacaoDocumentoServices;
        private readonly ServicoHistoricoGeracaoDocumento _historicoGeracaoServices;
        private readonly ServicoAtividadeEstagio _atividadeEstagioServices;
        private readonly ServicoAreaEstagioCurso _servicoAreaEstagioCurso;

        public ListagemDocumentosController(
            IHostingEnvironment env,
            UserManager<UsuarioADE> userManager,
            ApplicationDbContext _context
            ) : base(new UnitOfWork(_context), userManager)
        {
            context = _context;
            unitOfWork = new UnitOfWork(_context);
            _documentoServices = new ServicoDocumento(ref unitOfWork, env);
            _servicoInstituicao = new ServicoInstituicao(ref unitOfWork);
            _cursoServices = new ServicoCurso(ref unitOfWork);
            _historicoGeracaoServices = new ServicoHistoricoGeracaoDocumento(ref unitOfWork);
            _informacaoCursoServices = new ServicoInformacaoCurso(ref unitOfWork);
            _informacaoDocumentoServices = new ServicoInformacaoDocumento(ref unitOfWork);
            _atividadeEstagioServices = new ServicoAtividadeEstagio(ref unitOfWork);
            _servicoAreaEstagioCurso = new ServicoAreaEstagioCurso(ref unitOfWork);
            _servicoRequisito = new ServicoRequisito(ref unitOfWork);
        }
        [HttpGet]
        public async Task<IActionResult> VisualizacaoDocumentosCurso(int idCurso, string Mensagem = "", int? pageNumber = 1)
        {
            try
            {
                ViewBag.Retorno = Mensagem;
                
                if (UsuarioValido())
                {
                    UsuarioADE Usuario = await ObterUsuarioLogado();
                    if (!Usuario.PossuiInstituicao())
                        RedirecionarPaginaInstituicao();

                    SelecaoViewModel VModel = await this.ParseListagemViewModelAsync(idCurso, Usuario, pageNumber);

                    if (!Usuario.PossuiCurso())
                        VModel.PrimeiroCurso = true;

                    return View("VisualizacaoDocumentosCurso", VModel);
                }
                else
                {
                    ModelState.AddModelError("Falha", "Usuário não está autenticado");
                    return RedirectToAction("Logout", "Account");
                }
            }
            catch(Exception ex)
            {
                await LogError(ex.Message, ex.Source, EnumTipoLog.ErroInterno);
                return RedirectToAction("Index", "Account");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Materiais()
        { 
            try
            {
                if (UsuarioValido())
                {
                    UsuarioADE Usuario = await ObterUsuarioLogado();
                    if (!Usuario.PossuiInstituicao())
                        return RedirecionarPaginaInstituicao(true);

                    SelecaoViewModel VModel = await this.ParseListagemViewModelAsync(Usuario.IdCurso, Usuario, 1);

                    if (!Usuario.PossuiCurso())
                        VModel.PrimeiroCurso = true;

                    return PartialView("_Materiais", VModel);
                }
                else
                {
                    ModelState.AddModelError("Falha", "Usuário não está autenticado");
                    return RedirectToAction("Logout", "Account");
                }
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source, EnumTipoLog.ErroInterno);
                return RedirectToAction("Index", "Account");
            }
        }

        private IActionResult RedirecionarPaginaInstituicao(bool partial = false)
        {
            return RedirectToAction("Index", "Instituicao", new { area = "Principal", Partial = partial } );
        }

        public async Task<IActionResult> RedirecionarNovoCurso(int idCurso, string idUsuario)
        {
            try
            {
                UsuarioADE usuario = await ObterUsuarioPorEmailOuId(idUsuario);
                Curso curso = await _cursoServices.BuscarPorId(idCurso);
                ViewBag.Retorno = $"Curso alterado para {curso.Nome()}";
                return RedirectToAction("VisualizacaoDocumentosCurso", "ListagemDocumentos", new { area = "Principal", Mensagem = ViewBag.Retorno });
            }
            catch(Exception ex)
            {
                await LogError(ex.Message, ex.Source, EnumTipoLog.ErroInterno);
                return RedirectToAction("Index", "Account");
            }
        }

        #region PARSING
        public async Task<SelecaoViewModel> ParseListagemViewModelAsync(int idCurso, UsuarioADE usuario, int? pageNumber, string AssinaturaDocumento = "", string TipoDocumento = "", string Etapa = "")
        {
            SelecaoViewModel model = new SelecaoViewModel();
            idCurso = idCurso == 0 ? usuario.IdCurso : idCurso;
            
            model.Documentos = await _documentoServices.ListarPorCurso(usuario.IdCurso);
            model.Curso = await _cursoServices.BuscarPorId(idCurso);
            model.Curso.Instituicao = await _servicoInstituicao.BuscarPorId(model.Curso.IdInstituicao);
            model.Cursos = await ObterListaCursos(usuario, pageNumber);
            model.HistoricoGeracaoDocumento = await _historicoGeracaoServices.RecuperarHistoricoDoUsuario(usuario.Id);
            model.InformacaoCurso = await _informacaoCursoServices.RecuperarInformacao(idCurso);
            model.DocumentoViewModel = new DocumentoViewModel();
            model.Paginated =  PaginatedList<Documento>.Create(model.Documentos.AsQueryable(), pageNumber ?? 1, 8);
            return model;
        }

        private async Task<PaginatedList<InformacaoCursoVM>> ObterListaCursos(UsuarioADE usuario, int? pageNumber)
        {
            List<Curso> ListaCursos = await _cursoServices.Filtrar(x => x.IdInstituicao == usuario.IdInstituicao);
            List<InformacaoCursoVM> model = new List<InformacaoCursoVM>();
            foreach (Curso curso in ListaCursos)
            {
                curso.Instituicao = await _servicoInstituicao.BuscarPorId(curso.IdInstituicao);
                int QuantidadeAlunosCurso = await CountUsuarioByCurso(curso.Identificador);
                int QuantidadeDocumentosCurso = await _documentoServices.CountByCurso(curso.Identificador);
                InformacaoCursoVM InfoCurso = new InformacaoCursoVM(curso, QuantidadeAlunosCurso, QuantidadeDocumentosCurso);
                if (curso.Identificador == usuario.IdCurso)
                    InfoCurso.CursoDoUsuario = true;
                model.Add(InfoCurso);
            }

            PaginatedList<InformacaoCursoVM> lista = PaginatedList<InformacaoCursoVM>.Create(model.AsQueryable(), pageNumber ?? 1, 6);
            return lista;
        }
        #endregion

        #region [MÉTODOS AJAX]
        [HttpGet]
        public async Task<IActionResult> ObterRequisitosDeDocumento(int idDocumento)
        {
            UsuarioADE Usuario = await ObterUsuarioLogado();
            if(!Usuario.AceitouTermos)
                return PartialView("_TermosDeUso");

            Documento Documento = await _documentoServices.BuscarPorId(idDocumento);
            Curso Curso = await _cursoServices.BuscarPorId(Documento.IdCurso);
            Documento.IdCursoNavigation = Curso;
            List<Requisito> Requisitos = await _documentoServices.ObterRequisitosDeDocumento(idDocumento, Usuario.Id);
            List<InformacaoDocumento> informacaoDocumento = await _informacaoDocumentoServices.RecuperarInformacao(idDocumento);
            DocumentoViewModel DVM = new DocumentoViewModel(Documento, Requisitos, informacaoDocumento);
            if (Requisitos.Any(x => x.Bookmark == NomesRequisitos.AreasEstagio_) || Requisitos.Any(x=>x.Bookmark == NomesRequisitos.TabelaAreasEstagio_))
            {
                DVM.AreasEstagioCurso = await _servicoAreaEstagioCurso.Filtrar(x => x.IdCurso == Usuario.IdCurso);
            }

            return PartialView("_TelaPreenchimento", DVM);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadDocumento([FromQuery] DocumentoRequest documentoRequest)
        {
            Documento Documento = await _documentoServices.BuscarPorId(documentoRequest.idDocumento);
            Documento.IdCursoNavigation = await _cursoServices.BuscarPorId(Documento.IdCurso);
            Documento.IdCursoNavigation.Instituicao = await _servicoInstituicao.BuscarPorId(Documento.IdCursoNavigation.IdInstituicao);
            UsuarioADE Usuario = await ObterUsuarioLogado();

            int TentativasDownload = 3;
            while (TentativasDownload > 0)
            {
                try
                {
                    if (Enum.TryParse(documentoRequest.MimeType, out EnumFormatoDocumento formatoDocumento))
                    {
                        List<DadosAlunoKV> dadosAlunoKV = JsonConvert.DeserializeObject<List<DadosAlunoKV>>(documentoRequest.dadosAluno);
                        foreach(DadosAlunoKV dado in dadosAlunoKV)
                        {
                            int id = Convert.ToInt32(dado.Name.Split(':')[0]);
                            dado.Requisito = await _servicoRequisito.BuscarUm(x=>x.Identificador == id);
                        }
                        ArquivoDownload Arquivo;
                        if (Documento.Tipo != EnumTipoDocumento.Questionario)
                        {
                            Arquivo = await _documentoServices.GerarDownloadDocumento(dadosAlunoKV, Documento, Usuario, formatoDocumento);
                        }
                        else
                        {
                            RequisitosBasicosCabecalho requisitosBasicos = await ObterRequisitosBasicosUsuario(Usuario);
                            Arquivo = await _documentoServices.GerarDownloadQuestionario(dadosAlunoKV, Documento, Usuario, requisitosBasicos, formatoDocumento);
                        }
                        if(TentativasDownload == 3)
                        await _atividadeEstagioServices.VerificarTarefasEConcluir(Usuario, EnumEntidadesSistema.Documento, documentoRequest.idDocumento, EnumTipoAtividadeEstagio.DownloadOuImpressao, 1);
                        
                        string handle = Guid.NewGuid().ToString();
                        TempData[handle] = Arquivo.Bytes.ToArray();
                        string data = JsonConvert.SerializeObject(new { FileGuid = handle, FileName = $"{Documento.TituloDocumento}.{documentoRequest.MimeType}" });
                        return Json(data);
                    }
                    else
                    {
                        ModelState.AddModelError("Falha", "Formato inválido para documento");
                        return Json("{\"Erro\": \"Formato inválido para documento\"}");
                    }
                }
                catch (Exception ex) when (TentativasDownload > 0)
                {
                    TentativasDownload -= 1;
                    System.Threading.Thread.Sleep(3000);
                    await LogError($"{ex.Message}", "ListagemDocumento", EnumTipoLog.ImpressaoArquivo);
                }
            }
            ViewBag.Retorno = "Recurso indisponivel no momento, tente novamente em alguns instantes.";
            ModelState.AddModelError("Falha", ViewBag.Retorno);
            return await VisualizacaoDocumentosCurso(Documento.IdCurso, "Recurso indisponivel no momento, tente novamente em alguns instantes.", 1);
        }

        [HttpGet]
        public virtual IActionResult Download(string fileGuid, string fileName, string MimeType)
        {
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, MimeType, fileName);
            }
            else
            {
                return Json("{\"Erro\": \"Erro ao obter documento, tente novamente. Caso o problema volte a acontecer, recomenda-se recarregar a página.\"}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> PDF(int IdDocumento)
        {
            Documento doc = await _documentoServices.BuscarPorId(IdDocumento);
            return File(doc.ArquivoPDF, "application/pdf", doc.TituloDocumento);
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

public class DocumentoRequest
{
    public string dadosAluno {get;set;}
    public int idDocumento { get; set; }
    public string MimeType {get;set;}

}