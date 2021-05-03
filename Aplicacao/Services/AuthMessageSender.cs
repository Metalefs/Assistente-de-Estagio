using Microsoft.Extensions.Options;
using System;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace ADE.Aplicacao.Services
{
    public class AuthMessageSender : IEmailSender
    {
        public AuthMessageSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var client = new SendGridClient(Options.SendGridKey);
                // Create a Web transport for sending email.

                var from = new EmailAddress("suporte@assistentedeestagio.com", "Equipe ADE");
                var to = new EmailAddress(email, email);
                var htmlContent = message;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);

                return client.SendEmailAsync(msg);

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
