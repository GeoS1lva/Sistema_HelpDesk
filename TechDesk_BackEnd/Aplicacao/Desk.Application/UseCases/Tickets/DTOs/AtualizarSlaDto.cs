using Sistema_HelpDesk.Desk.Domain.Chamados.Enums;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Tickets.DTOs
{
    public class AtualizarSlaDto
    {
        public string NumeroChamado { get; set; }
        public StatusSla StatusSla { get; set; }
    }
}
