using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;
using Sistema_HelpDesk.Desk.Domain.Chamados.Enums;
using Sistema_HelpDesk.Desk.Domain.Common;

namespace Sistema_HelpDesk.Desk.Domain.Chamados.Entities
{
    public class SubCategoria(string nome, Prioridade prioridade, int sLA, int categoriaId) : Entidade
    {
        public string Nome { get; set; } = nome;
        public Prioridade Prioridade { get; set; } = prioridade;
        public int SLA { get; set; } = sLA;

        public bool Status { get; set; } = true;

        public int CategoriaId { get; set; } = categoriaId;
        public Categoria Categoria { get; set; }

        public void Desativar()
           => Status = false;

        public void Ativar()
            => Status = true;
    }
}
