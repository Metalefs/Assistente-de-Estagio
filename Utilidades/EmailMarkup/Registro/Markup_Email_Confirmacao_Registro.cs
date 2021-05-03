using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ADE.Dominio.Interfaces;

namespace ADE.Utilidades.EmailMarkup
{
    public class Markup_Email_Confirmacao_Registro : IEmailMarkup
    {
        public List<KeyValuePair<string, string>> Placeholders { get; private set; }
        private string EmailTemplatePath;
        private string EmailMarkup;

        public Markup_Email_Confirmacao_Registro(string _callback, string _EmailUsuario, string _EmailTemplatePath)
        {
            Placeholders = new List<KeyValuePair<string, string>>()
            {
               new KeyValuePair<string, string>("_ADE_Email_Placeholder_", _EmailUsuario),
               new KeyValuePair<string, string>("_ADE_ConfirmationCodePlaceholder_", _callback)
            };
            EmailTemplatePath = _EmailTemplatePath;
        }

        public async Task<string> Parse()
        {
            await GetEmailMarkup();
            return ReplaceMarkupPlaceholders();
        }

        private async Task GetEmailMarkup()
        {
            EmailMarkup = await File.ReadAllTextAsync(EmailTemplatePath);
        }

        private string ReplaceMarkupPlaceholders()
        {
            for (int i = 0; i < Placeholders.Count; i++)
            {
                EmailMarkup = EmailMarkup.Replace(Placeholders.ElementAt(i).Key, Placeholders.ElementAt(i).Value);
            }
            return EmailMarkup;
        }
    }
}
