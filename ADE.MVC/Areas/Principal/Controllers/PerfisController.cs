using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADE.Aplicacao.Services;
using ADE.Apresentacao.Areas.Shared;
using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.Individuais;
using ADE.Dominio.Models.RelacaoEntidades;
using ADE.Infra.Data.UOW;
using Assistente_de_Estagio.Areas.Principal.Models;
using Assistente_de_Estagio.Areas.Shared;
using Assistente_de_Estagio.Data;
using Assistente_de_Estagio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ADE.Apresentacao.Areas.Principal.Controllers
{
    [Authorize]
    [Area("Principal")]
    public class PerfisController : BaseController
    {
        private ApplicationDbContext context;
        private UnitOfWork unitOfWork;
        private readonly ServicoHistoricoGeracaoDocumento _servicoHistoricoGeracaoDocumento;
        private readonly ServicoListaAmigos _servicoListaAmigos;
        private readonly ServicoCurso _servicoCurso;
        private readonly ServicoUsuario _servicoUsuario;

        public PerfisController(
            UserManager<UsuarioADE> userManager,
            ApplicationDbContext _context
        ) : base(new UnitOfWork(_context), userManager)
        {
            context = _context;
            unitOfWork = new UnitOfWork(context);
            _servicoHistoricoGeracaoDocumento = new ServicoHistoricoGeracaoDocumento(ref unitOfWork);
            _servicoListaAmigos = new ServicoListaAmigos(ref unitOfWork, userManager);
            _servicoCurso = new ServicoCurso(ref unitOfWork);
            _servicoUsuario = new ServicoUsuario(unitOfWork, userManager);
        }

        public async Task<IActionResult> Index(int? page = 1, FiltroPerfil filter = FiltroPerfil.Curso, string email = "")
        {
            PerfisViewModel model;
            try
            {
                int size = 50;
                int start = page.Value * size - size;
                int finish = page.Value * size;
                ViewData["filter"] = filter;
                model = await ParsePerfis(page, filter, email, start, finish);
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source, EnumTipoLog.ListaPerfis);
                return RedirectToAction("Index", "Account");
            }
            return View(model);
        }

        private async Task<PerfisViewModel> ParsePerfis(int? page, FiltroPerfil filter, string email, int start, int finish)
        {
            UsuarioADE usuario = await ObterUsuarioLogado();
            PerfisViewModel model;
            if (!string.IsNullOrWhiteSpace(email))
            {
                model = new PerfisViewModel()
                {
                    Usuarios = await ObterUsuarios(page, email, usuario),
                    Amigos = await _servicoListaAmigos.BuscarPorIdUsuario(usuario.Id),
                    Paginas = Math.Abs(await _servicoUsuario.Count() / 50),
                    PaginaAtual = page.Value,
                };
            }
            else
            {
                model = new PerfisViewModel()
                {
                    Usuarios = await ObterUsuarios(page, start, finish, filter, usuario),
                    Amigos = await _servicoListaAmigos.BuscarPorIdUsuario(usuario.Id),
                    Paginas = Math.Abs(await _servicoUsuario.Count() / 50),
                    PaginaAtual = page.Value,
                };
            }
            return model;
        }

        private async Task<PaginatedList<UsuarioADE>> ObterUsuarios(int? pagenumber, int start, int finish, FiltroPerfil filter, UsuarioADE usuario)
        {
            List<UsuarioADE> lista;
            switch (filter)
            {
                case FiltroPerfil.Instituicao:
                    lista = await _servicoUsuario.Filtrar(x=> x.IdInstituicao == usuario.IdInstituicao);
                    break;
                case FiltroPerfil.Curso: 
                    lista = await _servicoUsuario.Filtrar(x=> x.IdCurso== usuario.IdCurso);
                    break;
                case FiltroPerfil.Pontuacao:
                    lista = await _servicoUsuario.TakeBetween(start, finish);
                    break;
                default:
                    lista = await _servicoUsuario.Filtrar(x => x.IdCurso == usuario.IdCurso);
                    break;
            };

            return PaginatedList<UsuarioADE>.Create(lista.AsQueryable(), pagenumber ?? 1, 100);
        }

        private async Task<PaginatedList<UsuarioADE>> ObterUsuarios(int? pagenumber, string email, UsuarioADE usuario)
        {
            List<UsuarioADE> lista;
            lista = await _servicoUsuario.Filtrar(x => x.IdCurso == usuario.IdCurso && x.UserName.Contains(email) ||  x.IdCurso == usuario.IdCurso && x.Email.Contains(email));

            return PaginatedList<UsuarioADE>.Create(lista.AsQueryable(), pagenumber ?? 1, 100);
        }

        public async Task<IActionResult> PerfilUsuario(string id)
        {
            PerfilUsuarioViewModel model;
            try
            {
                model = await ParsePerfilUsuario(id);
            }
            catch(Exception ex)
            {
                await LogError(ex.Message, ex.Source, EnumTipoLog.PerfilUsuario);
                return RedirectToAction("Index","Account");
            }
            return View(model);
        }

        private async Task<PerfilUsuarioViewModel> ParsePerfilUsuario(string EmailOrId = null)
        {
            UsuarioADE usuario = EmailOrId == null ? await ObterUsuarioLogado() : await ObterUsuarioPorEmailOuId(EmailOrId);
            PerfilUsuarioViewModel model = new PerfilUsuarioViewModel()
            {
                ListaAmigos = await _servicoListaAmigos.BuscarPorIdUsuario(usuario.Id),
                Usuario = usuario,
                CursoUsuario = await _servicoCurso.BuscarPorId(usuario.IdCurso),
                QuantidadeDocumentosGerados = await _servicoHistoricoGeracaoDocumento.CountByFilter(x => x.IdUsuario == EmailOrId || x.IdUsuarioNavigation.Email == EmailOrId),
                AutorizacaoUsuario = await ObterAutorizacaoUsuario(usuario)
            };
            return model;
        }

    }
}