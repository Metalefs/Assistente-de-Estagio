using ADE.Dominio.Interfaces;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADE.Aplicacao.Services
{
    public class ServicoRequisitoDeDocumento : ServicoBase<RequisitoDeDocumento>, IServicoBase<RequisitoDeDocumento>
    {
        private ServicoRequisito _servicoRequisito;
        public ServicoRequisitoDeDocumento(ref UnitOfWork _unitOfWork) : base (ref _unitOfWork) { _servicoRequisito = new ServicoRequisito(ref _unitOfWork); } 

        public async override Task LogCadastramento(UsuarioADE usuario, RequisitoDeDocumento RDD)
        {
            await LogAcao(usuario, RDD, "ServicoRequisitoDeDocumento", EnumTipoLog.CriacaoRequisitoDeDocumento, TipoEvento.Alteracao);
        }

        public async override Task LogAtualizacao(UsuarioADE usuario, RequisitoDeDocumento RDD, string Mensagem = null)
        {
            await LogAcao(usuario, RDD, "ServicoRequisitoDeDocumento", EnumTipoLog.AlteracaoRequisitoDeDocumento, TipoEvento.Alteracao);
        }

        public async override Task LogRemocao(UsuarioADE usuario, RequisitoDeDocumento RDD)
        {
            await LogAcao(usuario, RDD, "ServicoRequisitoDeDocumento", EnumTipoLog.DelecaoRequisitoDeDocumento, TipoEvento.Alteracao);
        }

        public async Task<RequisitoDeDocumento> BuscarPorId(int idDocumento, int IdRequisito)
        {
            try
            {
                List<RequisitoDeDocumento> lrdd = await Filtrar(x => x.IdDocumento == idDocumento && x.IdRequisito == IdRequisito);
                return lrdd.First();
            }
            catch(System.Exception)
            {
                return new RequisitoDeDocumento();
            }
        }

        public async Task<List<RequisitoDeDocumento>> ListarRegistros(int idDocumento)
        {
            List<RequisitoDeDocumento> lista = await Filtrar(x => x.IdDocumento == idDocumento);
            foreach(RequisitoDeDocumento rdd in lista)
            {
                rdd.IdRequisitoNavigation = await _servicoRequisito.BuscarPorId(rdd.IdRequisito);
            }
            return lista;
        }
    }
}
