using Assistente_de_Estagio.Areas.Administracao.Models.Charts.Interface;
using System.Collections.Generic;
using System.Linq;
using ADE.Dominio.Models.Individuais;

namespace Assistente_de_Estagio.Areas.Administracao.Models.Charts
{
    public class UsuariosPorCurso : BaseChart, IChart
    {
        private List<UsuarioADE> usuarios;
        public UsuariosPorCurso(List<UsuarioADE> _usuarios)
        {
            Labels = new List<string>();
            Values = new List<int>();
            usuarios = _usuarios;
            Parse();
        }
        public override void Parse()
        {
            var usuariosAgrupados = usuarios.GroupBy(
                NomesCursos => NomesCursos.IdCursoNavigation.NomeCurso,
                idCurso => idCurso.IdCurso,
                (cursos, id) => new
                {
                    Key = cursos,
                    Count = id.Count(),
                });

            foreach (var result in usuariosAgrupados)
            {
                Labels.Add(result.Key);
                Values.Add(result.Count);
            }
        }
    }
}
