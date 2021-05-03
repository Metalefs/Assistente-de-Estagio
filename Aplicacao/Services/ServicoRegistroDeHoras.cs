using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using ADE.Utilidades.Extensions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ADE.GeradorArquivo.Word;
using System.IO;

namespace ADE.Aplicacao.Services
{
    public class ServicoRegistroDeHoras : ServicoBase<RegistroDeHoras>
    {
        ServicoCurso servicoCurso;
        private MontadorDocX GeradorDocumento;
        public ServicoRegistroDeHoras(ref UnitOfWork unitOfWork):base(ref unitOfWork)
        { 
            servicoCurso = new ServicoCurso(ref unitOfWork);
            GeradorDocumento = new MontadorDocX();
        }

        public async Task<ArquivoDownload> GerarTabelaHistorico(UsuarioADE usuario, Documento tabela, RequisitosBasicosCabecalho requisitosFichaRegistroHoras)
        {
            List<RegistroDeHoras> registros = await ObterRegistrosUsuario(usuario);
            int codigo = await RecuperarCodigoHistoricoGeracaoDocumento();
            if(registros.Count == 0)
            {
                throw new Exception("É necessário ao menos dois registros para realizar a exportação do histórico.");
            }
            MemoryStream file = (MemoryStream)GeradorDocumento.GerarDocumentoRegistroHoras(tabela, registros, requisitosFichaRegistroHoras, codigo);
            ArquivoDownload arquivoDownload = new ArquivoDownload(file, EnumFormatoDocumento.docx);
            await LogGeracaoDocumento(tabela, usuario);
            return arquivoDownload;
        }

        public async Task<List<RegistroDeHoras>> ObterRegistrosUsuario(UsuarioADE usuario)
        {
            return await Filtrar(x => x.IdUsuario == usuario.Id);
        }

        public async Task<List<RegistroDeHoras>> ObterRegistrosPorData(UsuarioADE usuario, DateTime date)
        {
            return await Filtrar(x => x.IdUsuario == usuario.Id && x.DataHoraUltimaAlteracao.ToShortDateString() == date.ToShortDateString());
        }

        public async Task<List<RegistroDeHoras>> ObterRegistrosPorPeriodo(UsuarioADE usuario, DateTime dataInicial, DateTime datafinal)
        {
            return await Filtrar(x => x.IdUsuario == usuario.Id && x.DataHoraUltimaAlteracao >= dataInicial && x.IdUsuario == usuario.Id && x.DataHoraUltimaAlteracao >= datafinal);
        }

        public async Task<float> ObterContabilizacaoHorasUsuario(UsuarioADE usuario)
        {
            List<RegistroDeHoras> Registros = await Filtrar(x => x.IdUsuario == usuario.Id);
            return Registros.Sum(x => x.CargaHoraria);
        }

        public async Task<int> ObterTotalCargaHorariaCursoUsuario(UsuarioADE usuario)
        {
            Curso curso = await servicoCurso.BuscarUm(x => x.Identificador == usuario.IdCurso);
            return curso.CargaHorariaMinimaEstagio;
        }

        public async Task<List<RegistroDeHoras>> ObterUltimosRegistrosUsuario(UsuarioADE usuario)
        {
            List<RegistroDeHoras> lista = await FiltrarComQuantidade(x => x.IdUsuario == usuario.Id,7);
            return lista.OrderByDescending(x=>x.DataHoraInclusao).ToList();
        }

        public override async Task LogCadastramento(UsuarioADE usuario, RegistroDeHoras entidade)
        {
            string Mensagem = $"Sua atividade {entidade.Atividade} foi cadastrada com a carga horaria de {entidade.CargaHoraria} minutos";
            string Cabecalho = MensagemCriacaoRegistroHoras(usuario, entidade);
            NotificacaoIndividual notificacao = new NotificacaoIndividual(usuario.Id, usuario.Id, Cabecalho, Mensagem);
            await CriarNotificacaoIndividual(usuario, notificacao);
        }

        public override async Task LogAtualizacao(UsuarioADE usuario, RegistroDeHoras entidade, string Mensagem = null)
        {
            Mensagem = Mensagem ?? entidade.GerarMensagemAlteracao(entidade);
            string Cabecalho = MensagemAlteracaoRegistroHoras(usuario, entidade, entidade);
            NotificacaoIndividual notificacao = new NotificacaoIndividual(usuario.Id, usuario.Id, Cabecalho, Mensagem);
            await CriarNotificacaoIndividual(usuario, notificacao);
        }

        private string MensagemCriacaoRegistroHoras(UsuarioADE usuario, RegistroDeHoras entidade) =>
        
            $"Cadastramento de registro de horas.";

        private string MensagemAlteracaoRegistroHoras(UsuarioADE usuario, RegistroDeHoras entidade, RegistroDeHoras registroAntesDaAtualizacao) =>

           $"Alteração de registro de horas.";

        private string MensagemDelecaoRegistroHoras(UsuarioADE usuario, RegistroDeHoras entidade) =>

           $"Remoção de registro de horas.";

        public override async Task LogRemocao(UsuarioADE usuario, RegistroDeHoras entity)
        {
            string Cabecalho = MensagemDelecaoRegistroHoras(usuario, entity);
            NotificacaoIndividual notificacao = new NotificacaoIndividual(entity.IdUsuario, entity.IdUsuario, Cabecalho, $"{usuario.UserName}, sua atividade {entity.Atividade} foi removida com sucesso. Data: {DateTime.Now.ToLocalTime()} ");
            await CriarNotificacaoIndividual(usuario, notificacao);
        }
    }
}
