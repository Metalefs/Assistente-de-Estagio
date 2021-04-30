using ADE.Dominio.Models.Individuais;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace ADE.Utilidades.Seeding
{
    public static class InstituicaoSeed
    {
        public static Instituicao Facisa(IHostingEnvironment env) {
            byte[] Logo;
            string ImagePath;
            if (env.IsDevelopment())
            {
                ImagePath = Path.Combine(env.WebRootPath.Replace("bin\\Debug\\netcoreapp3.1\\",""),"Images","FacisaLogo.jpg");
            }
            else
            {
                ImagePath = Path.Combine(env.WebRootPath,"Images","FacisaLogo.jpg");
            }
            using (Stream fs = new FileStream(ImagePath, FileMode.Open))
            {
                using(MemoryStream ms = new MemoryStream())
                {
                    fs.CopyTo(ms);
                    Logo = ms.ToArray();
                }
            }
            return new Instituicao
            ("FacisaBH",
            @"A Faculdade de Ciências Sociais Aplicadas de Belo Horizonte visa produzir, intermediar e disseminar conhecimento, capacitando desta forma nossos alunos a enfrentarem os diferentes desafios do mercado. Assim sendo, percebe-se que a FACISABH tem recebido o reconhecimento do meio acadêmico e empresarial, em decorrência da qualidade, seriedade e dinamismo dos seus cursos.
    Notoriamente sensível às dificuldades e oportunidades vivenciadas, acreditamos que a busca por novos conhecimentos e aprimoramento dos atuais, por meio de uma educação superior, são essenciais para exercício pleno de qualquer profissão.Desta forma, estruturamos os cursos tendo como objetivo lapidar nossos estudantes, para que estes tenham a correta percepção estratégica e crítica a qual desempenham ou que desempenharão perante a sociedade.
    Temos assim, a plena certeza que, o desenvolvimento intelectual e o aprendizado de ferramentas clássicas e modernas, são condições essenciais para uma jornada de sucesso.Cientes disto, Diretoria, aliado aos nossos Professores doutores, mestres e especialistas, estamos focados em lhe oferecer oportunidades para mudar sua vida.
    Para tanto, procuramos selecionar para nosso quadro estudantil, estudantes ousados, que valorizam a busca por desafios e novos conhecimentos, que possuam um espírito vencedor, que não tenham receio a mudanças, que almejam “degraus” cada vez mais altos na sua carreira, que sejam humildes para reconhecer a necessidade de se qualificarem e se aperfeiçoarem constantemente, sem deixar de ser firmes em suas convicções.",
            "Antônio Baião de Amorim",
            "Av. Antônio Carlos, 571 – Bairro: Lagoinha Belo Horizonte – MG – CEP: 31210 010",
            "9 8978 4750",
            "ouvidoria@facisa.com.br",
            "www.facisabh.com.br",
            "#ffc925",
            Logo
            );
        }
    }
}
