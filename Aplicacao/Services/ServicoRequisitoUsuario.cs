using ADE.Dominio.Interfaces;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ADE.Aplicacao.Services
{
    public class ServicoRequisitoUsuario : ServicoBase<RequisitoDeUsuario>, IServicoBase<RequisitoDeUsuario>
    {
        private UnitOfWork unitOfWork;
        private ServicoAtividadeEstagio ServicoAtividadeEstagio;

        public ServicoRequisitoUsuario(ref UnitOfWork _unitOfWork) : base (ref _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            ServicoAtividadeEstagio = new ServicoAtividadeEstagio(ref unitOfWork);
        }

        public async override Task LogCadastramento (UsuarioADE usuario, RequisitoDeUsuario requisitoDeUsuario)
        {
            await LogAcao(usuario, requisitoDeUsuario, "ServicoRequisitoUsuario", EnumTipoLog.CriacaoRequisitoDeUsuario, TipoEvento.Criacao);
        }

        public async override Task LogAtualizacao(UsuarioADE usuario, RequisitoDeUsuario requisitoDeUsuario, string Mensagem = null)
        {
            await LogAcao(usuario, requisitoDeUsuario, "ServicoRequisitoUsuario", EnumTipoLog.AlteracaoRequisitoDeUsuario, TipoEvento.Alteracao);
        }

        public async override Task LogRemocao(UsuarioADE usuario, RequisitoDeUsuario requisitoDeUsuario)
        {
            await LogAcao(usuario, requisitoDeUsuario, "ServicoRequisitoUsuario", EnumTipoLog.DelecaoRequisitoDeUsuario, TipoEvento.Delecao);
        }

        public async Task AdicionarRequisitosDeUsuarioAsync(List<DadosAlunoKV> DE, UsuarioADE usuario)
        {
            try
            {
                foreach (DadosAlunoKV dado in DE)
                {
                    if(dado.Requisito != null)
                    {
                        await ServicoAtividadeEstagio.VerificarTarefasEConcluir(usuario, EnumEntidadesSistema.Requisito, dado.Requisito.Identificador, EnumTipoAtividadeEstagio.AtualizarDados, 1);

                        RequisitoDeUsuario requisitoDeUsuario = new RequisitoDeUsuario();
                        requisitoDeUsuario.IdRequisito = dado.Requisito.Identificador;
                        requisitoDeUsuario.UserId = usuario.Id;
                        requisitoDeUsuario.Valor = dado.Value;

                        var RequisitoExistente = unitOfWork.RepositorioBase<RequisitoDeUsuario>().Filtrar(x => x.IdRequisito == requisitoDeUsuario.IdRequisito && x.UserId == requisitoDeUsuario.UserId).Result;
                        if (RequisitoExistente == null || RequisitoExistente.Count <= 0)
                        {
                            if(!string.IsNullOrWhiteSpace(requisitoDeUsuario.Valor))
                                await CadastrarAsync(usuario,requisitoDeUsuario);
                        }
                        else
                        {
                            RequisitoExistente.FirstOrDefault().Valor = requisitoDeUsuario.Valor;
                            unitOfWork.RepositorioBase<RequisitoDeUsuario>().Editar(RequisitoExistente.FirstOrDefault());
                        }
                    }
                    await unitOfWork.Commit();
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<RequisitoDeUsuario>> BuscarRequisitosDoUsuario(string IdUsuario)
        {
            return await Filtrar(x => x.UserId == IdUsuario);
        }

        public async Task<List<RequisitoDeUsuario>> BuscarRequisitoUsuario(int IdRequisito, string IdUsuario)
        {
            return await Filtrar(x => x.IdRequisito == IdRequisito && x.UserId == IdUsuario);
        }

    }
}
