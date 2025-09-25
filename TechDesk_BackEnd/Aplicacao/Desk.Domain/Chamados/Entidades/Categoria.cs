using Sistema_HelpDesk.Desk.Domain.Common;

namespace Sistema_HelpDesk.Desk.Domain.Chamados.Entidades
{
    public class Categoria(string nome) : Entidade
    {
        public string Nome { get; set; } = nome;
    }
}
