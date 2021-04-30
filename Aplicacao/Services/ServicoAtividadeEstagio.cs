using System.Collections.Generic;
using ADE.Dominio.Models;
using ADE.Dominio.Interfaces;
using System;
using System.Threading.Tasks;
using ADE.Dominio.Models.Individuais;
using ADE.Dominio.Models.Enums;
using ADE.Infra.Data.UOW;
using System.Linq;
using System.Linq.Expressions;
using ADE.Dominio.Models.RelacaoEntidades;

namespace ADE.Aplicacao.Services
{
    public class ServicoAtividadeEstagio : ICountable
    {
        private UnitOfWork unitOfWork;
        private readonly ServicoConclusaoAtividadeCurso _conclusaoAtividadeCursoServices;

        public ServicoAtividadeEstagio(ref UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            _conclusaoAtividadeCursoServices = new ServicoConclusaoAtividadeCurso(ref unitOfWork);
        }

        public async Task VerificarTarefasEConcluir(UsuarioADE usuario, EnumEntidadesSistema enumEntidadesSistema, int idEntidade, EnumTipoAtividadeEstagio tipoAtividade, int QuantidadeVezes)
        {
            List<AtividadeEstagio> AtividadesRelacionadas = await BuscarPorEntidade(enumEntidadesSistema, idEntidade);
            foreach (AtividadeEstagio atividade in AtividadesRelacionadas)
            {
                if (atividade.TipoAtividade == tipoAtividade)
                {
                    if (atividade.CondicaoVezes == QuantidadeVezes)
                    {
                        ConclusaoAtividadeCurso conclusaoAtividade = new ConclusaoAtividadeCurso(usuario.Id, atividade.Identificador);
                        await _conclusaoAtividadeCursoServices.CadastrarAsync(usuario,conclusaoAtividade, atividade);
                    }
                }
            }
            await unitOfWork.Commit();
        }

        public async Task CadastrarAtividadeParaEntidade<T>(T entidade, int IdCurso = 1) where T : ModeloBase
        {
            if (entidade.GetType() == typeof(Documento))
            {
                try
                {
                    IdCurso = (int)entidade.GetType().GetProperty("IdCurso").GetValue(entidade);
                }
                catch
                {  }
                EnumTipoAtividadeEstagio tipoAtividadeEstagio = EnumTipoAtividadeEstagio.DownloadOuImpressao;
                int vezes = 1;
                AtividadeEstagio atividade = new AtividadeEstagio($"Preencher material {entidade.ToString()}", $"Realize o download ou impressão do material({entidade.ToString()}) {vezes} vez(es) para completar essa atividade", IdCurso, tipoAtividadeEstagio, vezes, EnumEntidadesSistema.Documento, entidade.Identificador);
                await CadastrarAsync(null, atividade);
            }

            else if (entidade.GetType() == typeof(Requisito))
            {
                if ((bool)entidade.GetType().GetProperty("Obrigatorio").GetValue(entidade) == true)
                {
                    EnumTipoAtividadeEstagio tipoAtividadeEstagio = EnumTipoAtividadeEstagio.AtualizarDados;
                    int vezes = 1;
                    AtividadeEstagio atividade = new AtividadeEstagio($"Preencher informação {entidade.ToString()}", $"Preencha a informação obrigatória ({entidade.ToString()}) para completar essa atividade", IdCurso, tipoAtividadeEstagio, vezes, EnumEntidadesSistema.Requisito, entidade.Identificador);
                    await CadastrarAsync(null, atividade);
                }
            }
        }
        public async Task RemoverAtividadeParaEntidade<T>(T entidade) where T : ModeloBase
        {
            try
            {
                if (entidade.GetType() == typeof(Documento))
                {
                    AtividadeEstagio atividade = await BuscarUm(x => x.IdentificadorEntidadeAtividade == entidade.Identificador && x.EnumEntidade == EnumEntidadesSistema.Documento);
                    await RemoverAsync(null, atividade);
                }

                else if (entidade.GetType() == typeof(Requisito))
                {
                    if ((bool)entidade.GetType().GetField("Obrigatorio").GetValue(null) == true)
                    {
                        AtividadeEstagio atividade = await BuscarUm(x => x.IdentificadorEntidadeAtividade == entidade.Identificador && x.EnumEntidade == EnumEntidadesSistema.Requisito);
                        await RemoverAsync(null, atividade);
                    }
                }
            }
            catch(Exception ex)
            {
            }
        }

        public async Task CadastrarAsync(UsuarioADE usuario, AtividadeEstagio entity)
        {
            await unitOfWork.RepositorioBase<AtividadeEstagio>().Criar(entity);
            await unitOfWork.Commit();
        }

        public async Task AtualizarAsync(UsuarioADE usuario, AtividadeEstagio entity)
        {
            unitOfWork.RepositorioBase<AtividadeEstagio>().Editar(entity);
            await unitOfWork.Commit();
        }

        public async Task<AtividadeEstagio> BuscarUm(Expression<Func<AtividadeEstagio, bool>> expression)
        {
            return await unitOfWork.RepositorioBase<AtividadeEstagio>().BuscarUm(expression);
        }

        public async Task<List<AtividadeEstagio>> BuscarPorEntidade(EnumEntidadesSistema entidadesSistema, int idEntidade)
        {
            return await unitOfWork.RepositorioBase<AtividadeEstagio>().Filtrar(x => x.EnumEntidade == entidadesSistema && x.IdentificadorEntidadeAtividade == idEntidade);
        }

        public async Task<List<AtividadeEstagio>> BuscarPorCursoDoUsuario(UsuarioADE usuario)
        {
            List<AtividadeEstagio> atividades = await unitOfWork.RepositorioBase<AtividadeEstagio>().Filtrar(x=>x.IdCurso == usuario.IdCurso || x.IdCurso == 0 );
            List<ConclusaoAtividadeCurso> conclusaoAtividades = await _conclusaoAtividadeCursoServices.Filtrar(x => x.IdUsuario == usuario.Id);
            foreach (AtividadeEstagio atividade in atividades)
            {
                if (conclusaoAtividades.Any(x => x.IdAtividade == atividade.Identificador))
                    atividade.ConclusoesAtividade.AddRange(conclusaoAtividades.Where(x => x.IdAtividade == atividade.Identificador));
            }
            return atividades;
        }

        public async Task<List<AtividadeEstagio>> BuscarPendentesPorCursoDoUsuario(UsuarioADE usuario)
        {
            List<AtividadeEstagio> atividades = await unitOfWork.RepositorioBase<AtividadeEstagio>().Filtrar(x => x.IdCurso == usuario.IdCurso || x.IdCurso == 0);
            List<ConclusaoAtividadeCurso> conclusaoAtividades = await _conclusaoAtividadeCursoServices.Filtrar(x => x.IdUsuario == usuario.Id);
            foreach (AtividadeEstagio atividade in atividades)
            {
                if (conclusaoAtividades.Any(x => x.IdAtividade == atividade.Identificador))
                    atividade.ConclusoesAtividade.AddRange(conclusaoAtividades.Where(x => x.IdAtividade == atividade.Identificador));
            }
            return atividades.Where(x => x.ConclusoesAtividade.Count == 0).ToList();
        }

        public async Task RemoverAsync(UsuarioADE usuario, AtividadeEstagio entity)
        {
            unitOfWork.RepositorioBase<AtividadeEstagio>().Remover(entity);
            await unitOfWork.Commit();
        }

        public async Task<List<AtividadeEstagio>> ListarAsync()
        {
            return await unitOfWork.RepositorioBase<AtividadeEstagio>().ListarAsync();
        }

        public async Task<List<AtividadeEstagio>> Filtrar(Expression<Func<AtividadeEstagio, bool>> expression)
        {
            return await unitOfWork.RepositorioBase<AtividadeEstagio>().Filtrar(expression);
        }

        public async Task<List<AtividadeEstagio>> FiltrarComQuantidade(Expression<Func<AtividadeEstagio, bool>> expression, int quantidade)
        {
            return await unitOfWork.RepositorioBase<AtividadeEstagio>().FiltrarComQuantidade(expression, quantidade);
        }

        public async Task<AtividadeEstagio> BuscarPorId(int id)
        {
            try
            {
                return await unitOfWork.RepositorioBase<AtividadeEstagio>().BuscarUm(x => x.Identificador == id);
            }
            catch (InvalidOperationException)
            {
                return new AtividadeEstagio() {Titulo="Atividade não encontrada"};
            }
        }

        public async Task<int> Count()
        {
            return await unitOfWork.RepositorioBase<AtividadeEstagio>().Count();
        }
    }
}
