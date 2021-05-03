using ADE.Dominio.Models.Enums;
using ADE.Utilidades.Extensions;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace ADE.Utilidades.EmailMarkup
{
    public class TemplatePathHelper
    {
        private string wwwrootPath;
        private string CaminhoEmailTemplates = "EmailTemplates";
        public TemplatePathHelper(string environmentWwwrootPath)
        {
            wwwrootPath = environmentWwwrootPath;
        }

        public string GetEmailTemplatePath(EnumEmailTemplate Type) => Path.Combine(wwwrootPath, CaminhoEmailTemplates, $"{Type.GetDescription()}.html");

    }
}
