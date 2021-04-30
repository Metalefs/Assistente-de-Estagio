using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ADE.Selenium.Helpers
{
    public static class Caminhos
    {
        public static string PastaDoExecutavel => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string StartUrl = "https://assistentedeestagio.com.br/";
        public static string RootTeste = @"C:\Users\User\Source\Repos\Assistente De Estágio4\ADE.MVC\wwwroot\Documents\Teste";
        public static string ArquivoDocXValido => $"{RootTeste}\\Valido.docx";
        public static string ArquivoDocXInvalido = $"{RootTeste}\\Invalido.docx";
    }
}
