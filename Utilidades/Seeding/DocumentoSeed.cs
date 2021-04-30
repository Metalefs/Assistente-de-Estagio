using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using System;
using static ADE.Utilidades.Constants.NomesRequisitos;
using static ADE.Utilidades.Constants.NomesDocumentos;
using System.Collections.Generic;

namespace ADE.Utilidades.Seeding
{
    public static class DocumentoSeed
    {

        public static List<Documento> DocumentosADS(Curso ADS) =>
            new List<Documento>(){
                DeclaracaoOpcaoAreaEstágio(ADS),
                AutorizacaoRealizacaoEstagio(ADS),
                AtestadoRealizacaoEstagio(ADS),
                SolicitacaoEstagioSupervisionado(ADS),
                QuestionarioEstagio(ADS)
            }
        ;

        public static List<Documento> DocumentosADM(Curso ADM) =>
           new List<Documento>(){
                AutorizacaoRealizacaoEstagioADM(ADM),
                AtestadoRealizacaoEstagio(ADM),
                SolicitacaoEstagioSupervisionado(ADM),
                QuestionarioEstagio(ADM)
           }
        ;

        public static List<Documento> DocumentosCC(Curso CC) =>
           new List<Documento>(){
                AutorizacaoRealizacaoEstagioCC(CC),
                AtestadoRealizacaoEstagio(CC),
                SolicitacaoEstagioSupervisionado(CC),
                QuestionarioEstagio(CC)
           }
        ;

        public static List<Documento> DocumentosLET(Curso LET) =>
           new List<Documento>(){
                DeclaracaoOpcaoAreaEstágio(LET),
                AutorizacaoRealizacaoEstagio(LET),
                AtestadoRealizacaoEstagio(LET),
                SolicitacaoEstagioSupervisionado(LET),
                QuestionarioEstagio(LET)
           }
        ;

        public static List<Documento> DocumentosPED(Curso CC) =>
           new List<Documento>()
           {
                DeclaracaoOpcaoAreaEstágio(CC),
                AutorizacaoRealizacaoEstagio(CC),
                AtestadoRealizacaoEstagio(CC),
                SolicitacaoEstagioSupervisionado(CC),
                QuestionarioEstagio(CC)
           }
        ;

        public static Documento DeclaracaoOpcaoAreaEstágio(Curso curso) => new Documento()
        {
            TituloDocumento = DeclaracaoOpcaoAreaEstágio_,
            DescricaoDocumento = "Preencher a Declaração de Opção pela Área do Estágio, marcando no quadro a área de estágio correspondente ao que o (a) aluno (a) irá desenvolver na empresa e, posteriormente, entregar apenas 01 via, no Núcleo de Estágio Supervisionado da FACISABH, para abertura e registro do processo de estágio.",
            PosicaoDocumento = 1,
            Assinatura = EnumAssinaturaDocumento.Aluno,
            Aviso = "",
            Tipo = EnumTipoDocumento.Preenchimento,
            Texto =
            "Aluno (a): "+NomeAluno_+" RA: "+RegistroAcademico_+ Environment.NewLine +
            "Período: "+PeriodoCurso_+" Turno: "+TurnoCurso_+"" + Environment.NewLine +
            "Telefone Fixo: "+TelefoneAluno_+" Telefone Celular: "+CelularAluno_+"" + Environment.NewLine +
            "E -mail: "+EmailAluno_+"" + Environment.NewLine +

            "Empresa: "+NomeEmpresa_+"" + Environment.NewLine + Environment.NewLine + Environment.NewLine +

            "CARGA HORÁRIA OBRIGATÓRIA DE ESTÁGIO – "+CargaHorariaMinima_+" HORAS" + Environment.NewLine +
            ""+TabelaAreasEstagio_+"",
            Etapa = EnumEtapaDocumento.Inicial,
            Visibilidade = EnumVisibilidade.Publico,
            PossuiAssinaturaResposavelEstagio = true,
            PossuiCarimboCNPJ = true,
            PossuiData = true,
            IdCursoNavigation = curso
        };
        
        public static Documento AutorizacaoRealizacaoEstagio(Curso curso) => new Documento()
        {
            TituloDocumento = AutorizacaoRealizacaoEstagio_,
            DescricaoDocumento = "Preencher a Autorização para a Realização do Estágio (apenas uma via) contendo o nome do (a) aluno (a), duração do estágio (caso não tenha a data exata, poderá ser uma aproximada), carga horária total do estágio, nome do (a) administrador (a) responsável e formação acadêmica de quem acompanhará o estagiário na empresa. Essa autorização deve ser assinada pela empresa contendo ainda o carimbo de CNPJ e deve ser entregue na secretaria acadêmica – FACISABH para arquivo no processo de estágio.",
            PosicaoDocumento = 2,
            Assinatura = EnumAssinaturaDocumento.Empresa,
            Aviso = "",
            Tipo = EnumTipoDocumento.Preenchimento,
            Texto =
            "Declaramos, para fins de estágio supervisionado, que "+NomeAluno_+", está autorizado (a) a " +
            "realizar seu estágio nesta instituição, no período de "+DataInicialEstagio_+" a"+
            ""+DataFinalEstagio_+", cumprindo um total de "+TotalCargaHorariaEstagio_+" horas." + Environment.NewLine +
            "" + Environment.NewLine +
            "Informamos, ainda, que o responsável pelo acompanhamento na instituição será o (a) Sr. (a) "+NomeSupervisor_+", " +
            "com formação acadêmica em "+FormacaoSupervisor_+". "  +
            "Número do RG do tutor "+RGSupervisor_+"." + Environment.NewLine,
            Etapa = EnumEtapaDocumento.Inicial,
            Visibilidade = EnumVisibilidade.Publico,
            PossuiAssinaturaResposavelEstagio = true,
            PossuiCarimboCNPJ = true,
            PossuiData = true,
            IdCursoNavigation = curso
        };
        
        public static Documento AutorizacaoRealizacaoEstagioADM(Curso curso) => new Documento()
        {
            TituloDocumento = AutorizacaoRealizacaoEstagioADM_,
            DescricaoDocumento = "Preencher a Autorização para a Realização do Estágio (apenas uma via) contendo o nome do (a) aluno (a), duração do estágio (caso não tenha a data exata, poderá ser uma aproximada), carga horária total do estágio, nome do (a) administrador (a) responsável e formação acadêmica de quem acompanhará o estagiário na empresa. Essa autorização deve ser assinada pela empresa contendo ainda o carimbo de CNPJ e deve ser entregue na secretaria acadêmica – FACISABH para arquivo no processo de estágio.",
            PosicaoDocumento = 2,
            Assinatura = EnumAssinaturaDocumento.Empresa,
            Aviso = "",
            Tipo = EnumTipoDocumento.Preenchimento,
            Texto = "Aluno (a): "+NomeAluno_+" RA: "+RegistroAcademico_ + Environment.NewLine +
            "Período: "+PeriodoCurso_+"Turno: "+TurnoCurso_+"" + Environment.NewLine +
            "Telefone Fixo: "+TelefoneAluno_+" Telefone Celular: "+CelularAluno_+"" + Environment.NewLine +
            "E -mail: "+EmailAluno_+"" + Environment.NewLine +
            "Supervisor: "+NomeSupervisor_+ " CRA: " + CRASupervisor_ + Environment.NewLine +
            "Área de estágio: "+AreasEstagio_+"" + Environment.NewLine +
            "Empresa: "+NomeEmpresa_+" Periodo: "+DataInicialEstagio_+" a "+DataFinalEstagio_+"" + Environment.NewLine + Environment.NewLine + Environment.NewLine + "Declaramos, junto a Faculdade de Ciências Sociais Aplicadas de Belo Horizonte(FACISABH)," + Environment.NewLine +
            "que o(a) aluno(a) acima identificado(a), está autorizado a realizar seu estágio nesta Instituição no período acima identificado,"+ 
            "para cumprir as "+TotalCargaHorariaEstagio_+" horas do seu estágio supervisionado obrigatório." + Environment.NewLine +
            "Declaramos ainda que o estágio será supervisionado por um Administrador(bacharel) devidamente registrado e regular junto ao Conselho Regional de Aministração(CRA) e ainda, "+
            "que o mesmo será desenvolvido dentro da área acima identificada."+ Environment.NewLine +
            "O presente estágio objetiva proporcionar ao estagiário treinamento prático, "+ 
            "aperfeiçoamento de caráter técnico - cultural - social e de relacionamento humano, “visando ao aprendizado de competências próprias da atividade profissional e à contextualização curricular, "+
            "objetivando o desenvolvimento do educando para a vida cidadã e para o trabalho."+ Environment.NewLine +
            "A jornada de trabalho será por tarefas determinadas e planejadas, não ultrapassando 6 horas diárias ou 30 horas semanais,"+
            "conforme artigo 10, inciso II da lei 11.788 / 08 de 25 de setembro de 2008."+ Environment.NewLine +
            "O estagiário compromete - se a prestar os serviços que a empresa exigir desde que relacionados com a profissão adquirente e acompanhada pela Supervisão, "+
            "no local do estágio, conforme o plano de trabalho."+ Environment.NewLine +
            "O presente Termo de Compromisso / Autorização poderá ser rescindido de comum acordo conforme necessidades das partes mediante comunicação escrita.",
            Etapa = EnumEtapaDocumento.Inicial,     
            Visibilidade = EnumVisibilidade.Publico,
            PossuiAssinaturaResposavelEstagio = true,
            PossuiCarimboCNPJ = true,
            PossuiData = true,
            IdCursoNavigation = curso
        };
        
        public static Documento AutorizacaoRealizacaoEstagioCC(Curso curso) => new Documento()
        {
            TituloDocumento = AutorizacaoRealizacaoEstagioCC_,
            DescricaoDocumento = "Preencher a Autorização para a Realização do Estágio (apenas uma via) contendo o nome do (a) aluno (a), duração do estágio (caso não tenha a data exata, poderá ser uma aproximada), carga horária total do estágio, nome do (a) administrador (a) responsável e formação acadêmica de quem acompanhará o estagiário na empresa. Essa autorização deve ser assinada pela empresa contendo ainda o carimbo de CNPJ e deve ser entregue na secretaria acadêmica – FACISABH para arquivo no processo de estágio.",
            PosicaoDocumento = 2,
            Assinatura = EnumAssinaturaDocumento.Empresa,
            Aviso = "",
            Tipo = EnumTipoDocumento.Preenchimento,
            Texto = "Aluno (a): "+NomeAluno_+" RA: "+RegistroAcademico_ + Environment.NewLine +
            "Período: "+PeriodoCurso_+" - Turno: "+TurnoCurso_+"" + Environment.NewLine +
            "Telefone Fixo: "+TelefoneAluno_+" Telefone Celular: "+CelularAluno_+"" + Environment.NewLine +
            "E -mail: "+EmailAluno_+"" + Environment.NewLine +
            "Supervisor: "+NomeSupervisor_+"- CRC: " + CRCSupervisor_ + Environment.NewLine +
            "Área de estágio: "+AreasEstagio_+"" + Environment.NewLine +
            "Empresa: "+NomeEmpresa_+" - Periodo: "+DataInicialEstagio_+" a "+DataFinalEstagio_+"" + Environment.NewLine + Environment.NewLine + Environment.NewLine +
            "Declaramos, junto a Faculdade de Ciências Sociais Aplicadas de Belo Horizonte(FACISABH), " + 
            "que o(a) aluno(a) acima identificado(a), "+ 
            "está autorizado a realizar seu estágio nesta Instituição no período acima identificado, "+
            "para cumprir as "+TotalCargaHorariaEstagio_+" horas do seu estágio supervisionado obrigatório." + 
            "Declaramos ainda que o estágio será supervisionado por um contador (bacharel) devidamente registrado e regular junto ao Conselho Regional de Contabilidade (CRC) e ainda, " + 
            "que o mesmo será desenvolvido dentro da área acima identificada."+ Environment.NewLine +
            "O presente estágio objetiva proporcionar ao estagiário treinamento prático, "+
            "aperfeiçoamento de caráter técnico - cultural - social e de relacionamento humano, “visando ao aprendizado de competências próprias da atividade profissional e à contextualização curricular, "+ 
            "objetivando o desenvolvimento do educando para a vida cidadã e para o trabalho."+ Environment.NewLine +
            "A jornada de trabalho será por tarefas determinadas e planejadas, "+ 
            "não ultrapassando 6 horas diárias ou 30 horas semanais, conforme artigo 10,"+ 
            "inciso II da lei 11.788 / 08 de 25 de setembro de 2008."+ Environment.NewLine +
            "O estagiário compromete - se a prestar os serviços que a empresa exigir desde que relacionados com a profissão adquirente e acompanhada pela Supervisão, "+ 
            "no local do estágio, conforme o plano de trabalho."+ Environment.NewLine +
            "O presente Termo de Compromisso / Autorização poderá ser rescindido de comum acordo conforme necessidades das partes mediante comunicação escrita.",
            Etapa = EnumEtapaDocumento.Inicial,     
            Visibilidade = EnumVisibilidade.Publico,
            PossuiAssinaturaResposavelEstagio = true,
            PossuiCarimboCNPJ = true,
            PossuiData = true,
            IdCursoNavigation = curso
        };

        public static Documento AtestadoRealizacaoEstagio(Curso curso) => new Documento()
        {
            TituloDocumento = AtestadoRealizacaoEstagio_,
            DescricaoDocumento = "Ao final do processo de estágio supervisionado o (a) aluno (a) deverá apresentar ao coordenador do curso da FACISABH (apenas uma via) do Atestado de Realização de Estágio Supervisionado contendo nome do (a) aluno (a) e demais informações constantes no documento. Este documento deve ser assinado pela empresa e conter carimbo CNPJ.",
            PosicaoDocumento = 3,
            Assinatura = EnumAssinaturaDocumento.Empresa,
            Aviso = "",
            Tipo = EnumTipoDocumento.Preenchimento,
            Texto =
            "Atestamos para fins de conclusão do Estágio Supervisionado, que "+NomeAluno_+", " +
            "portador da Carteira de Identidade Nº."+RGAluno_+" e CPF "+CPFAluno_+", exerceu atividades nessa empresa na área de " + 
            ""+AreasEstagio_+", no período de "+DataInicialEstagio_+" a "+DataFinalEstagio_+", tendo cumprido um total de "+TotalCargaHorariaEstagio_+" horas." + 
            "" + Environment.NewLine +
            "Solicitamos que as atividades desempenhadas pelo (a) mesmo (a), que resultou no relatório final de estágio, por nós corroborado, tenha validade como " + 
            "estágio curricular obrigatório no curso de Análise e Desenvolvimento de Sistemas." + Environment.NewLine,
            Etapa = EnumEtapaDocumento.Final,
            Visibilidade = EnumVisibilidade.Publico,
            PossuiAssinaturaResposavelEstagio = true,
            PossuiCarimboCNPJ = true,
            PossuiData = true,
            IdCursoNavigation = curso
        };

        public static Documento SolicitacaoEstagioSupervisionado(Curso curso) => new Documento()
        {
            TituloDocumento = SolicitacaoEstagioSupervisionado_,
            DescricaoDocumento = "Preencher a carta de Solicitação de Estágio (em 02 vias) onde deverá conter o nome do (a) empresário (a) ou contador (a) e o nome da empresa. Essas duas vias deverão ser assinadas pelo coordenador do Núcleo de Estágio Supervisionado da FACISABH, para que o (a) aluno (a) posteriormente possa entregar na empresa onde realizará o processo de estágio. Após o protocolo de recebimento da mesma, uma das vias ficará na empresa e a outra deverá ser entregue para ser arquivada no Núcleo de Estágio da FACISABH. Esta carta é a apresentação do estagiário na instituição onde desenvolverá seus trabalhos e estudos.",
            PosicaoDocumento = 1,
            Assinatura = EnumAssinaturaDocumento.NDE,
            Aviso = "",
            Tipo = EnumTipoDocumento.Preenchimento,
            Texto =
            "Instituição: "+NomeEmpresa_+"" + Environment.NewLine +
            "Aos Cuidados (Orientador): "+NomeSupervisor_+"" + Environment.NewLine +
            "" + Environment.NewLine +
            "Ref: Solicitação de Estágio" + Environment.NewLine +
            "" + Environment.NewLine +
            "Solicitamos a V.Sª. a autorização para a realização de estágio dentro da área de "+EscolhaAreaEstagio_+", junto a essa conceituada " + 
            "instituição, do (a) aluno (a) abaixo identificado." + 
            "" + Environment.NewLine +
            "Esclarecemos que o referido estágio é uma exigência legal do Ministério da Educação, não caracterizando nenhuma responsabilidade de " + 
            "remuneração, vínculo empregatício, nem implicações previdenciárias." + 
            "" + Environment.NewLine +
            "Certos de vossa atenção, antecipo nossos agradecimentos e subscrevemo-nos." +
            "" + Environment.NewLine +
            "Atenciosamente," + Environment.NewLine + Environment.NewLine +
            "Aluno (a): " +NomeAluno_+" - RA: "+RegistroAcademico_ +Environment.NewLine +
            "Curso: " +CursoAluno_+" - Período: "+PeriodoCurso_+" - Turno: "+TurnoCurso_
            ,
            Etapa = EnumEtapaDocumento.Final,
            Visibilidade = EnumVisibilidade.Publico,
            PossuiAssinaturaResposavelEstagio = true,
            PossuiCarimboCNPJ = true,
            PossuiData = true,
            IdCursoNavigation = curso
        };
        
        public static Documento RegistroDeHoras(Curso curso) => new Documento()
        {
            TituloDocumento = "Tabela de Registro de Horas",
            DescricaoDocumento = "",
            PosicaoDocumento = 4,
            Assinatura = EnumAssinaturaDocumento.Multiplos,
            Aviso = "",
            Tipo = EnumTipoDocumento.Tabela,
            Texto = "",
            Etapa = EnumEtapaDocumento.Intermediaria,
            Visibilidade = EnumVisibilidade.Publico,
            PossuiAssinaturaResposavelEstagio = true,
            PossuiCarimboCNPJ = true,
            PossuiData = true,
            IdCursoNavigation = curso
        };

        public static Documento QuestionarioEstagio(Curso curso) => new Documento()
        {
            TituloDocumento = QuestionarioEstagio_,
            DescricaoDocumento = "",
            PosicaoDocumento = 5,
            Assinatura = EnumAssinaturaDocumento.Aluno,
            Aviso = "",
            Tipo = EnumTipoDocumento.Questionario,
            Texto = "",
            Etapa = EnumEtapaDocumento.Final,
            Visibilidade = EnumVisibilidade.Publico,
            PossuiAssinaturaResposavelEstagio = true,
            PossuiCarimboCNPJ = true,
            PossuiData = true,
            IdCursoNavigation = curso
        };
    }
}
