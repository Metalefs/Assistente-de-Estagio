using ADE.Dominio.Models;
using System.Collections.Generic;

namespace ADE.Utilidades.Seeding
{
    public static class AreasEstagioSeed
    {
        public static List<AreaEstagioCurso> AreasDeEstagioADS(Curso curso) => new List<AreaEstagioCurso>()
        {
            new AreaEstagioCurso(curso.Identificador,"Administração de Banco de Dados"),
            new AreaEstagioCurso(curso.Identificador,"Administração de Redes"),
            new AreaEstagioCurso(curso.Identificador,"Projetos"),
            new AreaEstagioCurso(curso.Identificador,"Gerência de Equipes"),
            new AreaEstagioCurso(curso.Identificador,"Desenvolvimento")
        };
        public static List<AreaEstagioCurso> AreasDeEstagioLET(Curso curso) => new List<AreaEstagioCurso>()
        {
            new AreaEstagioCurso(curso.Identificador,"Espaço Escolar - Ensino Fundamental (6° ao 9° ano) Minimo 280h/ Máximo 400h"),
            new AreaEstagioCurso(curso.Identificador,"Espaço Escolar - Ensino Médio (1° ao 3° ano) Minimo 280h/ Máximo 400h"),
            new AreaEstagioCurso(curso.Identificador,"Outros Espaços Educativos Formais e Informais Minimo 0h/ Máximo 120h"),
            new AreaEstagioCurso(curso.Identificador,"Atividades de Pesquisa-Ação Minimo 0h/ Máximo 120h"),
            new AreaEstagioCurso(curso.Identificador,"Outros : ")
        };
    }
}
