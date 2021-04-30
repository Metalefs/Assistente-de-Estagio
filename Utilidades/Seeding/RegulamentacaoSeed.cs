using ADE.Dominio.Models;

namespace ADE.Utilidades.Seeding
{
    public static class RegulamentacaoSeed
    {
        public static RegulamentacaoCurso ADS(Curso ADS)
        {
            string Path = "/Documentos/Regulamentacao/FACISABH/REGULAMENTO-ADS.pdf";
            return new RegulamentacaoCurso(ADS.Identificador, Path);
        }
        public static RegulamentacaoCurso ADM(Curso ADM)
        {
            string Path = "/Documentos/Regulamentacao/FACISABH/REGULAMENTO-ADM.pdf";
            return new RegulamentacaoCurso(ADM.Identificador, Path);
        }
        public static RegulamentacaoCurso CC(Curso CC)
        {
            string Path = "/Documentos/Regulamentacao/FACISABH/REGULAMENTO-CC.pdf";
            return new RegulamentacaoCurso(CC.Identificador, Path);
        }
        public static RegulamentacaoCurso LET(Curso LET)
        {
            string Path = "/Documentos/Regulamentacao/FACISABH/REGULAMENTO-LET.pdf";
            return new RegulamentacaoCurso(LET.Identificador, Path);
        }
        public static RegulamentacaoCurso PED(Curso PED)
        {
            string Path = "/Documentos/Regulamentacao/FACISABH/REGULAMENTO-PED.pdf";
            return new RegulamentacaoCurso(PED.Identificador, Path);
        }
    }
}
