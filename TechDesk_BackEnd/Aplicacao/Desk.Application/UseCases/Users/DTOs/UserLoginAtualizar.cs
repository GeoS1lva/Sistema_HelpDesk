namespace Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs
{
    public class UserLoginAtualizar
    {
        public string AntigoUserName { get; set; }

        public string? Nome { get; set; }
        public string? NovoUserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
