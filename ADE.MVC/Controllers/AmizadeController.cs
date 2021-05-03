using System.Threading.Tasks;
using ADE.Aplicacao.Services;
using ADE.Apresentacao.Areas.Shared;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using Assistente_de_Estagio.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ADE.Dominio.Models.RelacaoEntidades;
using ADE.Dominio.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assistente_de_Estagio.Areas.Shared
{
    [Authorize]
    public class AmizadeController : BaseController
    {
        readonly ApplicationDbContext context;
        static UnitOfWork unitOfWork;
        private ServicoListaAmigos _servicoListaAmigos;
        private ServicoMensagemIndividual _servicoMensagemIndividual;
       
        public AmizadeController(UserManager<UsuarioADE> userManager,
            ApplicationDbContext context
            ) : base(new UnitOfWork(context), userManager)
        {
            unitOfWork = new UnitOfWork(context);
            _servicoListaAmigos = new ServicoListaAmigos(ref unitOfWork, userManager);
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarRelacionamentoJson(string idUsuario)
        {
            try
            {
                UsuarioADE usuario = await ObterUsuarioLogado();
                ListaAmigos amigos = new ListaAmigos(usuario.Id, idUsuario, EnumTipoRelacionamento.Amigo);
                await _servicoListaAmigos.CadastrarAsync(amigos);
                return Json(new { Sucesso = "Contato cadastrado" });
            }
            catch(Exception ex)
            {
                await LogError("Erro ao cadastrar contato", "Cadastrar relacionamento", EnumTipoLog.ListaPerfis);
                return Json(new { Erro = "Erro ao cadastrar contato"});
            }
        }
        [HttpPost]
        public async Task<IActionResult> CadastrarRelacionamento(string idUsuario, string view = "Index")
        {
            UsuarioADE usuario = await ObterUsuarioLogado();
            ListaAmigos amigos = new ListaAmigos(usuario.Id, idUsuario, EnumTipoRelacionamento.Amigo);
            await _servicoListaAmigos.CadastrarAsync(amigos);
            return RedirectToAction(view, "Perfis", new { area = "Principal" });
        }
        [HttpPost]
        public async Task<IActionResult> AlterarRelacionamentoJson(ListaAmigos amigos)
        {
            UsuarioADE usuario = await ObterUsuarioLogado();
            try
            {
                await _servicoListaAmigos.AtualizarAsync(usuario, amigos);
                return Json(new { Sucesso = "Relacionamento alterado" });
            }
            catch(Exception ex)
            {
                await LogError("Erro ao alterar relacionamento", "Alterar relacionamento", EnumTipoLog.ListaPerfis);
                return Json(new { Erro = "Erro ao alterar relacionamento"});
            }
        }
        [HttpPost]
        public async Task<IActionResult> AlterarRelacionamento(ListaAmigos amigos)
        {
            UsuarioADE usuario = await ObterUsuarioLogado();
            await _servicoListaAmigos.AtualizarAsync(usuario,amigos);
            return RedirectToAction("Index","Perfis", new {area = "Principal"});
        }
        [HttpPost]
        public async Task<IActionResult> AdicionarOuRemoverRelacionamentoJson(string idUsuario)
        {
            UsuarioADE usuario = await ObterUsuarioLogado();
            UsuarioADE amigo = await ObterUsuarioPorEmailOuId(idUsuario);
            ListaAmigos amizade = new ListaAmigos();
            try
            {
                try
                {
                    amizade = await _servicoListaAmigos.FiltrarUltimo(x=>x.IdUsuario == usuario.Id && x.IdAmigo == idUsuario);
                    bool Amigo  = amizade != null;
                    if (Amigo)
                    {
                        await _servicoListaAmigos.RemoverAsync(usuario, amizade);
                        return Json(new { Sucesso = "Relacionamento removido com sucesso" , Tipo = "remocao"});
                    }
                    else
                    {
                        ListaAmigos amigos = new ListaAmigos(usuario.Id, idUsuario, EnumTipoRelacionamento.Amigo);
                        await _servicoListaAmigos.CadastrarAsync(amigos);
                        return Json(new { Sucesso = "Contato cadastrado", Tipo = "adicao" });
                    }
                }
                catch(Exception)
                {
                    ListaAmigos amigos = new ListaAmigos(usuario.Id, idUsuario, EnumTipoRelacionamento.Amigo);
                    await _servicoListaAmigos.CadastrarAsync(amigos);
                    return Json(new { Sucesso = "Contato cadastrado" , Tipo = "adicao"});
                }
            }
            catch (Exception ex)
            {
                await LogError("Erro ao cadastrar contato", "Cadastrar relacionamento", EnumTipoLog.ListaPerfis);
                return Json(new { Erro = "Erro ao cadastrar contato" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> RemoverRelacionamento(string idUsuario, string view = "Index")
        {
            UsuarioADE usuario = await ObterUsuarioLogado();
            UsuarioADE amigo = await ObterUsuarioPorEmailOuId(idUsuario);
            ListaAmigos amizade = new ListaAmigos();
            try
            {
                amizade = await _servicoListaAmigos.BuscarUm(x=>x.IdUsuario == usuario.Id && x.IdAmigo == amigo.Id);
                await _servicoListaAmigos.RemoverAsync(usuario, amizade);
            }
            catch (Exception ex)
            {
                await LogError("Erro ao Remover contato", "Remover relacionamento", EnumTipoLog.ListaPerfis);
                return Json(new { Erro = "Erro ao Remover contato" });
            }
            return RedirectToAction(view, "Perfis", new {area = "Principal"});
        }
    }
}