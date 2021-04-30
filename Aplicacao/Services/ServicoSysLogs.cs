using ADE.Dominio.Models;
using ADE.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using ADE.Dominio.Models.Individuais;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ADE.Infra.Data.UOW;
using System.Linq;

namespace ADE.Aplicacao.Services
{
    public class ServicoSysLogs : IServicoBase<SysLogs>
    {
        private UnitOfWork unitOfWork;

        public ServicoSysLogs(ref UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task CadastrarAsync(UsuarioADE usuario, SysLogs sysLogs)
        {
            await unitOfWork.RepositorioBase<SysLogs>().Criar(sysLogs);
            await unitOfWork.Commit();
            Console.WriteLine(sysLogs.ToString());
        }

        public Task AtualizarAsync(UsuarioADE usuario, SysLogs entity, string Mensagem = null)
        {
            throw new NotImplementedException();
        }

        public Task RemoverAsync(UsuarioADE usuario, SysLogs entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<SysLogs>> Filtrar(Expression<Func<SysLogs, bool>> expression)
        {
            return unitOfWork.RepositorioBase<SysLogs>().Filtrar(expression);
        }

        public async Task<List<SysLogs>> ListarAsync()
        {
            List<SysLogs> Logs = await unitOfWork.RepositorioBase<SysLogs>().ListarAsync();
            return Logs.OrderByDescending(x => x.DataHoraInclusao).ToList();
        }

        public Task<SysLogs> BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
