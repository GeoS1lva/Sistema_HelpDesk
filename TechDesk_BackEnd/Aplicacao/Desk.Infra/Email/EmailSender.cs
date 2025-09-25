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
        private readonly ILogger<EmailSender> _logger = logger;

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            if(string.IsNullOrWhiteSpace(_options.SendGridKey))
                throw new InvalidOperationException("SendGridKey vazio nas configurações.");
            if(string.IsNullOrWhiteSpace(_options.FromEmail))
                throw new InvalidOperationException("FromEmail vazio nas configurações.");

            var chave = new SendGridClient(_options.SendGridKey);
            var remetente = new EmailAddress(_options.FromEmail, _options.FromName);

            var destinatario = new EmailAddress(email);
            var mensagem = MailHelper.CreateSingleEmail(remetente, destinatario, subject, plainTextContent: null, htmlContent: htmlMessage);
            var result = await chave.SendEmailAsync(mensagem);

            var body = await result.Body.ReadAsStringAsync();
            _logger.LogInformation("SendGrid status {Status} body: {Body}", result.StatusCode, body);

            if(result.StatusCode!=)
            {
                throw new InvalidOperationException($"Erro ao enviar e-mail: {result.StatusCode} - {body}");
            }
        }
    }
}
