using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace ADE.Dominio.Interfaces
{
    public interface IEmailMarkup 
    {
        List<KeyValuePair<string, string>> Placeholders { get; }
        Task<string> Parse();
    }
}
