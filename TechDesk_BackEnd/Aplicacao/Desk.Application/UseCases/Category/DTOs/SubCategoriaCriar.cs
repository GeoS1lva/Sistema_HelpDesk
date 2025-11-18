using Sistema_HelpDesk.Desk.Domain.Chamados.Enums;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Category.DTOs
{
    public class SubCategoriaCriar
    {
        public string Nome { get; set; }
        public Prioridade Prioridade { get; set; }
        public int SLA { get; set; }
        public int MesaAtendimentoId { get; set; }

    }
}
