using ADE.Dominio.Models;
using ADE.Dominio.Interfaces;
using ADE.Dominio.Models.Individuais;
using System.Threading.Tasks;
using ADE.Dominio.Models.RelacaoEntidades;

namespace ADE.Dominio.Interfaces
{
    public interface IUnitOfWork
    {
        IRepositorioAlteracaoEntidades RepositorioAlteracaoEntidades { get; }
        IRepositorioRecuperacaoEntidades<T> RepositorioRecuperacao<T>() where T : ModeloBase, IRecuperavel;
        IRepositorioBase<T> RepositorioBase<T>() where T : ModeloBase;
        IRepositorioUsuario RepositorioUsuario { get; }
        Task Commit();
    }
}
