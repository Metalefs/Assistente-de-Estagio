using System;
using System.Collections.Generic;
using System.Linq;
using ADE.Dominio.Interfaces;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using static ADE.Utilidades.Handlers.FileHandler;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Novacode;
using ADE.Dominio.Models.Individuais;
using ADE.GeradorArquivo.Word;
using System.Threading.Tasks;
using ADE.Utilidades.Constants;
using ADE.Infra.Data.UOW;

namespace ADE.Aplicacao.Services
{
    public class ServicoDocumento : ServicoBase<Documento>, IServicoBase<Documento>, ICountable
    {
        private UnitOfWork unitOfWork;
        private ServicoCurso _cursoServices;
        private ServicoInstituicao _servicoInstituicao;
        private ServicoRequisitoDeDocumento _servicoRequisitoDeDocumento;
        private ServicoSysLogs _loggerServices;
        private ServicoHistoricoGeracaoDocumento _servicoHistoricoGeracaoDocumento;
        private ServicoRequisitoUsuario _servicoRequisitoUsuario;
        private ServicoRequisito _servicoRequisito;
        private ServicoAreaEstagioCurso _servicoAreaEstagioCurso;

        private IHostingEnvironment _env;

        private UtilidadesDocX UtilidadesDocX;
        private MontadorDocX GeradorDocumento;
        public ServicoDocumento(ref UnitOfWork _unitOfWork,
            IHostingEnvironment env = null
            ) : base(ref _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            _env = env;
            _cursoServices = new ServicoCurso(ref unitOfWork);
            _servicoInstituicao = new ServicoInstituicao(ref unitOfWork);
            _servicoRequisitoDeDocumento = new ServicoRequisitoDeDocumento(ref unitOfWork);
            _loggerServices = new ServicoSysLogs(ref unitOfWork);
            _servicoHistoricoGeracaoDocumento = new ServicoHistoricoGeracaoDocumento(ref unitOfWork);
            _servicoRequisitoUsuario = new ServicoRequisitoUsuario(ref unitOfWork);
            _servicoRequisito = new ServicoRequisito(ref unitOfWork);
            _servicoAreaEstagioCurso = new ServicoAreaEstagioCurso(ref unitOfWork);
            UtilidadesDocX = new UtilidadesDocX();
            GeradorDocumento = new MontadorDocX();
        }

        public new async Task CadastrarAsync(UsuarioADE usuario, Documento documento)
        {
            await unitOfWork.RepositorioBase<Documento>().Criar(documento);
            await TornarCursoInalterado(documento.IdCurso);
            documento.DataHoraInclusao = DateTime.Now;
            await LogCadastramento(usuario, documento);
            await unitOfWork.Commit();
        }

        public override async Task LogCadastramento(UsuarioADE usuario, Documento documento)
        {
            await LogAcao(usuario, documento, "Documento", EnumTipoLog.CriacaoDocumento, TipoEvento.Criacao);
            Documento confirmacao = await BuscarUm(x => x.TituloDocumento == documento.TituloDocumento);
            confirmacao = confirmacao ?? new Documento() { TituloDocumento = "Erro" };
            await LogAlteracaoEntidade(usuario, confirmacao, documento, EnumEntidadesSistema.Documento, EnumTipoLog.CriacaoDocumento);
        }

        private async Task TornarCursoInalterado(int IdCurso)
        {
            Curso curso = await _cursoServices.BuscarPorId(IdCurso);
            unitOfWork.SetStateUnchanged(curso);
        }

        public override async Task LogAtualizacao(UsuarioADE usuario, Documento DocumentoNovo, string Mensagem = null)
        {
            Documento DocumentoAntigo = await BuscarPorId(DocumentoNovo.Identificador);
            string TituloAntigo = DocumentoAntigo.TituloDocumento;
            await LogAlteracaoEntidade(usuario, DocumentoAntigo, DocumentoNovo, EnumEntidadesSistema.Documento, EnumTipoLog.AlteracaoDocumento, Mensagem);
            DocumentoAntigo.Clonar(DocumentoNovo);
            await LogAtualizacaoComTituloAntigo(usuario, DocumentoAntigo, TituloAntigo);
        }

        private async Task LogAtualizacaoComTituloAntigo(UsuarioADE usuario, Documento documento, string TituloAntigo)
        {
            documento.TituloDocumento = TituloAntigo;
            await LogAcao(usuario, documento, "Documento", EnumTipoLog.AlteracaoDocumento, TipoEvento.Alteracao);
        }

        public override async Task LogRemocao(UsuarioADE usuario, Documento documento)
        {
            await LogAcao(usuario, documento, "Documento", EnumTipoLog.DelecaoDocumento, TipoEvento.Delecao);
            await LogDelecaoEntidade(usuario, documento, EnumEntidadesSistema.Documento, EnumTipoLog.DelecaoDocumento);
        }

        public async Task AdcionarArquivoAoSistemaECadastrarAsync(IFormFile ArquivoDOCX, IFormFile ArquivoPDF, Documento documento, UsuarioADE usuario)
        {
            try
            {
                Curso curso = await ObterCursoDocumento(documento);
                Instituicao instituicao = await ObterInstituicaoDocumentoPeloCurso(curso);
                if (ArquivoValido(ArquivoDOCX))
                {
                    EnumFormatoDocumento formato = ObterFormatoDoIFormFile(ArquivoDOCX);
                    if (ArquivoDocXValido(ArquivoDOCX))
                    {
                        using(MemoryStream ms = new MemoryStream())
                        {
                            await ArquivoDOCX.CopyToAsync(ms);
                            documento.Arquivo = ms.ToArray();
                        }
                        documento.Texto = ObterTextoDocumento(ArquivoDOCX);
                    }
                    else if(ArquivoValido(ArquivoPDF))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await ArquivoPDF.CopyToAsync(ms);
                            documento.ArquivoPDF = ms.ToArray();
                        }
                    }
                }
                else
                {
                    throw new FileFormatException("O Arquivo docx é inválido, o tamanho máximo é de 2 MB.");
                }
                await CadastrarAsync(usuario, documento);
                try
                {
                    List<Documento> Verificacao = await Filtrar(x => x.TituloDocumento == documento.TituloDocumento && x.IdCurso == documento.IdCurso);
                    if (Verificacao != null)
                    {
                        await AdicionarRequisitosDeDocumento(usuario, ArquivoDOCX, Verificacao.FirstOrDefault());
                    }
                }
                catch (Exception ex)
                {
                    await LogError(usuario, ex.Message, ex.StackTrace, EnumTipoLog.ErroInterno);
                }
                await LogAcao(usuario, documento, "Documento", EnumTipoLog.CriacaoDocumento, TipoEvento.Criacao);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private async Task<Instituicao> ObterInstituicaoDocumentoPeloCurso(Curso curso)
        {
            return await _servicoInstituicao.BuscarPorId(curso.IdInstituicao);
        }
        
        private async Task<Curso> ObterCursoDocumento(Documento documento)
        {
            return await _cursoServices.BuscarPorId(documento.IdCurso);
        }

        private EnumFormatoDocumento ObterFormatoDoIFormFile(IFormFile file)
        {
            string Formato = file.ContentType;
            switch (Formato)
            {
                case Constants.DocXMimeType:
                    return EnumFormatoDocumento.docx;
                case Constants.PDFMimeType:
                    return EnumFormatoDocumento.pdf;
            }
            throw new FileFormatException("Content Type Inválido");
        }

        private async Task AdicionarRequisitosDeDocumento(UsuarioADE usuario, IFormFile Arquivo, Documento documento)
        {
            try
            {
                Documento Documento = await BuscarPorId(documento.Identificador);
                using (Stream ms = new MemoryStream()) {
                    Arquivo.CopyToAsync(ms);
                    ms.Position = 0;
                    ms.Seek(0, SeekOrigin.Begin);
                    ComparacaoRequisitos Comparacao = await CompararRequisitosNoDocumentoComBancoDeDados(ms);
                    await CadastrarRequisitosCompativeis(usuario,Comparacao,Documento);
                } 
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private async Task CadastrarRequisitosCompativeis(UsuarioADE usuario, ComparacaoRequisitos comparacao, Documento documento)
        {
            try
            {
                foreach (Requisito req in comparacao.RequisitosCompativeis)
                {
                    RequisitoDeDocumento rdd = new RequisitoDeDocumento();
                    RequisitoDeDocumento Verificacao;
                    rdd.Identificador = await RecuperarCodigoHistoricoGeracaoDocumento();
                    rdd.IdRequisito = req.Identificador;
                    rdd.IdDocumento = documento.Identificador;
                    try
                    {
                        Verificacao = await _servicoRequisitoDeDocumento.BuscarPorId(documento.Identificador, req.Identificador);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    if (Verificacao.IdDocumento == 0)
                        await _servicoRequisitoDeDocumento.CadastrarAsync(usuario, rdd);
                }
                await unitOfWork.Commit();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ComparacaoRequisitos> ChecarRequisitosDocumento(Stream documento)
        {
            ComparacaoRequisitos Comparacao = await CompararRequisitosNoDocumentoComBancoDeDados(documento);
            return Comparacao;
        }

        public string ObterTextoDocumento(Stream documento)
        {
            return UtilidadesDocX.ObterTexto(documento);
        }

        public string ObterTextoDocumento(IFormFile Arquivo)
        {
            using (Stream ms = new MemoryStream())
            {
                Arquivo.CopyToAsync(ms);
                ms.Position = 0;
                ms.Seek(0, SeekOrigin.Begin);
                return UtilidadesDocX.ObterTexto(ms);
            }
        }

        public string ObterTextoDocumentoHTML(Stream documento)
        {
            return UtilidadesDocX.ObterTextoEmHTML(documento);
        }

        public async Task<ComparacaoRequisitos> ChecarRequisitosDocumento(string CaminhoDocumento)
        {
            ComparacaoRequisitos Comparacao = await CompararRequisitosNoDocumentoComBancoDeDados(CaminhoDocumento);
            return Comparacao;
        }

        private async Task <ComparacaoRequisitos> CompararRequisitosNoDocumentoComBancoDeDados(Stream documento)
        {
            List<Requisito> Lista = new List<Requisito>();
            try
            {
                Lista = await _servicoRequisito.ListarAsync();
            }
            catch (Exception)
            {
            }

            ComparacaoRequisitos Comparacao = new ComparacaoRequisitos
            {
                RequisitosExistentes = Lista,
                RequisitosNovos = ObterBookmarksDocumento(documento)
            };

            if (Comparacao.IsValidForComparison())
            {
                Comparacao.ObterComparacaoRequisitosCompativeis();
                Comparacao.DiferencaNovosEExistentes();
            }

            return Comparacao;
        }

        private async Task<ComparacaoRequisitos> CompararRequisitosNoDocumentoComBancoDeDados(string CaminhoDocumento)
        {
            ComparacaoRequisitos Comparacao = new ComparacaoRequisitos
            {
                RequisitosExistentes = await _servicoRequisito.ListarAsync(),
                RequisitosNovos = ObterBookmarksDocumento(CaminhoDocumento)
            };

            if (Comparacao.IsValidForComparison())
            {
                Comparacao.ObterComparacaoRequisitosCompativeis();
                Comparacao.DiferencaNovosEExistentes();
            }

            return Comparacao;
        }

        public List<string> ObterBookmarksDocumento(Stream documento)
        {
            List<string> BookmarksDocumento = new List<string>();
            DocX documentX = DocX.Load(documento);
            documentX.Bookmarks.ForEach(x => BookmarksDocumento.Add(x.Name));
            return BookmarksDocumento;
        }

        public List<string> ObterBookmarksDocumento(string CaminhoDocumento)
        {
            List<string> BookmarksDocumento = new List<string>();
            DocX documentX = DocX.Load(CaminhoDocumento);
            documentX.Bookmarks.ForEach(x => BookmarksDocumento.Add(x.Name));
            return BookmarksDocumento;
        }

        public async Task<List<Documento>> ListarPorCurso(int? IdCurso)
        {
            try
            {
                List<Documento> Documentos = await Filtrar(x => x.IdCurso == IdCurso);
                Documentos = Documentos.OrderBy(x => x.PosicaoDocumento).ToList<Documento>();
                return Documentos;
            }
            catch (Exception ex)
            {
                SysLogs Error = new SysLogs("Não foi possível Listar Por Curso | "+ ex, "DocumentoServices/ListarPorCurso",EnumTipoLog.CriacaoArquivo);
                await _loggerServices.CadastrarAsync(new UsuarioADE(),Error);
                return new List<Documento>();
            }
        }

        public async Task<int> ContarPorCurso(int? IdCurso)
        {
            try
            {
                List<Documento> Documentos = await Filtrar(x => x.IdCurso == IdCurso);
                return Documentos.Count;
            }
            catch (Exception ex)
            {
                SysLogs Error = new SysLogs("Não foi possível Listar Por Curso | "+ ex, "DocumentoServices/ListarPorCurso",EnumTipoLog.CriacaoArquivo);
                await _loggerServices.CadastrarAsync(new UsuarioADE(),Error);
                return 0;
            }
        }

        public async Task<List<Documento>> Filtrar(EnumAssinaturaDocumento Assinatura, EnumTipoDocumento Tipo, EnumEtapaDocumento Etapa)
        {
            return await unitOfWork.RepositorioBase<Documento>().Filtrar(x=> x.Assinatura == Assinatura || x.Tipo == Tipo || x.Etapa == Etapa);
        }

        public async new Task<List<Documento>> ListarAsync()
        {
            List<Documento> documento = await unitOfWork.RepositorioBase<Documento>().ListarAsync();
            return documento.OrderBy(x => x.PosicaoDocumento).ToList(); 
        }

        #region [DocX]
        /// <summary>
        ///     Gera Download de um documento preenchido com os dados enviados pelo usuário
        /// </summary>
        /// <param name="dadosJson">json key/value pair with {name: "x", value:"y"}</param>
        /// <param name="documento">Instance of Documento</param>
        /// <param name="usuario">Instance of UsuarioADE</param>
        /// <returns>Task<<ArquivoDownload>></returns>
        public async Task<ArquivoDownload> GerarDownloadDocumento(List<DadosAlunoKV> dadosAluno, Documento documento, UsuarioADE usuario, EnumFormatoDocumento formatoDocumento)
        {
            await AtualizarRequisitosDeUsuario(dadosAluno, usuario);
            MemoryStream file = await GerarDocumento(dadosAluno, documento,usuario);
            ArquivoDownload arquivoDownload = new ArquivoDownload(file, formatoDocumento);
            await LogGeracaoDocumento(documento, usuario);
            return arquivoDownload;
        }
        public async Task<ArquivoDownload> GerarDownloadQuestionario(List<DadosAlunoKV> dadosAluno, Documento documento, UsuarioADE usuario, RequisitosBasicosCabecalho requisitosBasicos, EnumFormatoDocumento formatoDocumento)
        {
            await AtualizarRequisitosDeUsuario(dadosAluno, usuario);
            MemoryStream file = await GerarQuestionario(dadosAluno, documento, usuario, requisitosBasicos);
            ArquivoDownload arquivoDownload = new ArquivoDownload(file, formatoDocumento);
            await LogGeracaoDocumento(documento, usuario);
            return arquivoDownload;
        }


        private async Task<MemoryStream> CopyFileToStream(FileStream file)
        {
            var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            ms.Position = 0;
            return ms;
        }

        private async Task AtualizandoRequisitosDeUsuario(List<DadosAlunoKV> dadosAluno, UsuarioADE usuario, Documento documento)
        {
            try
            {
                List<RequisitoDeDocumento> allBookmarks = await _servicoRequisitoDeDocumento.Filtrar(x => x.IdDocumento == documento.Identificador);
                List<string> BookmarksDB = await ObterRequisitosDocumento(allBookmarks);
                List<string> BookmarksAluno = ObterRequisitosAluno(dadosAluno);

                if (AreEquivalent(BookmarksDB,BookmarksAluno))
                    await AtualizarRequisitosDeUsuario(dadosAluno,usuario);
                else
                    await _loggerServices.CadastrarAsync(new UsuarioADE(),new SysLogs($"Os dados enviados pela página não coincidem com os do servidor || Ao realizar o download do Documento {documento.ToString()}", "DocumentoServices", EnumTipoLog.CriacaoArquivo));
                
            }
            catch (Exception ex)
            {
                await _loggerServices.CadastrarAsync(new UsuarioADE(), new SysLogs(ex.Message, ex.StackTrace, EnumTipoLog.CriacaoArquivo));
                throw ex;
            }
        }

        private async Task<MemoryStream> GerarDocumento(List<DadosAlunoKV> dadosAluno, Documento documento, UsuarioADE usuario)
        {
            int codigo = await RecuperarCodigoHistoricoGeracaoDocumento();
            List<RequisitoDeDocumento> Rdd = await _servicoRequisitoDeDocumento.ListarRegistros(documento.Identificador);

            DadosAlunoKV NomeCurso = new DadosAlunoKV();
            Curso curso = await _cursoServices.BuscarPorId(usuario.IdCurso);
            NomeCurso.Name = NomesRequisitos.CursoAluno_;
            NomeCurso.Value = curso.NomeCurso;
            dadosAluno.Add(NomeCurso);

            if (Rdd.Any(x=>x.IdRequisitoNavigation.Bookmark == NomesRequisitos.TabelaAreasEstagio_))
            {
                List<AreaEstagioCurso> areas = await _servicoAreaEstagioCurso.BuscarPorIdCurso(usuario.IdCurso);
                string area = dadosAluno.First(x => x.Name.Contains(NomesRequisitos.AreasEstagio_)).Value;
                int IdArea = Convert.ToInt32(area == "" ? "0" : area);
                return (MemoryStream)GeradorDocumento.GerarDocumentoPreenchimento(documento,dadosAluno, codigo, areas, IdArea);
            }
            return (MemoryStream)GeradorDocumento.GerarDocumentoPreenchimento(documento,dadosAluno, codigo);
        }
        private async Task<MemoryStream> GerarQuestionario(List<DadosAlunoKV> dadosAluno, Documento documento, UsuarioADE usuario, RequisitosBasicosCabecalho requisitosBasicos)
        {
            int codigo = await RecuperarCodigoHistoricoGeracaoDocumento();
            List<RequisitoDeDocumento> Rdd = await _servicoRequisitoDeDocumento.ListarRegistros(documento.Identificador);
            return (MemoryStream)GeradorDocumento.GerarDocumentoQuestionario(documento, dadosAluno, codigo, requisitosBasicos);
        }

        private bool AreEquivalent(List<string> List1, List<string> List2)
        {
            return (List1.Count() == List2.Count()) && !List1.Except(List2).Any();
        }

        private List<string> ObterRequisitosAluno(List<DadosAlunoKV> dadosAluno)
        {
            List<string> BookmarksAluno = new List<string>();
            foreach (DadosAlunoKV DE in dadosAluno)
            {
                BookmarksAluno.Add(DE.Name.Split(":").Last());
            }
            return BookmarksAluno;
        }

        public async Task<List<Requisito>> ObterRequisitosDeDocumento(int idDocumento, string IdAluno = "")
        {
            idDocumento = idDocumento == 0 ? 1 : idDocumento;
            List<RequisitoDeDocumento> requisitosDoDocumento = await unitOfWork.RepositorioBase<RequisitoDeDocumento>().Filtrar(x => x.IdDocumento == idDocumento);
            
            List<OpcaoRequisito> listaOpcaoDerequisitosDocumento = new List<OpcaoRequisito>();
            List<RequisitoDeUsuario> listarequisitosDeUsuario = new List<RequisitoDeUsuario>();
            List<Requisito> listarequisitos = new List<Requisito>();

            try
            {
                foreach(RequisitoDeDocumento reqdoc in requisitosDoDocumento)
                {
                    listaOpcaoDerequisitosDocumento.Add(await unitOfWork.RepositorioBase<OpcaoRequisito>().BuscarUm(req => req.IdRequisito == reqdoc.IdRequisito));

                    listarequisitosDeUsuario.Add(await unitOfWork.RepositorioBase<RequisitoDeUsuario>().BuscarUm(req=>req.IdRequisito == reqdoc.IdRequisito));

                    listarequisitos.Add(await unitOfWork.RepositorioBase<Requisito>().BuscarUm(req => req.Identificador == reqdoc.IdRequisito));
                }
            }
            catch(Exception ex)
            {

            }

            return listarequisitos;
        }

        private async Task<List<string>> ObterRequisitosDocumento(List<RequisitoDeDocumento> allBookmarks)
        {
            List<string> BookmarksDB = new List<string>();
            foreach (RequisitoDeDocumento rdd in allBookmarks)
            {
                Requisito req = await _servicoRequisito.BuscarPorId(rdd.IdRequisito);
                BookmarksDB.Add(req.Bookmark);
            }
            return BookmarksDB;
        }

        private async Task AtualizarRequisitosDeUsuario(List<DadosAlunoKV> dadosAluno,UsuarioADE usuario)
        {
            await _servicoRequisitoUsuario.AdicionarRequisitosDeUsuarioAsync(dadosAluno, usuario);
        }
        

        public async Task<int> CountByCurso(int idCurso)
        {
            List<Documento> Documentos = await unitOfWork.RepositorioBase<Documento>().Filtrar(x => x.IdCurso == idCurso);
            return Documentos.Count;
        }
        #endregion
    }
}
  
