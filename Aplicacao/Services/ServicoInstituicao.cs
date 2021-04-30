using ADE.Dominio.Interfaces;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ADE.Aplicacao.Services
{
    public class ServicoInstituicao : ServicoBase<Instituicao>, IServicoBase<Instituicao>
    {
        private UnitOfWork unitOfWork;
        
        public ServicoInstituicao(ref UnitOfWork _unitOfWork) : base(ref _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public override async Task LogCadastramento(UsuarioADE usuario, Instituicao Instituicao)
        {
            await LogAcao(usuario, Instituicao, "ServicoInstituicao", EnumTipoLog.AlteracaoCurso, TipoEvento.Criacao);
            List<Instituicao> confirmacao = await Filtrar(x => x.Nome == Instituicao.Nome);
            Instituicao LogCriacao = confirmacao.FirstOrDefault() ?? new Instituicao() { Nome = "Erro", Color = "red" };
            await LogAlteracaoEntidade(usuario, LogCriacao, Instituicao, EnumEntidadesSistema.Instituicao, EnumTipoLog.CriacaoInstituicao);
        }

        public override async Task LogAtualizacao(UsuarioADE usuario, Instituicao Instituicao, string Mensagem = null)
        {
            Instituicao InstituicaoAntiga = await BuscarPorId(Instituicao.Identificador);
            await LogAlteracaoEntidade(usuario, InstituicaoAntiga, Instituicao, EnumEntidadesSistema.Instituicao, EnumTipoLog.AlteracaoInstituicao, Mensagem);
            InstituicaoAntiga.Clonar(Instituicao);
            await LogAcao(usuario, Instituicao, "ServicoInstituicao", EnumTipoLog.AlteracaoCurso, TipoEvento.Alteracao);
        }

        public override async Task LogRemocao(UsuarioADE usuario, Instituicao Instituicao)
        {
            await LogAcao(usuario, Instituicao, "ServicoInstituicao", EnumTipoLog.AlteracaoCurso, TipoEvento.Delecao);
            await LogDelecaoEntidade(usuario, Instituicao, EnumEntidadesSistema.Instituicao, EnumTipoLog.DelecaoInstituicao);
        }

        public new async Task<Instituicao> BuscarPorId(int id)
        {
            try
            {
                Instituicao instituicao = await unitOfWork.RepositorioBase<Instituicao>().BuscarUm(x => x.Identificador == id);
                if(instituicao != null)
                {
                    return instituicao;
                }
                else
                {
                    return new Instituicao()
                    {
                        Nome = "Não encontrado",
                        Color = "red"
                    };
                }
            }
            catch(Exception ex)
            {
                await LogError(new UsuarioADE(), ex.Message, ex.Source, EnumTipoLog.ErroInterno);
                return new Instituicao()
                {
                    Nome = "Não encontrado",
                    Color = "red"
                };
            }
        }
    }
}
