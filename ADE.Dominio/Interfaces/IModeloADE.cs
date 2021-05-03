using System;

namespace ADE.Dominio.Interfaces
{
    public interface IModeloADE
    {
        int Identificador { get; set; }
        DateTime DataHoraInclusao { get; set; }
        DateTime DataHoraUltimaAlteracao { get; set; }
        DateTime? DataHoraExclusao { get; set; }

        string ToString();
    }
}
