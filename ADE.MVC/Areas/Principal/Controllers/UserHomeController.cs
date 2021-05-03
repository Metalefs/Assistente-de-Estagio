using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADE.Aplicacao.Services;
using ADE.Apresentacao.Areas.Shared;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using Assistente_de_Estagio.Areas.Administracao.Models.Charts;
using Assistente_de_Estagio.Areas.Administracao.Models.Charts.Interface;
using Assistente_de_Estagio.Areas.Principal.Models;
using Assistente_de_Estagio.Areas.Principal.Models.Charts;
using Assistente_de_Estagio.Areas.Shared;
using Assistente_de_Estagio.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ADE.Apresentacao.Areas.Principal.Controllers
{
    [Authorize]
    [Area("Principal")]
    public class UserHomeController : BaseController
    {
        private ApplicationDbContext context;
        private UnitOfWork unitOfWork;
        private ServicoDocumento _servicoDocumento;
        private ServicoHistoricoGeracaoDocumento _servicoHistoricoGeracaoDocumento;
        private ServicoRequisitoUsuario _servicoRequisitoUsuario;
        private ServicoRequisito _servicoRequisito;
        private ServicoCurso _servicoCurso;
        private ServicoInstituicao _servicoInstituicao;
        private ServicoLogAcoesEspeciais _servicoLogAcoesEspeciais;
        private ServicoRegistroDeHoras _servicoRegistroDeHoras;
        readonly IHostingEnvironment env;

        public UserHomeController(
            UserManager<UsuarioADE> userManager,
            IHostingEnvironment _env,
            ApplicationDbContext _context
        ): base(new UnitOfWork(_context), userManager)
        {
            context = _context;
            env = _env;
            unitOfWork = new UnitOfWork(context);
            _servicoDocumento = new ServicoDocumento(ref unitOfWork, env);
            _servicoHistoricoGeracaoDocumento = new ServicoHistoricoGeracaoDocumento(ref unitOfWork);
            _servicoRequisitoUsuario = new ServicoRequisitoUsuario(ref unitOfWork);
            _servicoCurso = new ServicoCurso(ref unitOfWork);
            _servicoInstituicao = new ServicoInstituicao(ref unitOfWork);
            _servicoLogAcoesEspeciais = new ServicoLogAcoesEspeciais(ref unitOfWork);
            _servicoRegistroDeHoras = new ServicoRegistroDeHoras(ref unitOfWork);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index(bool Partial = false)
        {
            UsuarioADE usuario = await ObterUsuarioComDadosPessoais();
            UsuarioHomeViewModel model;
            try
            {
                if (usuario.Id != "N/A")
                {
                    usuario.IdInstituicaoNavigation = await _servicoInstituicao.BuscarPorId(usuario.IdInstituicao);
                    model = new UsuarioHomeViewModel()
                    {
                        Usuario = usuario,
                        GeracoesDocumento = await _servicoHistoricoGeracaoDocumento.RecuperarHistoricoDoUsuario(usuario.Id),
                        QuantidadeDocumentosGerados = await ObterProgressoDownloads(usuario),
                        ContagemRequisitoDoUsuario = await ObterRequisitoUsuarioComRequisito(usuario.Id),
                        ContagemDocumentosCurso = await _servicoDocumento.ContarPorCurso(usuario.IdCurso)
                    };
                    if (!usuario.PossuiCurso())
                        model.PrimeiroCurso = true;
                }
                else
                {
                    model = new UsuarioHomeViewModel()
                    {
                        Usuario = await ObterUsuarioLogado(),
                        GeracoesDocumento = null,
                        QuantidadeDocumentosGerados = 0,
                        ContagemRequisitoDoUsuario = 0,
                        ContagemDocumentosCurso = 0
                    };
                }
            }
            catch (System.Exception ex)
            {
                model = new UsuarioHomeViewModel()
                {
                    Usuario = await ObterUsuarioLogado(),
                    GeracoesDocumento = null,
                    QuantidadeDocumentosGerados = 0,
                    ContagemRequisitoDoUsuario = 0,
                    ContagemDocumentosCurso = 0
                };
            }
            if (Partial)
                return PartialView("_Index", model);

            return View(model);
        }

        public async Task<PaginatedList<InformacaoCursoVM>> ParseInformacaoCursoVM(UsuarioADE usuario, int? idInstituicao, int? pageNumber, UnitOfWork unitOfWork)
        {
            try
            {
                unitOfWork = unitOfWork ?? new UnitOfWork(context);
                _servicoCurso = new ServicoCurso(ref unitOfWork);
                _servicoInstituicao = new ServicoInstituicao(ref unitOfWork);
                _servicoDocumento = new ServicoDocumento(ref unitOfWork);

                List<Curso> ListaCursos = await _servicoCurso.Filtrar(x => x.IdInstituicao == idInstituicao);
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

                PaginatedList<InformacaoCursoVM> lista = PaginatedList<InformacaoCursoVM>.Create(model.AsQueryable(), pageNumber ?? 1, 10);
                return lista;
            }
            catch(System.Exception ex)
            {
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                return null;
            }
        }

        private async Task<int> ObterRequisitoUsuarioComRequisito(string idUsuario)
        {
            _servicoRequisitoUsuario = new ServicoRequisitoUsuario(ref unitOfWork);
            _servicoRequisito = new ServicoRequisito(ref unitOfWork);
            List<RequisitoDeUsuario> ListaRequisitos = await _servicoRequisitoUsuario.BuscarRequisitosDoUsuario(idUsuario);
            return ListaRequisitos.Count;
        }

        public async Task<string> AtividadesUsuarioJson()
        {
            try
            {
                UsuarioADE usuario = await ObterUsuarioLogado();
                List<LogAcoesEspeciais> Atividades = await _servicoLogAcoesEspeciais.Filtrar(x=>x.IdUsuario == usuario.Id);
                IChart Chart = new AtividadesPorDiaChart(Atividades);
                return Newtonsoft.Json.JsonConvert.SerializeObject(Chart);
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("Falha", "Erro ao obter dados do gráfico tráfego do site");
                ViewBag.Retorno = "Erro ao obter dados do gráfico tráfego do site";
                await LogError(ex.Message, ex.Source, ADE.Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                return "{Values: ['1'], Labels: ['Erro']}";
            }
        }

        public async Task<string> PorcentagemConclusao(string idUsuario = null)
        {
            UsuarioADE usuario = idUsuario == null ? await ObterUsuarioLogado() : await ObterUsuarioPorEmailOuId(idUsuario);

            int DocumentosCursoUsuario = await _servicoDocumento.ContarPorCurso(usuario.IdCurso);
            int DownloadsDistintos = await ObterProgressoDownloads(usuario);

            float CargaHorariaCurso = await _servicoRegistroDeHoras.ObterTotalCargaHorariaCursoUsuario(usuario);
            float ContabilizacaoHoras = await _servicoRegistroDeHoras.ObterContabilizacaoHorasUsuario(usuario);

            float ProgressoUsuario = DownloadsDistintos + ContabilizacaoHoras/60;
            float Total = DocumentosCursoUsuario + CargaHorariaCurso;

            PorcentagemConclusaoChart PCC = new PorcentagemConclusaoChart(ProgressoUsuario, Total);

            return Newtonsoft.Json.JsonConvert.SerializeObject(PCC);
        }

        private async Task<int> ObterProgressoDownloads(UsuarioADE usuario)
        {
            List<Documento> DocumentosCursoUsuario = await _servicoDocumento.ListarPorCurso(usuario.IdCurso);
            List<HistoricoGeracaoDocumento> DownloadsDoUsuario = await _servicoHistoricoGeracaoDocumento.RecuperarHistoricoDoUsuario(usuario.Id);
            DownloadsDoUsuario = DownloadsDoUsuario.Distinct().ToList();
            ComparacaoDownloadDocumento comparacao = new ComparacaoDownloadDocumento(DocumentosCursoUsuario, DownloadsDoUsuario);
            return comparacao.ProgressoUsuario;
        }

        private async Task<int> ObterDownloadsDistintos(UsuarioADE usuario)
        {
            List<HistoricoGeracaoDocumento> DownloadsDoUsuario = await _servicoHistoricoGeracaoDocumento.RecuperarHistoricoDoUsuario(usuario.Id);
            return DownloadsDoUsuario.Distinct().Count();
        }

    }
}