using Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.DTOs;
using Sistema_HelpDesk.Desk.Domain.Users.Enum;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs
{
    public class UserCriar
    {
        public string Nome { get; set; }
        public List<int> MesasAtendimentoId { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        public TipoPerfil TipoPerfil { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
