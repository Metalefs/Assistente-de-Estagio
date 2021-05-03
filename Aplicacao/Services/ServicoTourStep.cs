using System.Collections.Generic;
using ADE.Dominio.Models;
using System;
using System.Threading.Tasks;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using System.Linq.Expressions;

namespace ADE.Aplicacao.Services
{
    public class ServicoTourStep: ServicoBase<TourStep>
    {
        public ServicoTourStep(ref UnitOfWork _unitOfWork):base(ref _unitOfWork)
        {}

        public async Task<List<TourStep>> BuscarTourStepParaView(string View, string Controller = null, string Area = null)
        {
            if(Controller != null && Area == null)
                return await Filtrar(x => (x.View == View) && (x.Controlador == Controller));
            if(Area != null)
                return await Filtrar(x => (x.View == View) && (x.Controlador == Controller) &&(x.Area == Area));
            return await Filtrar(x => (x.View == View));
        }

        public override Task LogCadastramento(UsuarioADE usuario, TourStep entidade)
        {
            throw new NotImplementedException();
        }

        public override Task LogAtualizacao(UsuarioADE usuario, TourStep entity, string Mensagem = null)
        {
            throw new NotImplementedException();
        }

        public override Task LogRemocao(UsuarioADE usuario, TourStep entity)
        {
            throw new NotImplementedException();
        }
    }
}
