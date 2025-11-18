using Sistema_HelpDesk.Desk.Domain.Chamados.Enums;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Tickets.DTOs
{
    public class InformacoesChamadoKanban
    {
        public string NumeroChamado { get; set; }
        public string Assunto { get; set; }
        public Prioridade Prioridade { get; set; }
        public StatusSla SLA { get; set; }
        public Status Status { get; set; }
        public string NomeEmpresa { get; set; }
        public string NomeMesaAtendimento { get; set; }
        public string NomeTecnicoResponsavel { get; set; }
    }
}
