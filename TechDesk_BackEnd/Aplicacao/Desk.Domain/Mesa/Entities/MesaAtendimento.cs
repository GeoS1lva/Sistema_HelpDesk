using Sistema_HelpDesk.Desk.Domain.Common;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;

namespace Sistema_HelpDesk.Desk.Domain.Mesa.Entities
{
    public class MesaAtendimento : Entidade
    {
        public string Nome { get; set; }
        public bool Status { get; set; }
        public List<Tecnico> Tecnicos { get; } = [];

        public void Ativo()
            => Status = true;

        public void Desativado()
            => Status = false;
    }
}
