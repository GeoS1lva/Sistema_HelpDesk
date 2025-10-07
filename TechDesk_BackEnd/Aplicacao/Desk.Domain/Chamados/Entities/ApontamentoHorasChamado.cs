using Sistema_HelpDesk.Desk.Domain.Common;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;

namespace Sistema_HelpDesk.Desk.Domain.Chamados.Entidades
{
    public class ApontamentoHorasChamado : Entidade
    {
        public int ChamadoId { get; private set; }
        public Chamado Chamado { get; set; }

        public long Horas { get; set; }
        public DateTimeOffset Temporizador { get; set; }
        public DateTime HoraInicio { get; set; }
        public string Comentario { get; set; }

        public int TecnicoId { get; private set; }
        public Tecnico Tecnico { get; set; }

        public ApontamentoHorasChamado(int chamadoId, int tecnicoId)
        {
            ChamadoId = chamadoId;
            TecnicoId = tecnicoId;
            HoraInicio = DateTime.Now;
        }
    }
}
