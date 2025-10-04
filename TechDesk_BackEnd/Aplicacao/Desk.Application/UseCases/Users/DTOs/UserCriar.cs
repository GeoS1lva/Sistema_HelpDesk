using Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.DTOs;
using Sistema_HelpDesk.Desk.Domain.Mesa;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs
{
    public class UserCriar
    {
        public string Nome { get; set; }
        public List<MesaAtendimentoCriar> MesasAtendimento { get; set; }
    }
}
