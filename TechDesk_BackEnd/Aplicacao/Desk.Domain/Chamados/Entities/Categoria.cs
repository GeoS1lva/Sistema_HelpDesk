using Sistema_HelpDesk.Desk.Domain.Chamados.Entities;
using Sistema_HelpDesk.Desk.Domain.Chamados.Enums;
using Sistema_HelpDesk.Desk.Domain.Common;

namespace Sistema_HelpDesk.Desk.Domain.Chamados.Entidades
{
    public class Categoria(string nome) : Entidade
    {
        public string Nome { get; set; } = nome;
        public bool Status { get; set; } = true;

        public ICollection<SubCategoria> SubCategorias { get; set; } = [];

        public void Desativar()
            => Status = false;

        public void Ativar()
            => Status = true;
    }
}
