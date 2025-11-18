using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entities;
using Sistema_HelpDesk.Desk.Domain.Chamados.Enums;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Tickets.DTOs
{
    public class RetornarDetalhesChamadosUsuarioEmpresa
    {
        public string NumeroChamado { get; set; }
        public string CategoriaNome { get; set; }
        public string SubCategoriaNome { get; set; }
        public string MesaAtendimento { get; set; }
        public string TecnicoResponsavelNome { get; set; }
        public TimeSpan TempoAtendimentoTotal { get; set; }
        public StatusSla StatusSla { get; set; }
        public Status Status { get; set; }
    }
}
