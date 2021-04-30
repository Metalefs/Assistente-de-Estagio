using ADE.Dominio.Models;
using System.Collections.Generic;
using System.Linq;
using Assistente_de_Estagio.Data;
using static ADE.Utilidades.Constants.NomesRequisitos;
namespace ADE.Utilidades.Seeding
{
    public class RequisitoDeDocumentoSeed
    {
        ApplicationDbContext context;
        public RequisitoDeDocumentoSeed(ApplicationDbContext _context)
        {
            context = _context;
        }

        public List<RequisitoDeDocumento> RequisitosDeclaracaoOpcaoAreaEstagio(Documento Doc)
        {
            RequisitoDeDocumento NomeAluno = new RequisitoDeDocumento(Doc.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == NomeAluno_).Identificador);
            RequisitoDeDocumento RegistroAcademico = new RequisitoDeDocumento(Doc.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == RegistroAcademico_).Identificador);
            RequisitoDeDocumento PeriodoCurso = new RequisitoDeDocumento(Doc.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == PeriodoCurso_).Identificador);
            RequisitoDeDocumento TurnoCurso = new RequisitoDeDocumento(Doc.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == TurnoCurso_).Identificador);
            RequisitoDeDocumento TelefoneAluno = new RequisitoDeDocumento(Doc.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == TelefoneAluno_).Identificador);
            RequisitoDeDocumento CelularAluno = new RequisitoDeDocumento(Doc.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == CelularAluno_).Identificador);
            RequisitoDeDocumento EmailAluno = new RequisitoDeDocumento(Doc.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == EmailAluno_).Identificador);
            RequisitoDeDocumento NomeEmpresa = new RequisitoDeDocumento(Doc.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == NomeEmpresa_).Identificador);
            RequisitoDeDocumento AreaDeEstagio = new RequisitoDeDocumento(Doc.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == TabelaAreasEstagio_).Identificador);

            return new List<RequisitoDeDocumento>()
            {
                NomeAluno,
                RegistroAcademico,
                PeriodoCurso,
                TurnoCurso,
                TelefoneAluno,
                CelularAluno,
                EmailAluno,
                NomeEmpresa,
                AreaDeEstagio
            };
        }

        public List<RequisitoDeDocumento> RequisitosSolicitacaoEstagioSupervisionado(Documento Doc)
        {
            RequisitoDeDocumento NomeEmpresa = new RequisitoDeDocumento(Doc.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == NomeEmpresa_).Identificador);
            RequisitoDeDocumento NomeSupervisor = new RequisitoDeDocumento(Doc.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == NomeSupervisor_).Identificador);
            RequisitoDeDocumento AreaDeEstagio = new RequisitoDeDocumento(Doc.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == AreasEstagio_).Identificador);
            RequisitoDeDocumento NomeAluno = new RequisitoDeDocumento(Doc.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == NomeAluno_).Identificador);
            RequisitoDeDocumento RegistroAcademico = new RequisitoDeDocumento(Doc.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == RegistroAcademico_).Identificador);
            RequisitoDeDocumento PeriodoCurso = new RequisitoDeDocumento(Doc.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == PeriodoCurso_).Identificador);
            RequisitoDeDocumento TurnoCurso = new RequisitoDeDocumento(Doc.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == TurnoCurso_).Identificador);
            
            return new List<RequisitoDeDocumento>()
            {
                NomeEmpresa,
                NomeSupervisor,
                AreaDeEstagio,
                NomeAluno,
                RegistroAcademico,
                PeriodoCurso,
                TurnoCurso
            };
        }
        
        public List<RequisitoDeDocumento> RequisitosAtestadoRealizacaoEstagio(Documento documento)
        {
            RequisitoDeDocumento NomeAluno = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == NomeAluno_).Identificador);
            RequisitoDeDocumento RGAluno = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == RGAluno_).Identificador);
            RequisitoDeDocumento CPFAluno = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == CPFAluno_).Identificador);
            RequisitoDeDocumento AreaDeEstagio = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == AreasEstagio_).Identificador);
            RequisitoDeDocumento DataInicialEstagio = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == DataInicialEstagio_).Identificador);
            RequisitoDeDocumento DataFinalEstagio = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == DataFinalEstagio_).Identificador);
            RequisitoDeDocumento HorasSemanaisEstagio = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == TotalCargaHorariaSemanalEstagio_).Identificador);
            RequisitoDeDocumento CursoAluno = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == CursoAluno_).Identificador);

            return new List<RequisitoDeDocumento>()
            {
                NomeAluno,
                RGAluno,
                CPFAluno,
                AreaDeEstagio,
                DataInicialEstagio,
                DataFinalEstagio,
                HorasSemanaisEstagio,
                CursoAluno
            };
        }

        public List<RequisitoDeDocumento> RequisitosFichaHorasEstagio(Documento documento)
        {
            RequisitoDeDocumento NomeAluno = new RequisitoDeDocumento(5, context.Requisito.FirstOrDefault(x => x.Bookmark == NomeAluno_).Identificador);
            RequisitoDeDocumento RegistroAcademico = new RequisitoDeDocumento(5, context.Requisito.FirstOrDefault(x => x.Bookmark == RegistroAcademico_).Identificador);
            RequisitoDeDocumento Turma = new RequisitoDeDocumento(5, context.Requisito.FirstOrDefault(x => x.Bookmark == TurmaAluno_).Identificador);
            RequisitoDeDocumento CargaHoraria = new RequisitoDeDocumento(5, context.Requisito.FirstOrDefault(x => x.Bookmark == TotalCargaHorariaEstagio_).Identificador);
            RequisitoDeDocumento NomeEmpresa = new RequisitoDeDocumento(5, context.Requisito.FirstOrDefault(x => x.Bookmark == NomeEmpresa_).Identificador);
           
            return new List<RequisitoDeDocumento>()
            {
                NomeAluno,
                RegistroAcademico,
                Turma,
                CargaHoraria,
                NomeEmpresa
            };
        }

        public List<RequisitoDeDocumento> RequisitosAutorizacaoRealizacaoEstagio(Documento documento)
        {
            RequisitoDeDocumento NomeAluno = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == NomeAluno_).Identificador);
            RequisitoDeDocumento DataInicialEstagio = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == DataInicialEstagio_).Identificador);
            RequisitoDeDocumento DataFinalEstagio = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == DataFinalEstagio_).Identificador);
            RequisitoDeDocumento TotalCargaHorariaEstagio = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == TotalCargaHorariaEstagio_).Identificador);
            RequisitoDeDocumento NomeSupervisor = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == NomeSupervisor_).Identificador);
            RequisitoDeDocumento FormacaoSupervisor = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == FormacaoSupervisor_).Identificador);
            RequisitoDeDocumento RGSupervisor = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == RGSupervisor_).Identificador);

            return new List<RequisitoDeDocumento>()
            {
                NomeAluno,
                DataInicialEstagio,
                DataFinalEstagio,
                TotalCargaHorariaEstagio,
                NomeSupervisor,
                FormacaoSupervisor,
                RGSupervisor
            };
        }

        public List<RequisitoDeDocumento> RequisitosAutorizacaoRealizacaoEstagioADM(Documento documento)
        {
            RequisitoDeDocumento NomeAluno = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == NomeAluno_).Identificador);
            RequisitoDeDocumento RA = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == RegistroAcademico_).Identificador);
            RequisitoDeDocumento TelefoneAluno = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == TelefoneAluno_).Identificador);
            RequisitoDeDocumento CelularAluno = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == CelularAluno_).Identificador);
            RequisitoDeDocumento EmailAluno = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == EmailAluno_).Identificador);
            RequisitoDeDocumento NomeEmpresa = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == NomeEmpresa_).Identificador);
            RequisitoDeDocumento NomeSupervisor = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == NomeSupervisor_).Identificador);

            RequisitoDeDocumento DataInicialEstagio = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == DataInicialEstagio_).Identificador);
            RequisitoDeDocumento DataFinalEstagio = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == DataFinalEstagio_).Identificador);
            
            return new List<RequisitoDeDocumento>()
            {
                NomeAluno,
                RA,
                TelefoneAluno,
                CelularAluno,
                EmailAluno,
                NomeEmpresa,
                NomeSupervisor,
                DataInicialEstagio,
                DataFinalEstagio
            };
        }

        public List<RequisitoDeDocumento> RequisitosAutorizacaoRealizacaoEstagioCC(Documento documento)
        {
            RequisitoDeDocumento NomeAluno = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == NomeAluno_).Identificador);
            RequisitoDeDocumento RA = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == RegistroAcademico_).Identificador);
            RequisitoDeDocumento TelefoneAluno = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == TelefoneAluno_).Identificador);
            RequisitoDeDocumento CelularAluno = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == CelularAluno_).Identificador);
            RequisitoDeDocumento EmailAluno = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == EmailAluno_).Identificador);
            RequisitoDeDocumento NomeEmpresa = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == NomeEmpresa_).Identificador);
            RequisitoDeDocumento NomeSupervisor = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == NomeSupervisor_).Identificador);

            RequisitoDeDocumento DataInicialEstagio = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == DataInicialEstagio_).Identificador);
            RequisitoDeDocumento DataFinalEstagio = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == DataFinalEstagio_).Identificador);

            return new List<RequisitoDeDocumento>()
            {
                NomeAluno,
                RA,
                TelefoneAluno,
                CelularAluno,
                EmailAluno,
                NomeEmpresa,
                NomeSupervisor,
                DataInicialEstagio,
                DataFinalEstagio
            };
        }

        public List<RequisitoDeDocumento> RequisitosQuestionarioEstagio(Documento documento)
        {
            RequisitoDeDocumento p1 = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == PerguntaQuestionario1).Identificador);
            RequisitoDeDocumento p2 = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == PerguntaQuestionario2).Identificador);
            RequisitoDeDocumento p3 = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == PerguntaQuestionario3).Identificador);
            RequisitoDeDocumento p4 = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == PerguntaQuestionario4).Identificador);
            RequisitoDeDocumento p5 = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == PerguntaQuestionario5).Identificador);
            RequisitoDeDocumento p6 = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == PerguntaQuestionario6).Identificador);
            RequisitoDeDocumento p7 = new RequisitoDeDocumento(documento.Identificador, context.Requisito.FirstOrDefault(x => x.Bookmark == PerguntaQuestionario7).Identificador);

            return new List<RequisitoDeDocumento>()
            {
                p1,
                p2,
                p3,
                p4,
                p5,
                p6,
                p7
            };
        }
    }
}
