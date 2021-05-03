using ADE.Dominio.Models;
using System;
using System.Collections.Generic;
using static ADE.Utilidades.Constants.NomesRequisitos;
using ADE.Dominio.Models.Enums;

namespace ADE.Utilidades.Seeding
{
    public static class RequisitoSeed
    {
        public static List<Requisito> RequisitosBase()
        {
            List<Requisito> Requisitos;
            Requisitos = new List<Requisito>()
            {
                AreasEstagio,
                TabelaAreasEstagio,
                NomeAluno,
                Email,
                RegistroAcademico,
                RGAluno,
                CPFAluno,
                CelularAluno,
                TelefoneAluno,
                NomeEmpresa,
                CNPJEmpresa,
                NomeSupervisor,
                FormacaoAcademicaSupervisor,
                RGSupervisor,
                CRCSupervisor,
                CRASupervisor,
                DataInicialEstagio,
                DataFinalEstagio,
                TotalCargaHorariaEstagio,
                HorasSemanaisEstagio,
                PeriodoCurso,
                TurnoCurso,
                PerguntaQuestionario1,
                PerguntaQuestionario2,
                PerguntaQuestionario3,
                PerguntaQuestionario4,
                PerguntaQuestionario5,
                PerguntaQuestionario6,
                PerguntaQuestionario7
            };
            return Requisitos;
        }
        public static Requisito TabelaAreasEstagio = new Requisito("Areas de Estágio", "",EnumElementoHTMLRequisito.select, EnumTipoRequisito.text, TabelaAreasEstagio_ , EnumGrupoRequisito.Aluno, 40, false, null, false);
        public static Requisito AreasEstagio = new Requisito("Areas de Estágio", "",EnumElementoHTMLRequisito.select, EnumTipoRequisito.text, AreasEstagio_, EnumGrupoRequisito.Aluno, 40, true, null, false);
        public static Requisito RegistroAcademico = new Requisito("Registro Acadêmico", "Registro Acadêmico (RA) do aluno", EnumElementoHTMLRequisito.input, EnumTipoRequisito.text, RegistroAcademico_, EnumGrupoRequisito.Aluno, 10, true, null, false);
        public static Requisito NomeAluno = new Requisito("Nome do Aluno(a) aplicante de estágio", "Nome do aluno aplicante do estágio", EnumElementoHTMLRequisito.input, EnumTipoRequisito.text, NomeAluno_, EnumGrupoRequisito.Aluno, 47, true, null, true);
        public static Requisito TurmaAluno = new Requisito("Turma do Aluno(a) ", "Turma do aluno aplicante do estágio na faculdade", EnumElementoHTMLRequisito.input, EnumTipoRequisito.text, TurmaAluno_, EnumGrupoRequisito.Aluno, 10, true, null, false);
        public static Requisito Email = new Requisito("Email do Aluno(a) ", "Email do aluno aplicante do estágio", EnumElementoHTMLRequisito.input, EnumTipoRequisito.email, EmailAluno_, EnumGrupoRequisito.Aluno, 36, true, null, true);
        public static Requisito CelularAluno = new Requisito("Celular", "Número de telefone celular do Aluno", EnumElementoHTMLRequisito.input, EnumTipoRequisito.text, CelularAluno_, EnumGrupoRequisito.Aluno, 21, true, "(00) 90000-0000", false);
        public static Requisito TelefoneAluno = new Requisito("Telefone", "Número de telefone fixo do Aluno", EnumElementoHTMLRequisito.input, EnumTipoRequisito.text, TelefoneAluno_, EnumGrupoRequisito.Aluno, 21, true, "(00) 90000-0000", false);
        public static Requisito RGAluno = new Requisito("RG do aluno", "Numero do Registro Geral do aluno", EnumElementoHTMLRequisito.input, EnumTipoRequisito.text, RGAluno_, EnumGrupoRequisito.Aluno, 21, true, "AA-99.999.999");
        public static Requisito CPFAluno = new Requisito("CPF do aluno", "Número de Cadastro de Pessoa Física do Aluno", EnumElementoHTMLRequisito.input, EnumTipoRequisito.text, CPFAluno_, EnumGrupoRequisito.Aluno, 47, true, "999.999.999-99", false);
        public static Requisito NomeEmpresa = new Requisito("Nome da empresa contratante", "Nome da empresa onde o estágio será realizado", EnumElementoHTMLRequisito.input, EnumTipoRequisito.text, NomeEmpresa_, EnumGrupoRequisito.Empresa, 21, true, null, false);
        public static Requisito CNPJEmpresa = new Requisito("CNPJ da empresa contratante", "CNPJ da empresa onde o estágio será realizado", EnumElementoHTMLRequisito.input, EnumTipoRequisito.text, CNPJEmpresa_, EnumGrupoRequisito.Empresa, 21, true, "99.999.999/9999-99", false);
        public static Requisito NomeSupervisor = new Requisito("Nome do Supervisor de estágio", "Nome do coordenador de estágio do aluno", EnumElementoHTMLRequisito.input, EnumTipoRequisito.text, NomeSupervisor_, EnumGrupoRequisito.Empresa, 47, true, null, false);
        public static Requisito FormacaoAcademicaSupervisor = new Requisito("Formação Acadêmica do Supervisor de estágio", "Formação Acadêmica do supervisor de estágio do aluno", EnumElementoHTMLRequisito.input, EnumTipoRequisito.text, FormacaoSupervisor_, EnumGrupoRequisito.Empresa, 47, true, null, false);
        public static Requisito RGSupervisor = new Requisito("RG do Supervisor de estágio", "Numero do Registro Geral do coordenador de estágio do aluno", EnumElementoHTMLRequisito.input, EnumTipoRequisito.text, RGSupervisor_, EnumGrupoRequisito.Empresa, 47, true, "AA-99.999.999", false);
        public static Requisito CRASupervisor = new Requisito("CRC do Supervisor de estágio", "Numero do Conselho Regional de Administração do coordenador de estágio do aluno", EnumElementoHTMLRequisito.input, EnumTipoRequisito.text, CRASupervisor_, EnumGrupoRequisito.Empresa, 47, true, null, false);
        public static Requisito CRCSupervisor = new Requisito("CRA do Supervisor de estágio", "Numero do Conselho Regional de Contabilidade do coordenador de estágio do aluno", EnumElementoHTMLRequisito.input, EnumTipoRequisito.text, CRCSupervisor_, EnumGrupoRequisito.Empresa, 47, true, null, false);
        public static Requisito DataInicialEstagio = new Requisito("Data de inicio do estágio", "Data em que o aluno começou o processo de estágio", EnumElementoHTMLRequisito.input, EnumTipoRequisito.date, DataInicialEstagio_, EnumGrupoRequisito.Aluno, 47, true, null, false);
        public static Requisito DataFinalEstagio = new Requisito("Data do fim do estágio", "Data em que o aluno finalizou o processo de estágio", EnumElementoHTMLRequisito.input, EnumTipoRequisito.date, DataFinalEstagio_, EnumGrupoRequisito.Aluno, 47, true, null, false);
        public static Requisito TotalCargaHorariaEstagio = new Requisito("Carga Horária total", "Contabilização de horas totais em estágio", EnumElementoHTMLRequisito.input, EnumTipoRequisito.number, TotalCargaHorariaEstagio_, EnumGrupoRequisito.Aluno, 47, true, null, false);
        public static Requisito HorasSemanaisEstagio = new Requisito("Carga Horaria Semanal", "Contabilização de horas semanais em estágio", EnumElementoHTMLRequisito.input, EnumTipoRequisito.number, TotalCargaHorariaSemanalEstagio_, EnumGrupoRequisito.Aluno, 47, true, null, false);


        public static Requisito PerguntaQuestionario1 = new Requisito("O estágio tem permitido que você adquira conhecimentos práticos, contribuindo para a sua formação profissional?", "", EnumElementoHTMLRequisito.input, EnumTipoRequisito.text, "PerguntaQuestionario1", EnumGrupoRequisito.Questionario, 47, true, null, false);
        public static Requisito PerguntaQuestionario2 = new Requisito("Em que medida as atividades de estágio tem favorecido o aproveitamento do seu curso, estimulando seus estudos e leituras?", "", EnumElementoHTMLRequisito.input, EnumTipoRequisito.text, "PerguntaQuestionario2", EnumGrupoRequisito.Questionario, 47, true, null, false);
        public static Requisito PerguntaQuestionario3 = new Requisito("Como experiência de trabalho, como você classifica os conhecimentos e informações que está adquirindo, para a sua futura profissionalização?", "", EnumElementoHTMLRequisito.input, EnumTipoRequisito.text, "PerguntaQuestionario3", EnumGrupoRequisito.Questionario, 47, true, null, false);
        public static Requisito PerguntaQuestionario4 = new Requisito("Permitindo a convivência com outros profissionais, em que medida o estágio tem contribuído para desenvolver seu espírito de equipe?", "", EnumElementoHTMLRequisito.input, EnumTipoRequisito.text, "PerguntaQuestionario4", EnumGrupoRequisito.Questionario, 47, true, null, false);
        public static Requisito PerguntaQuestionario5 = new Requisito("Para o desenvolvimento de suas atividades, como você considera os conhecimentos teóricos do seu curso?", "", EnumElementoHTMLRequisito.input, EnumTipoRequisito.text, "PerguntaQuestionario5", EnumGrupoRequisito.Questionario, 47, true, null, false);
        public static Requisito PerguntaQuestionario6 = new Requisito("Como está sendo a supervisão de seu estágio na instituição? ", "", EnumElementoHTMLRequisito.input, EnumTipoRequisito.text, "PerguntaQuestionario6", EnumGrupoRequisito.Questionario, 47, true, null, false);
        public static Requisito PerguntaQuestionario7 = new Requisito("Em termos de satisfação pessoal e aproveitamento profissional, como você avalia o seu estágio?", "", EnumElementoHTMLRequisito.input, EnumTipoRequisito.text, "PerguntaQuestionario7", EnumGrupoRequisito.Questionario, 47, true, null, false);


        static List<OpcaoRequisito> OpcoesPeridoCurso = new List<OpcaoRequisito>()
        {
            new OpcaoRequisito("1°"),
            new OpcaoRequisito("2°"),
            new OpcaoRequisito("3°"),
            new OpcaoRequisito("4°"),
            new OpcaoRequisito("5°"),
            new OpcaoRequisito("6°"),
            new OpcaoRequisito("7°"),
            new OpcaoRequisito("8°"),
        };
        public static Requisito PeriodoCurso = new Requisito("Período do Curso", "Período (Etapa) do curso do aluno", EnumElementoHTMLRequisito.select, EnumTipoRequisito.number, PeriodoCurso_, EnumGrupoRequisito.Faculdade, 11, false, null, true, OpcoesPeridoCurso);
        static List<OpcaoRequisito> OpcoesTurnoCurso = new List<OpcaoRequisito>()
        {
            new OpcaoRequisito("Manhã"),
            new OpcaoRequisito("Tarde"),
            new OpcaoRequisito("Noite"),
        };
        public static Requisito TurnoCurso = new Requisito("Turno do Curso", "Turno em que o aluno realiza o curso", EnumElementoHTMLRequisito.select, EnumTipoRequisito.text, TurnoCurso_, EnumGrupoRequisito.Faculdade, 20, false, null, true, OpcoesTurnoCurso);
    }
}
