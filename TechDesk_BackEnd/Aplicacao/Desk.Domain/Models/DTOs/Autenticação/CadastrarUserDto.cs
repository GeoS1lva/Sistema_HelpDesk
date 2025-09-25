namespace Sistema_HelpDesk.Desk.Domain.Models.DTOs.Autenticação
{
    public class CadastrarUserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
