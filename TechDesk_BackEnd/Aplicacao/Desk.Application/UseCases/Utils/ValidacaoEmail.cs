using System.Net.Mail;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Utils
{
    public static class ValidacaoEmail
    {
        public static bool ValidarEmail(string email)
        {
            try
            {
                _ = new MailAddress(email);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
