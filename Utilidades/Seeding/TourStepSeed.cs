using ADE.Dominio.Models.Individuais;
using System.Collections.Generic;

namespace ADE.Utilidades.Seeding
{
    public static class TourStepSeed
    {
        public static List<List<TourStep>> TourBase => new List<List<TourStep>>()
        {
            UserHome(),
            RegistroDeHora(),
            MeusDados(),
            DocumentosModelo()
        };

        public static List<TourStep> UserHome() => new List<TourStep>()
        {
            new TourStep(1,"#material-hint","Material do curso", "Verifique os materiais do seu curso disponíveis para download aqui","Principal","UserHome","Index"),
            new TourStep(2,"#carga-horaria","Carga-Horaria total","Registre as atividades que você realiza no seu tempo de trabalho como num diário. <br> O tempo que você anotar será somado á carga-horária obrigatória do seu Curso, e no final você poderá exportar esse diário em forma de tabela.","Principal","UserHome","Estatisticas"),
            new TourStep(3,"#atividades-dia","Análise da sua atividade no site"," Aqui é possível verificar os dias em você foi ativo no site, atividades como Download e Criação de eventos vão somar nas atividades de cada dia.","Principal","UserHome","Estatisticas"),
            new TourStep(4,"#percentual-conclusao","Percentual de conclusão","Aqui você pode acompanhar o seu progresso na conclusão da jornada <br> O material que você preenche (via atividades e download) soma no percententual de conclusão.","Principal","UserHome","Estatisticas"),
        };
        public static List<TourStep> RegistroDeHora() => new List<TourStep>()
        {
            new TourStep(1,"#registro-hora-novo","Registrar Horas em Atividade", "Escreva as atividades que você realiza no seu ambiente profissional. <br> Com esse registro, você poderá exportar os seus dados através do botão 'Exportar'.","Principal","RegistroHoras","Index"),
            
        };
        public static List<TourStep> MeusDados() => new List<TourStep>()
        {
            new TourStep(1,"#DadosAluno","Informações do estágio", "As suas informações, contidas aqui serão utilizadas na geração de qualquer contrato de estágio que você precise <br> Acesse a página <a class='link' href='/Principal/ListagemDocumentos/VisualizacaoDocumentosCurso'>Documentos modelo</a> para obter esses materiais.","Principal","MeusDados","Index"),
            
        };
        public static List<TourStep> DocumentosModelo() => new List<TourStep>()
        {
            new TourStep(1,"#1-doc","Documento de estágio", "Cada cartão desse representa um documento a ser preenchido, eles estão ordenados por ordem de entrega. <br> Você pode clicar nele para abrir, ou somente imprimi-lo, se precisar","Principal","ListagemDocumentos","VisualizacaoDocumentosCurso"),
            
        };
    }
}
