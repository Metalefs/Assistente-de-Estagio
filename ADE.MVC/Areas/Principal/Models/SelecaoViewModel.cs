using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADE.Principal.Models;
using ADE.Dominio.Models;
using Assistente_de_Estagio.Models;
using Assistente_de_Estagio.Areas.Shared;
using Assistente_de_Estagio.Areas.Principal.Models;

namespace Assistente_de_Estagio.Models
{
    public class SelecaoViewModel
    {
        public IList<Documento> Documentos { get; set; }
        public Curso Curso { get; set; }
        public PaginatedList<InformacaoCursoVM> Cursos { get; set; }
        public DocumentoViewModel DocumentoViewModel { get; set; }
        public bool PrimeiroCurso { get; set; }
        public List<InformacaoCurso> InformacaoCurso { get; set; }
        public List<HistoricoGeracaoDocumento> HistoricoGeracaoDocumento { get; set; }
        public PaginatedList<Documento> Paginated { get; set; }

        public SelecaoViewModel() { }

        public SelecaoViewModel(IList<Documento> documentos, PaginatedList<InformacaoCursoVM> cursos, Curso curso, DocumentoViewModel documentoViewModel, List<HistoricoGeracaoDocumento> historicoGeracaoDocumento, List<InformacaoCurso> informacaoCurso)
        {
            Documentos = documentos;
            Curso = curso;
            Cursos = cursos;
            DocumentoViewModel = documentoViewModel;
            HistoricoGeracaoDocumento = historicoGeracaoDocumento;
            InformacaoCurso = informacaoCurso;
        }
    }
}
