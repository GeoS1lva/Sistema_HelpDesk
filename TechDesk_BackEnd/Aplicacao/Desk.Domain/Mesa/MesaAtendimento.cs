using Sistema_HelpDesk.Desk.Domain.Common;
using Sistema_HelpDesk.Desk.Domain.Users;

namespace Sistema_HelpDesk.Desk.Domain.Mesa
{
    public class MesaAtendimento : Entidade
    {
        public string Nome { get; set; }
        public bool Status { get; set; }
        public List<Tecnico> Tecnicos { get; } = [];
    }
}
