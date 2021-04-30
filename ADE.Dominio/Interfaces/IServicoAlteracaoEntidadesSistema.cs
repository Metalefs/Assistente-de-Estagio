using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ADE.Dominio.Interfaces
{
    public interface IServicoAlteracaoEntidadesSistema
    {
        void Atualizar(AlteracaoEntidadesSistema entity);
        Task<AlteracaoEntidadesSistema> BuscarPorId(int id);
        Task CadastrarAsync(AlteracaoEntidadesSistema entity, EnumTipoLog TipoLog);
        Task<List<AlteracaoEntidadesSistema>> Filtrar(Expression<Func<AlteracaoEntidadesSistema, bool>> expression);
        Task<List<AlteracaoEntidadesSistema>> ListarAsync();
        void RemoverAsync(AlteracaoEntidadesSistema entity);
    }
}
