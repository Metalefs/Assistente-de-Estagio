using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ADE.Aplicacao.Services;
using ADE.Apresentacao.Areas.Shared;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using Assistente_de_Estagio.Areas.Principal.Models;
using Assistente_de_Estagio.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ADE.Apresentacao.Areas.Principal.Controllers
{
    [Authorize]
    [Area("Principal")]
    public class RegistroHorasController : BaseController
    {
        private ApplicationDbContext context;
        private UnitOfWork unitOfWork;
        private ServicoRegistroDeHoras servicoRegistroDeHoras;
        private readonly ServicoCurso _cursoServices;
        private readonly ServicoInstituicao _servicoInstituicao;
        private readonly ServicoAtividadeEstagio _atividadeEstagioServices;
        private readonly ServicoRequisitoUsuario _servicoRequisitoUsuario;
        private readonly ServicoRequisito _servicoRequisito;
        readonly IHostingEnvironment env;

        public RegistroHorasController(
            UserManager<UsuarioADE> userManager,
            IHostingEnvironment _env,
            ApplicationDbContext _context
        ) : base(new UnitOfWork(_context), userManager)
        {
            context = _context;
            env = _env;
            unitOfWork = new UnitOfWork(context);
            _servicoInstituicao = new ServicoInstituicao(ref unitOfWork);
            _cursoServices = new ServicoCurso(ref unitOfWork);
            servicoRegistroDeHoras = new ServicoRegistroDeHoras(ref unitOfWork);
            _atividadeEstagioServices = new ServicoAtividadeEstagio(ref unitOfWork);
            _servicoRequisitoUsuario = new ServicoRequisitoUsuario(ref unitOfWork);
            _servicoRequisito = new ServicoRequisito(ref unitOfWork);
        }

        [ActionName("index")]
        public async Task<IActionResult> Index(bool Partial = false)
        {
            try
            {
                UsuarioADE usuario = await ObterUsuarioLogado();

                if(!usuario.AceitouTermos)
                    return PartialView("_TermosDeUso");

                if (Partial)
                    return PartialView("_RegistroHoras", await ParseIndexVM());

                return View(await ParseIndexVM());
            }
            catch (Exception ex)
            {
                await LogError($"Ocorreu um erro ao obter montar a página (RegistroHoras/Index): {ex.Message}", "RegistroHoras", EnumTipoLog.CriacaoRegistroHoras);
                return RedirectToAction("Account","Login");
            }
        }

        public async Task<IActionResult> Historico()
        {
            try
            {
                return View(await ObterRegistros());
            }
            catch (Exception ex)
            {
                await LogError($"Ocorreu um erro ao obter montar a página (RegistroHoras/Historico): {ex.Message}", "RegistroHoras", EnumTipoLog.VisualizacaoHistoricoAtividades);
                return RedirectToAction("Account", "Login");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Exportar()
        {
            UsuarioADE usuario = await ObterUsuarioLogado();
            try
            {
                Documento tabela = new Documento();
                tabela.IdCursoNavigation = await _cursoServices.BuscarPorId(usuario.IdCurso);
                tabela.IdCursoNavigation.Instituicao = await _servicoInstituicao.BuscarPorId(tabela.IdCursoNavigation.IdInstituicao);

                RequisitosBasicosCabecalho requisitosFicha = await ObterRequisitosBasicosUsuario(usuario);
                
                ArquivoDownload Arquivo = await servicoRegistroDeHoras.GerarTabelaHistorico(usuario, tabela, requisitosFicha);
                await _atividadeEstagioServices.VerificarTarefasEConcluir(usuario, EnumEntidadesSistema.Documento, tabela.Identificador, EnumTipoAtividadeEstagio.DownloadOuImpressao, 1);
                return File(Arquivo.Bytes, Arquivo.TipoArquivo, $"Tabela de Registro de Horas - {usuario.ToString()}.docx");
            }
            catch (Exception ex)
            { 
                System.Threading.Thread.Sleep(3000);
                await LogError($"{ex.Message}", "RegistroHoras", EnumTipoLog.ImpressaoArquivo);
            }
            ViewBag.Retorno = "É necessário ao menos um registro para realizar a exportação do histórico.";
            ModelState.AddModelError("Falha", ViewBag.Retorno);
            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> BuscarRegistrosPorData(string data)
        {
            UsuarioADE usuario = await ObterUsuarioLogado();
            try
            {
                DateTime dataBusca = DateTime.Parse(data, new CultureInfo("pt-BR"));
                ViewData["Data"] = data;
                return View("Historico",await servicoRegistroDeHoras.ObterRegistrosPorData(usuario, dataBusca));
            }
            catch (Exception ex)
            {
                await LogError($"Ocorreu um erro ao obter montar a página (RegistroHoras/Historico): {ex.Message}", "RegistroHoras", EnumTipoLog.VisualizacaoHistoricoAtividades);
            }
            return View(new List<RegistroDeHoras>());
        }
        
        public async Task<List<RegistroDeHoras>> BuscarRegistrosUsuario()
        {
            UsuarioADE usuario = await ObterUsuarioLogado();
            try
            {
                return await servicoRegistroDeHoras.ObterRegistrosUsuario(usuario);
            }
            catch (Exception ex)
            {
                await LogError($"Ocorreu um erro ao obter montar a página (RegistroHoras/Historico): {ex.Message}", "RegistroHoras", EnumTipoLog.VisualizacaoHistoricoAtividades);
            }
            return new List<RegistroDeHoras>();
        }

        private async Task<TabelaHorasViewModel> ParseIndexVM()
        {
            UsuarioADE usuario = await ObterUsuarioComDadosPessoais();
            TabelaHorasViewModel model = new TabelaHorasViewModel()
            {
                Usuario = usuario,
                Registros = await ObterRegistros(usuario)
            };
            return model;
        }

        public async Task<List<RegistroDeHoras>> ObterRegistros(UsuarioADE usuario = null)
        {
            usuario = usuario ?? await ObterUsuarioComDadosPessoais();
            return await servicoRegistroDeHoras.ObterRegistrosUsuario(usuario);
        }

        [HttpGet]
        public async Task<IActionResult> ObterRegistro(int idRegistro)
        {
            try
            {
                RegistroDeHoras model = await servicoRegistroDeHoras.BuscarPorId(idRegistro);
                return PartialView("_ModalEdicaoRegistro", model);
            }
            catch (Exception ex)
            {
                await LogError($"{ex.Message}", "RegistroHoras", EnumTipoLog.CriacaoRegistroHoras);
                return Json("{\"Erro\": \"Ocorreu um erro ao obter o registro\"}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObterModalDelecaoRegistro(int idRegistro)
        {
            try
            {
                RegistroDeHoras model = await servicoRegistroDeHoras.BuscarPorId(idRegistro);
                return PartialView("_ModalRemocaoRegistro", model);
            }
            catch (Exception ex)
            {
                await LogError($"{ex.Message}", "RegistroHoras", EnumTipoLog.DelecaoRegistroHoras);
                return Json("{\"Erro\": \"Ocorreu um erro ao obter o registro\"}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> IncluirRegistroHora(RegistroDeHoras registro)
        {
            try
            {
                if (registro.Validar())
                {
                    UsuarioADE usuario = await ObterUsuarioComDadosPessoais();
                    registro.IdUsuario = usuario.Id;
                    await servicoRegistroDeHoras.CadastrarAsync(usuario,registro);
                    ViewBag.Retorno = "Seu registro de atividade foi incluido com sucesso.";
                    ModelState.Clear();
                }
            }
            catch (Exception ex)
            {
                await LogError($"{ex.Message}", "RegistroHoras", EnumTipoLog.CriacaoRegistroHoras);
                ModelState.AddModelError("Falha", "Ocorreu um erro ao incluir o registro - " + ex.Message);
                ViewBag.Retorno = "Ocorreu um erro ao incluir o registro, contate o suporte para maior exclarecimento.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> IncluirRegistroHoraAsync(RegistroDeHoras registro)
        {
            try
            {
                if (registro.Validar())
                {
                    UsuarioADE usuario = await ObterUsuarioComDadosPessoais();
                    registro.IdUsuario = usuario.Id;
                    await servicoRegistroDeHoras.CadastrarAsync(usuario, registro);
                    return Json(new { Sucesso = "Seu registro de atividade foi incluido com sucesso." });
                }
                return Json(new { Erro = "Ocorreu um erro ao incluir o registro. O registro estava inválido." });
            }
            catch (Exception ex)
            {
                await LogError($"{ex.Message}", "RegistroHoras", EnumTipoLog.CriacaoRegistroHoras);
                ModelState.AddModelError("Falha", "Ocorreu um erro ao incluir o registro - " + ex.Message);
                ViewBag.Retorno = "Ocorreu um erro ao incluir o registro, contate o suporte para maior exclarecimento.";
                return Json(new { Erro = "Ocorreu um erro ao incluir o registro, contate o suporte para maior exclarecimento." });
            }
        }

        public async Task<IActionResult> EditarRegistroHora(RegistroDeHoras registro)
        {
            try
            {
                if (registro.Validar())
                {
                    UsuarioADE usuario = await ObterUsuarioComDadosPessoais();
                    registro.IdUsuario = usuario.Id;
                    registro.Usuario = usuario;
                    await servicoRegistroDeHoras.AtualizarAsync(usuario, registro);
                    ViewBag.Retorno = "Seu registro de atividade foi alterado com sucesso.";
                }
            }
            catch (Exception ex)
            {
                await LogError($"{ex.Message}", "RegistroHoras", EnumTipoLog.AlteracaoRegistroHoras);
                ModelState.AddModelError("Falha", "Ocorreu um erro ao alterar o registro. - " + ex.Message);
                ViewBag.Retorno = "Ocorreu um erro ao alterar o registro, contate o suporte para maior exclarecimento. " + ex.Message;
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoverRegistroHora(int Identificador)
        {
            UsuarioADE usuario = await ObterUsuarioComDadosPessoais();
            try
            {
                RegistroDeHoras registro = await servicoRegistroDeHoras.BuscarPorId(Identificador);
                await servicoRegistroDeHoras.RemoverAsync(usuario, registro);
                ViewBag.Retorno = "Seu registro de atividade foi removido com sucesso.";
            }
            catch (Exception ex)
            {
                await LogError($"{ex.Message}", "RegistroHoras", EnumTipoLog.DelecaoRequisito);
                ModelState.AddModelError("Falha", "Ocorreu um erro ao remover o registro - " + ex.Message);
                ViewBag.Retorno = "Ocorreu um erro ao remover o registro, contate o suporte para maior exclarecimento";
            }
            return RedirectToAction("Index");
        }
    }
}