using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid . Helpers . Mail;
using Sistema_HelpDesk.Desk.Domain.Emaill;

namespace Sistema_HelpDesk.Desk.Infra.Email
{
    public class EmailSender(IOptions<AuthMessageSenderOptions> options, ILogger<EmailSender> logger) : IEmailSender
    {
        private readonly AuthMessageSenderOptions _options = options.Value;

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var chave = new SendGridClient(_options.SendGridKey);

            var fromEmail = _options.FromEmail?.Trim();
            var remetente = new EmailAddress(fromEmail, _options.FromName);

            var destinatario = new EmailAddress(email);
            var mensagem = MailHelper.CreateSingleEmail(remetente, destinatario, subject, plainTextContent: null, htmlContent: htmlMessage);
            Response result = await chave.SendEmailAsync(mensagem).ConfigureAwait(false);

            var status = (int)result.StatusCode;
            var body = result.Body.ReadAsStringAsync();
            var headers = result.Headers?.ToString();

            if(result.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                throw new InvalidOperationException(
                $"E-mail não enviado. Status: {(int)result.StatusCode} {result.StatusCode}. " +
                $"Body: {body.Result} | Headers: {headers}"
                );
            }
        }
    }
}
