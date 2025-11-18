using Sistema_HelpDesk.Desk.Domain.Chamados.Enums;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Tickets.DTOs
{
    public class RetornarChamadoPausado
    {
        public string NumeroChamado { get; set; }

        public TimeSpan TotalTempoPercorrido { get; set; }
        public Status Status { get; set; }

        public string NomeTecnico { get; set; }
        public DateTimeOffset HoraInicio { get; set; }
        public DateTimeOffset HoraFim { get; set; }
        public TimeSpan TotalApontamentoTecnico { get; set; }
        public string Comentario { get; set; }
    }
}
