using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;
using Sistema_HelpDesk.Desk.Domain.Common;
using Sistema_HelpDesk.Desk.Domain.Mesa.Entities;

namespace Sistema_HelpDesk.Desk.Domain.Users.Entities
{
    public class Tecnico : Entidade
    {
        public string Nome { get; set; }
        public long HorasTotalAtendimento { get; set; }

        public List<MesaAtendimento> Mesas { get; } = [];
        public ICollection<Chamado> Chamados { get; set; }

        public bool Status { get; set; }

        public void Ativo()
            => Status = true;

        public void Desativado()
            => Status = false;

        public void AlterarNome(string nome)
            => Nome = nome;
    }
}
