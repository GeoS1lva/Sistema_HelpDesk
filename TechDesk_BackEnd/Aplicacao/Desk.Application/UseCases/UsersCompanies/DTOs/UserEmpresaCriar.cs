using Sistema_HelpDesk.Desk.Domain.Users.Enum;

namespace Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies.DTOs
{
    public class UserEmpresaCriar
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Nome { get; set; }
        public int EmpresaId { get; set; }
    }
}
