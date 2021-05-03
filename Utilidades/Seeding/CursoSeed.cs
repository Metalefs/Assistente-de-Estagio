using ADE.Dominio.Models;
using ADE.Dominio.Models.Individuais;
using ADE.Dominio.Models.Enums;

namespace ADE.Utilidades.Seeding
{
    public static class CursoSeed
    {
        public static Curso ADS(Instituicao instituicao) => new Curso()
        {
            NomeCurso = "Análise e Desenvolvimento de Sistemas",
            Sigla = "ADS",
            DescricaoCurso = "Analisa, projeta, desenvolve, testa, implanta e mantém sistemas computacionais de informação. Avalia, seleciona, especifica e utiliza metodologias, tecnologias e ferramentas da Engenharia de Software, linguagens de programação e bancos de dados. Coordena equipes de produção de softwares. Vistoria, realiza perícia, avalia, emite laudo e parecer técnico em sua área de formação.",
            CoordenadorCurso = "André rodrigues",
            EmailCoordenadorCurso = "andre.augusto.rodrigues@hotmail.com",
            TipoCurso = EnumTipoCurso.Tecnologo,
            IdInstituicao = instituicao.Identificador,
            CargaHorariaMinimaEstagio = 200
        };
        public static Curso ADM(Instituicao instituicao) => new Curso()
        {
            NomeCurso = "Administração",
            Sigla = "ADM",
            DescricaoCurso = "As teorias da Administração, sua evolução e aplicabilidade; Plano de negócios; Marketing; Recursos Humanos; Produção, Logística; Orçamento empresarial; Custos; Controladoria; Contabilidade; Economia micro e macro; Direito do trabalho; Tributário; Previdenciário e empresarial; Estatística; Métodos quantitativos; Processo decisório e outros conteúdos necessários a uma formação completa do profissional de Administração.",
            CoordenadorCurso = "Francisco",
            EmailCoordenadorCurso = "proffrancisco@facisa.com.br",
            TipoCurso = EnumTipoCurso.Graduacao,
            IdInstituicao = instituicao.Identificador,
            CargaHorariaMinimaEstagio = 400
        };
        public static Curso CC(Instituicao instituicao) => new Curso()
        {
            NomeCurso = "Ciências Contábeis",
            Sigla = "CC",
            DescricaoCurso = "O curso Ciências Contábeis da FACISABH é composto por disciplinas que em seu conjunto buscam a formação de um profissional preparado para atuar com segurança e assertividade tanto na área contábil quanto na área financeira.",
            CoordenadorCurso = "Helenice",
            EmailCoordenadorCurso = "helenice@facisa.com.br",
            TipoCurso = EnumTipoCurso.Graduacao,
            IdInstituicao = instituicao.Identificador,
            CargaHorariaMinimaEstagio = 400
        };
        public static Curso LET(Instituicao instituicao) => new Curso()
        {
            NomeCurso = "Letras",
            Sigla = "LET",
            DescricaoCurso = "A faculdade de Letras dedica-se ao estudo da língua portuguesa e da literatura. Na maioria dos casos, estuda também um outro idioma, sua estrutura linguística e obras literárias. Um profissional formado em Letras pode ser professor das disciplinas de português, literatura, redação e idioma estrangeiro.",
            CoordenadorCurso = "Rafael",
            EmailCoordenadorCurso = "profrafael@facisa.com.br",
            TipoCurso = EnumTipoCurso.Graduacao,
            IdInstituicao = instituicao.Identificador,
            CargaHorariaMinimaEstagio = 400
        };
        public static Curso PED(Instituicao instituicao) => new Curso()
        {
            NomeCurso = "Pedagogia",
            Sigla = "PED",
            DescricaoCurso = "Profissionais da área de Pedagogia atuam na educação infantil, nos anos iniciais do ensino fundamental e na educação de jovens e adultos. Além disso, trabalham em funções de suporte pedagógico, como direção, supervisão e orientação educacional.",
            CoordenadorCurso = "Fernanda Soares",
            EmailCoordenadorCurso = "proffernandasoares@facisa.com.br",
            TipoCurso = EnumTipoCurso.Bacharel,
            IdInstituicao = instituicao.Identificador,
            CargaHorariaMinimaEstagio = 400
        };
    }
}
