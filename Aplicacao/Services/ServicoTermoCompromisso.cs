using System.Threading.Tasks;
using ADE.Dominio.Models.Individuais;
using ADE.Dominio.Models;
using ADE.Infra.Data.UOW;
using ADE.Dominio.Models.Enums;

namespace ADE.Aplicacao.Services
{
    public class ServicoTermoCompromisso : ServicoBase<TermoCompromisso>
    {
        public ServicoTermoCompromisso(ref UnitOfWork unitOfWork):base(ref unitOfWork)
        {
        }

        public override async Task LogAtualizacao(UsuarioADE usuario, TermoCompromisso entity, string Mensagem = null)
        {
            TermoCompromisso TermoAntigo = await BuscarPorId(entity.Identificador);
            string TituloAntigo = TermoAntigo.Titulo;
            await LogAlteracaoEntidade(usuario, TermoAntigo, entity, EnumEntidadesSistema.TermoCompromisso, EnumTipoLog.AlteracaoTermoCompromisso, Mensagem);
            await LogAtualizacaoComTituloAntigo(usuario, TermoAntigo, TituloAntigo);
        }
        private async Task LogAtualizacaoComTituloAntigo(UsuarioADE usuario, TermoCompromisso entidade, string TituloAntigo)
        {
            entidade.Titulo = TituloAntigo;
            await LogAcao(usuario, entidade, "TermoCompromisso", EnumTipoLog.AlteracaoTermoCompromisso, TipoEvento.Alteracao);
        }

        public override async Task LogCadastramento(UsuarioADE usuario, TermoCompromisso entidade)
        {
            await LogAcao(usuario, entidade, "TermoCompromisso", EnumTipoLog.CriacaoTermoCompromisso, TipoEvento.Criacao);
            TermoCompromisso confirmacao = await BuscarUm(x => x.Titulo == entidade.Titulo);
            confirmacao = confirmacao ?? new TermoCompromisso() { Titulo = "Erro" };
            await LogAlteracaoEntidade(usuario, confirmacao, entidade, EnumEntidadesSistema.TermoCompromisso, EnumTipoLog.CriacaoTermoCompromisso);
        }

        public override async Task LogRemocao(UsuarioADE usuario, TermoCompromisso entidade)
        {
            await LogAcao(usuario, entidade, "TermoCompromisso", EnumTipoLog.DelecaoTermoCompromisso, TipoEvento.Delecao);
            await LogDelecaoEntidade(usuario, entidade, EnumEntidadesSistema.TermoCompromisso, EnumTipoLog.DelecaoTermoCompromisso);
        }
    }
}
