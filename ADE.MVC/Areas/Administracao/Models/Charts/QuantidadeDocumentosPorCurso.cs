using Assistente_de_Estagio.Areas.Administracao.Models.Charts.Interface;
using ADE.Dominio.Models;
using System.Collections.Generic;
using System.Linq;

namespace Assistente_de_Estagio.Areas.Administracao.Models.Charts
{
    public class QuantidadeDocumentosPorCurso : BaseChart, IChart
    {
        private List<Curso> cursos;
        public QuantidadeDocumentosPorCurso(List<Curso> _cursos)
        {
            Labels = new List<string>();
            Values = new List<int>();
            cursos = _cursos;
            Parse();
        }
        public override void Parse()
        {
            var cursosAgrupados = cursos.GroupBy(
                curso => curso.Nome(),
                idDoc => idDoc.Documento,
                (curso, idDoc) => new
                {
                    Key = curso,
                    Count = idDoc.ToList().Count,
                });

            foreach (var result in cursosAgrupados)
            {
                Labels.Add(result.Key);
                Values.Add(result.Count);
            }
        }
    }
}
