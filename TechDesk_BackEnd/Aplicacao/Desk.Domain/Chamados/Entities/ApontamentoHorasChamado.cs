using Sistema_HelpDesk.Desk.Domain.Chamados.Enums;
using Sistema_HelpDesk.Desk.Domain.Common;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_HelpDesk.Desk.Domain.Chamados.Entidades
{
    public class ApontamentoHorasChamado(int chamadoId, int tecnicoId) : Entidade
    {
        public int ChamadoId { get; private set; } = chamadoId;
        public Chamado Chamado { get; set; }

        public DateTimeOffset HoraInicio { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset HoraFim { get; set; }
        public StatusApontamento Status { get; set; } = StatusApontamento.emAndamento;

        public long TempoGastoTicks { get; set; }

        [NotMapped]
        public TimeSpan TempoGasto => new TimeSpan(TempoGastoTicks);

        public string? Comentario { get; set; }

        public int TecnicoId { get; private set; } = tecnicoId;
        public UsuarioSistema Tecnico { get; set; }

        public void EncerrarApontamento(string comentario)
        {
            HoraFim = DateTimeOffset.UtcNow;
            Status = StatusApontamento.finalizado;
            Comentario = comentario;
        }

        public long CalcularTicksTotal()
        {
            TempoGastoTicks = (HoraFim - HoraInicio).Ticks;
            return TempoGastoTicks;
        }
    }
}
