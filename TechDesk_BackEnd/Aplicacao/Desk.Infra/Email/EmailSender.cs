using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Net.Mail;

namespace Sistema_HelpDesk.Desk.Infra.Email
{
    public class EmailSender(IOptions<EmailConfiguracao> options, ILogger<EmailSender> logger) : IEmailSender
    {
        private readonly EmailConfiguracao _options = options.Value;
        private readonly ILogger<EmailSender> _logger = logger;

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_options.FromName, _options.FromEmail));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                await smtp.ConnectAsync(_options.SmtpServer, _options.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_options.UserName, _options.Password);
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar e-mail");
                throw;
            }
        }
    }
}
