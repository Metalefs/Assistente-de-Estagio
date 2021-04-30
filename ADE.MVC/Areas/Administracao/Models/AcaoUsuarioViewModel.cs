using ADE.Dominio.Models;
using ADE.Dominio.Models.Individuais;
using Assistente_de_Estagio.Areas.Shared;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Areas.Administracao.Models
{
    public class AcaoUsuarioViewModel
    {
        public PaginatedList<LogAcoesEspeciais> LogAcaoUsuario { get; set; }
        public int QuantidadeDocumentosGerados { get; set; }
        public IList<string> AutorizacaoUsuario { get; set; }
        public UsuarioADE Usuario { get; set; }
        public Curso CursoUsuario { get; set; }
    }
}
