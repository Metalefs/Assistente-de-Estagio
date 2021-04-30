using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ADE.Dominio.Interfaces
{
    public interface IServicoInformacao<T>
    {
        Task <List<T>> RecuperarInformacao(int id);
    }
}
