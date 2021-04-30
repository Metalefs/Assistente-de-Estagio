using ADE.Aplicacao.Services;
using ADE.Dominio.Models.Enums;
using ADE.Utilidades.Handlers;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using System.Threading.Tasks;
using ADE.Utilidades.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ADE.Dominio.Models
{
    public abstract partial class ServicoBase<T> where T : ModeloBase
    {
        private UnitOfWork unitOfWork;
        public static ServicoAlteracaoEntidadesSistema _servicoAlteracaoEntidades;
        public static ServicoAtividadeEstagio _servicoAtividadeEstagio;
        public static ServicoLogAcoesEspeciais _servicoLogAcoesEspeciais;
        public static ServicoSysLogs ServicoLogErros;
        public static ServicoNotificacaoIndividual ServicoNotificacaoIndividual;
        public ServicoBase(ref UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            _servicoAlteracaoEntidades = new ServicoAlteracaoEntidadesSistema(ref unitOfWork);
            _servicoAtividadeEstagio = new ServicoAtividadeEstagio(ref unitOfWork);
            _servicoLogAcoesEspeciais = new ServicoLogAcoesEspeciais(ref unitOfWork);
            ServicoNotificacaoIndividual = new ServicoNotificacaoIndividual(ref unitOfWork);
            ServicoLogErros = new ServicoSysLogs(ref unitOfWork);
        }

        public async Task CadastrarAsync(T entidade)
        {
            await unitOfWork.RepositorioBase<T>().Criar(entidade);
            await unitOfWork.Commit();
            await _servicoAtividadeEstagio.CadastrarAtividadeParaEntidade(entidade);
        }

        public async Task CadastrarAsync(List<T> entidade)
        {
            foreach(T elemento in entidade)
            {
                await unitOfWork.RepositorioBase<T>().Criar(elemento);
                await _servicoAtividadeEstagio.CadastrarAtividadeParaEntidade(elemento);
            }
            await unitOfWork.Commit();
        }

        public async Task CadastrarAsync(UsuarioADE usuario, T entidade)
        {
            await unitOfWork.RepositorioBase<T>().Criar(entidade);
            await unitOfWork.Commit();
            await _servicoAtividadeEstagio.CadastrarAtividadeParaEntidade(entidade);
            try
            {
                await LogCadastramento(usuario, entidade);
            }
            catch (NotImplementedException) { }
        }

        public abstract Task LogCadastramento(UsuarioADE usuario, T entidade);

        public async Task AtualizarAsync(UsuarioADE usuario, T entity, string Mensagem = null)
        {
            try
            {
                await LogAtualizacao(usuario,entity,Mensagem);
            }
            catch (NotImplementedException) { }
            unitOfWork.RepositorioBase<T>().Editar(entity);
            await unitOfWork.Commit();
        }

        public abstract Task LogAtualizacao(UsuarioADE usuario, T entity, string Mensagem = null);

        public async Task RemoverAsync(UsuarioADE usuario, T entity)
        {
            try
            {
                await LogRemocao(usuario, entity);
            }
            catch (NotImplementedException) { }
            T verificacao = await unitOfWork.RepositorioBase<T>().BuscarPorId(entity.Identificador);
            await _servicoAtividadeEstagio.RemoverAtividadeParaEntidade(entity);
            unitOfWork.RepositorioBase<T>().Remover(verificacao);
            await unitOfWork.Commit();
        }
        
        public abstract Task LogRemocao(UsuarioADE usuario, T entity);

        public async Task<T> BuscarPorId(int id)
        {
            return await unitOfWork.RepositorioBase<T>().BuscarUm(x => x.Identificador == id);
        }

        public async Task<T> BuscarUm(Expression<Func<T, bool>> expression)
        {
            return await unitOfWork.RepositorioBase<T>().BuscarUm(expression);
        }

        public async Task<List<Documento>> BuscarDocumentosPorTipo(EnumTipoDocumento tipo)
        {
            return await unitOfWork.RepositorioBase<Documento>().Filtrar(x=>x.Tipo == tipo);
        }
        public async Task<int> RecuperarCodigoHistoricoGeracaoDocumento()
        {
            try
            {
                float count = await unitOfWork.RepositorioBase<HistoricoGeracaoDocumento>().Count();
                return Convert.ToInt32(count + 1);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task LogGeracaoDocumento(Documento documento, UsuarioADE usuario)
        {
            HistoricoGeracaoDocumento saveDoc = new HistoricoGeracaoDocumento($"Documento {documento.ToString()} criado pelo Usuario {usuario.ToString()}", documento.Identificador, usuario.Id)
            {
                Identificador = await RecuperarCodigoHistoricoGeracaoDocumento()
            };
            await LogAcao(usuario, saveDoc, "LogGeracaoDocumento", EnumTipoLog.CriacaoArquivo, TipoEvento.Criacao);
            await unitOfWork.RepositorioBase<HistoricoGeracaoDocumento>().Criar(saveDoc);
        }

        public async Task<List<T>> Filtrar(Expression<Func<T, bool>> expression)
        {
            return await unitOfWork.RepositorioBase<T>().Filtrar(expression);
        }
        
        public async Task<List<T>> FiltrarComQuantidade(Expression<Func<T, bool>> expression, int quantidade)
        {
            return await unitOfWork.RepositorioBase<T>().FiltrarComQuantidade(expression, quantidade);
        }

        public async Task<T> FiltrarUltimo(Expression<Func<T, bool>> expression)
        {
            return await unitOfWork.RepositorioBase<T>().FiltrarUltimo(expression);
        }

        public async Task<List<T>> ListarAsync()
        {
            return await unitOfWork.RepositorioBase<T>().ListarAsync();
        }

        public async Task<int> Count()
        {
            return await unitOfWork.RepositorioBase<T>().Count();
        }
        public async Task<int> CountByFilter(Expression<Func<T, bool>> expression)
        {
            return await unitOfWork.RepositorioBase<T>().CountByFilter(expression);
        }

        public async Task CriarNotificacaoIndividual(UsuarioADE usuario, NotificacaoIndividual entity)
        {
            entity.DataHoraUltimaAlteracao = DateTime.Now;
            await ServicoNotificacaoIndividual.CadastrarAsync(usuario,entity);
        }

        public async Task LogAcao(UsuarioADE usuario, T entity, string LocalAcao, EnumTipoLog tipoLog, TipoEvento Acao)
        {
            string Mensagem = LoggingHandler<T>.GerarMensagemTipoLog(usuario, entity, Acao);
            LogAcoesEspeciais Log = new LogAcoesEspeciais(Mensagem, LocalAcao, tipoLog, usuario.Id);
            await _servicoLogAcoesEspeciais.CadastrarAsync(usuario,Log);
        }
        public async Task LogAcao(UsuarioADE usuario, HistoricoGeracaoDocumento entity, string LocalAcao, EnumTipoLog tipoLog, TipoEvento Acao)
        {
            string Mensagem = LoggingHandler<HistoricoGeracaoDocumento>.GerarMensagemTipoLog(usuario, entity, Acao);
            LogAcoesEspeciais Log = new LogAcoesEspeciais(Mensagem, LocalAcao, tipoLog, usuario.Id);
        }

        public async Task LogAlteracaoEntidade(UsuarioADE Autor, T entidadeAntiga, T entidadeAtualizada, EnumEntidadesSistema Entidade, EnumTipoLog tipoLog, string Mensagem = null)
        {
            Mensagem = Mensagem ?? string.Empty;
            entidadeAtualizada.DataHoraUltimaAlteracao = DateTime.Now; 
            string MensagemAlteracao = entidadeAtualizada.GerarMensagemAlteracao(entidadeAntiga, Mensagem);
            AlteracaoEntidadesSistema AlteracaoEntidade = new AlteracaoEntidadesSistema()
            {
                IdentificadorEntidade = entidadeAtualizada.Identificador,
                MensagemAlteracao = MensagemAlteracao,
                Autor = Autor,
                IdAutor = Autor.Id,
                Entidade = Entidade
            };
            await _servicoAlteracaoEntidades.CadastrarAsync(AlteracaoEntidade, tipoLog);
        }

        public async Task LogDelecaoEntidade(UsuarioADE Autor, T entidade, EnumEntidadesSistema Entidade, EnumTipoLog tipoLog)
        {
            string MensagemAlteracao = $"{Entidade.ObterNomeEnum()} {entidade.ToString()} foi removido do sistema";
            AlteracaoEntidadesSistema AlteracaoEntidade = new AlteracaoEntidadesSistema()
            {
                IdentificadorEntidade = entidade.Identificador,
                MensagemAlteracao = MensagemAlteracao,
                Autor = Autor,
                IdAutor = Autor.Id,
                Entidade = Entidade
            };
            await _servicoAlteracaoEntidades.CadastrarAsync(AlteracaoEntidade, tipoLog);
        }

        public async Task LogAcao(UsuarioADE usuario, string Mensagem, string LocalAcao, EnumTipoLog tipoLog, TipoEvento Acao)
        {
            LogAcoesEspeciais Log = new LogAcoesEspeciais(Mensagem, LocalAcao, tipoLog, usuario.Id);
            await _servicoLogAcoesEspeciais.CadastrarAsync(usuario, Log);
        }

        public async Task LogError(UsuarioADE usuario, string mensagem, string localOrigem, EnumTipoLog acaoSistema)
        {
            SysLogs sysLogs = new SysLogs(mensagem, localOrigem, acaoSistema);
            await ServicoLogErros.CadastrarAsync(usuario, sysLogs);
        }
    }
}
