using System.Collections.Generic;
using ADE.Dominio.Models;
using System;
using System.Threading.Tasks;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using System.Linq;
using System.Linq.Expressions;
using ADE.Utilidades.Extensions;
using ADE.Dominio.Models.Enums;

namespace ADE.Aplicacao.Services
{
    public class ServicoAtividadeUsuario: ServicoBase<AtividadeUsuario>
    {
        private UnitOfWork unitOfWork;
        private ServicoAlteracaoEntidadesSistema servicoAlteracaoEntidadesSistema;

        public ServicoAtividadeUsuario(ref UnitOfWork _unitOfWork):base(ref _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            servicoAlteracaoEntidadesSistema = new ServicoAlteracaoEntidadesSistema(ref unitOfWork);
        }

        public async Task CadastrarAtividadeUsuarioAsync(UsuarioADE usuario, AtividadeUsuario entity)
        {
            await unitOfWork.RepositorioBase<AtividadeUsuario>().Criar(entity);
            await CriarNotificacaoParaAtividade(usuario, entity);
            await unitOfWork.Commit();
        }

        public async Task AtualizarAtividadeUsuarioAsync(UsuarioADE usuario, AtividadeUsuario entity)
        {
            unitOfWork.RepositorioBase<AtividadeUsuario>().Editar(entity);
            NotificacaoIndividual notificacao = new NotificacaoIndividual(usuario.Id, usuario.Id, $"<i class='material-icons medium btn-outline-yellow prefix'>assignment</i> Sua atividade {entity}, foi alterada com sucesso!", $"Sua atividade {entity}, foi alterada com sucesso! na data {entity.DataHoraUltimaAlteracao.ToLocalTime()}.");

            await ServicoNotificacaoIndividual.CadastrarAsync(usuario, notificacao);
            await unitOfWork.Commit();
        }

        public async Task RemoverAtividadeUsuarioAsync(UsuarioADE usuario, AtividadeUsuario entity)
        {
            unitOfWork.RepositorioBase<AtividadeUsuario>().Remover(entity);
            NotificacaoIndividual notificacao = new NotificacaoIndividual(usuario.Id, usuario.Id, $"<i class='material-icons medium btn-outline-yellow prefix'>assignment_return</i> Sua atividade {entity}, foi deletada com sucesso!", $"Sua atividade {entity}, foi deletada com sucesso! na data {entity.DataHoraUltimaAlteracao.ToLocalTime()}.");

            await ServicoNotificacaoIndividual.CadastrarAsync(usuario, notificacao);
            await unitOfWork.Commit();
        }

        public async Task CriarNotificacaoParaAtividade(UsuarioADE usuario, AtividadeUsuario atividade)
        {
            EnumEstadoAtividade estado = atividade.VerificarEstado();
            string icon = string.Empty;
            string verbo = string.Empty;
            string adjetivo = string.Empty;
            switch (estado)
            {
                case EnumEstadoAtividade.Aberto:
                    if (atividade.Concluido)
                    {
                        icon = "assignment_turned_in";
                        verbo = "Concluiu";
                        adjetivo = "com sucesso";
                    }
                    else
                    {
                        icon = "assignment_turned_in";
                        verbo = "Cadastrou";
                        adjetivo = "com sucesso";
                    }
                    break;
                case EnumEstadoAtividade.Adiantado:
                    icon = "card_giftcard";
                    verbo = "Concluiu";
                    adjetivo = "com extrema eficiência!";
                    break;
                case EnumEstadoAtividade.Atrasado:
                    if (atividade.Concluido)
                        verbo = "Concluiu";
                    else
                        verbo = "Cadastrou";

                    icon = "assignment_late";
                    adjetivo = $"um pouco atrasado ({(DateTime.Today - atividade.Data).TotalDays} dias atrasado)";
                    break;
            }
            string tipoAtividade = atividade.TipoAtividade.GetDescription();
            string titulo = $"<i class='material-icons medium btn-outline-yellow'>{icon}</i> {verbo} a atividade.";
            string descricao = $" <hr>Parabéns pelo seu empenho, continue assim!<hr> {verbo} {atividade.Titulo} {adjetivo} em {DateTime.Now.ToLocalTime()}";

            NotificacaoIndividual notificacao = new NotificacaoIndividual(usuario.Id, usuario.Id, titulo, descricao);

            await ServicoNotificacaoIndividual.CadastrarAsync(usuario, notificacao);
            await unitOfWork.Commit();
        }

        public async Task<AtividadeUsuario> BuscarUm(Expression<Func<AtividadeUsuario, bool>> expression)
        {
            return await unitOfWork.RepositorioBase<AtividadeUsuario>().BuscarUm(expression);
        }

        public async Task<List<AtividadeUsuario>> BuscarPorIdUsuario(string idUsuario)
        {
            List<AtividadeUsuario> lista = await unitOfWork.RepositorioBase<AtividadeUsuario>().Filtrar(x=>x.IdUsuario == idUsuario);
            return lista.OrderByDescending(x => x.DataHoraInclusao).ToList();
        }

        public async Task<List<AtividadeUsuario>> Filtrar(Expression<Func<AtividadeUsuario, bool>> expression)
        {
            return await unitOfWork.RepositorioBase<AtividadeUsuario>().Filtrar(expression);
        }

        public async Task<List<AtividadeUsuario>> FiltrarComQuantidade(Expression<Func<AtividadeUsuario, bool>> expression, int quantidade)
        {
            return await unitOfWork.RepositorioBase<AtividadeUsuario>().FiltrarComQuantidade(expression, quantidade);
        }

        public override Task LogCadastramento(UsuarioADE usuario, AtividadeUsuario entidade)
        {
            throw new NotImplementedException();
        }

        public override Task LogAtualizacao(UsuarioADE usuario, AtividadeUsuario entity, string Mensagem = null)
        {
            throw new NotImplementedException();
        }

        public override Task LogRemocao(UsuarioADE usuario, AtividadeUsuario entity)
        {
            throw new NotImplementedException();
        }
    }
}
