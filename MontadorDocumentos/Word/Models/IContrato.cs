using Novacode;
using System.IO;

namespace ADE.GeradorArquivo.Word.Models
{
    public interface IContrato
    {
        Stream Gerar();
    }
}
