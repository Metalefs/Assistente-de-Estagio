using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ADE.Dominio.Interfaces.Testes
{
    public abstract class ICadastro
    {
        public static readonly string EmailEmUso = "admin@assistentedeestagio.com";
        public static readonly string EmailInvalido = "EmailInvalido@com";
        public static readonly string SenhaInvalida = "123";
        public static readonly string EmailValido = "EmailValido@gmail.com";
        public static readonly string SenhaValida = "senhavalida123";

        public abstract void CadastroComEmailEmUso();
        public abstract void CadastroComEmailInvalido();
        public abstract Task CadastroComSenhaInvalida();
        public abstract void CadastroComOmissaoDeDados();
        public abstract void CadastroComDadosValidos();
    }
}
