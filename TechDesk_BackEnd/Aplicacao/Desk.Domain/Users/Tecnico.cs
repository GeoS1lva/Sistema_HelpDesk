using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;
using Sistema_HelpDesk.Desk.Domain.Common;
using Sistema_HelpDesk.Desk.Domain.Mesa;

namespace Sistema_HelpDesk.Desk.Domain.Users
{
    public class Tecnico : Entidade
    {
        public string Nome { get; set; }
        public long HorasTotalAtendimento { get; set; }

        public UserLogin UserLogin { get; set; }
        public List<MesaAtendimento> Mesas { get; } = [];
        public ICollection<Chamado> Chamados { get; set; }
    }
}
