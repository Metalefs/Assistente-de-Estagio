using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using ADE.Utilidades.Extensions;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ADE.Dominio.Models.RelacaoEntidades;

namespace ADE.Aplicacao.Services
{
    public class ServicoListaAmigos : ServicoBase<ListaAmigos>
    {
        private UnitOfWork unitOfWork;
        private ServicoUsuario ServicoUsuario;

        public ServicoListaAmigos(ref UnitOfWork _unitOfWork, UserManager<UsuarioADE> userManager) : base(ref _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            ServicoUsuario = new ServicoUsuario(unitOfWork, userManager);
        }

        public override async Task LogCadastramento(UsuarioADE usuario, ListaAmigos entidade)
        {
            UsuarioADE solicitado = await ServicoUsuario.BuscarPorId(entidade.IdAmigo);
            string Mensagem1 = $"Sua solicitação de amizade foi enviada para {solicitado.UserName}. <br> Você já pode encontrá-lo no chat!"; 
            string Mensagem2 = $"Você recebeu uma solicitação de amizade de {usuario.UserName}. <br> Ao aceitar, vocês poderão trocar mensagens no chat!"; 
            
            string Cabecalho1 = $"Solicitação de amizade enviada.";
            string Cabecalho2 = $"Solicitação de amizade recebida.";

            NotificacaoIndividual notificacao1 = new NotificacaoIndividual(entidade.IdUsuario, entidade.IdUsuario, Cabecalho1, Mensagem1);
            NotificacaoIndividual notificacao2 = new NotificacaoIndividual(entidade.IdUsuario, entidade.IdAmigo, Cabecalho2, Mensagem2);
            
            await CriarNotificacaoIndividual(usuario, notificacao1);
            await CriarNotificacaoIndividual(usuario, notificacao2);
        }

        public override async Task LogAtualizacao(UsuarioADE usuario, ListaAmigos Resposta, string Mensagem = null)
        {
            throw new NotImplementedException();
        }

        public override async Task LogRemocao(UsuarioADE usuario, ListaAmigos entidade)
        {
            UsuarioADE solicitado = await ServicoUsuario.BuscarPorId(entidade.IdAmigo);
            string Mensagem1 = $"Sua amizade com {solicitado.UserName} foi encerrada.";
            string Mensagem2 = $"{usuario.UserName} pode ter te removido da lista de amigos.";

            string Cabecalho1 = $"Encerrar solicitação de amizade.";
            string Cabecalho2 = $"Alteração em lista de amigos.";

            NotificacaoIndividual notificacao1 = new NotificacaoIndividual(entidade.IdUsuario, entidade.IdUsuario, Cabecalho1, Mensagem1);
            NotificacaoIndividual notificacao2 = new NotificacaoIndividual(entidade.IdUsuario, entidade.IdAmigo, Cabecalho2, Mensagem2);

            await CriarNotificacaoIndividual(usuario, notificacao1);
            await CriarNotificacaoIndividual(usuario, notificacao2);
        }

        public async new Task<List<ListaAmigos>> Filtrar(Expression<Func<ListaAmigos, bool>> expression) // Não ideal
        {
            List<ListaAmigos> ListaAmigos = await unitOfWork.RepositorioBase<ListaAmigos>().Filtrar(expression);
            return ListaAmigos;
        }

        public async Task<List<ListaAmigos>> BuscarPorIdUsuario(string IdUsuario)
        {
            List<ListaAmigos> lista = await unitOfWork.RepositorioBase<ListaAmigos>().Filtrar(x => x.IdUsuario == IdUsuario);
            return lista;
        }

        public async Task<List<UsuarioADE>> BuscarAmigosUsuario(string IdUsuario)
        {
            List<ListaAmigos> lista = await unitOfWork.RepositorioBase<ListaAmigos>().Filtrar(x => x.IdUsuario == IdUsuario);
            List<UsuarioADE> amigos = new List<UsuarioADE>();
            foreach (ListaAmigos LA in lista)
            {
                UsuarioADE amigo = await ServicoUsuario.BuscarPorId(LA.IdAmigo);
                amigos.Add(amigo);
            }
            return amigos;
        }

    }
}
