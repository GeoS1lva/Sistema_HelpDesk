namespace Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs
{
    public class UserTokenResetPassword
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
    }
}
