using System.Collections.Generic;
using ADE.Dominio.Models;
using ADE.Dominio.Interfaces;
using System;
using System.Threading.Tasks;
using ADE.Dominio.Models.Individuais;
using ADE.Dominio.Models.Enums;
using ADE.Infra.Data.UOW;
using System.Linq;

namespace ADE.Aplicacao.Services
{
    public class ServicoCurso : ServicoBase<Curso>, IServicoBase<Curso>, ICountable
    {
        private UnitOfWork unitOfWork;

        public ServicoCurso(ref UnitOfWork _unitOfWork) : base(ref _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public override async Task LogCadastramento(UsuarioADE usuario, Curso Curso)
        {
            await LogAcao(usuario,Curso,"ServicoCurso",EnumTipoLog.CriacaoCurso,TipoEvento.Criacao);
            List<Curso> confirmacao = await Filtrar(x => x.NomeCurso == Curso.NomeCurso);
            Curso LogCriacao = confirmacao.FirstOrDefault() ?? new Curso() { NomeCurso = "Erro"};
            await LogAlteracaoEntidade(usuario, LogCriacao, Curso, EnumEntidadesSistema.Curso, EnumTipoLog.CriacaoCurso);
        }
        
        public override async Task LogAtualizacao(UsuarioADE usuario, Curso CursoNovo, string Mensagem = null)
        {
            Curso CursoAntigo = await BuscarPorId(CursoNovo.Identificador);
            string TituloAntigo = CursoAntigo.NomeCurso;
            await LogAlteracaoEntidade(usuario, CursoAntigo, CursoNovo, EnumEntidadesSistema.Curso, EnumTipoLog.AlteracaoCurso, Mensagem);
            CursoAntigo.Clonar(CursoNovo);
            await LogAtualizacaoComTituloAntigo(usuario, CursoAntigo, TituloAntigo);
        }

        private async Task LogAtualizacaoComTituloAntigo(UsuarioADE usuario, Curso Curso, string TituloAntigo)
        {
            Curso.NomeCurso = TituloAntigo;
            await LogAcao(usuario, Curso, "ServicoCurso", EnumTipoLog.AlteracaoCurso, TipoEvento.Alteracao);
        }

        public override async Task LogRemocao(UsuarioADE usuario, Curso Curso)
        {
            await LogAcao(usuario, Curso, "ServicoCurso", EnumTipoLog.DelecaoCurso, TipoEvento.Delecao);
            await LogDelecaoEntidade(usuario, Curso, EnumEntidadesSistema.Curso, EnumTipoLog.DelecaoCurso);
        }

        public new async Task<Curso> BuscarPorId(int id)
        {
            try
            {
                return await unitOfWork.RepositorioBase<Curso>().BuscarUm(x=>x.Identificador == id);
            }
            catch (InvalidOperationException)
            {
                return new Curso() { NomeCurso = "Curso indisponível", DescricaoCurso = "Esse curso pode ter sido excluido, favor entrar em contato com o suporte suporte@assistentedeestagio.com"};
            }
        }

    }
}
