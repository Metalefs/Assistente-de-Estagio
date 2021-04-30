using System;
using System.Collections.Generic;
using System.Text;

namespace ADE.Dominio.Interfaces
{
    public interface IColorCoded
    {
        string Color { get; set; }
        byte[] Logo { get; set; }
        string Nome { get; set; }
    }
}
