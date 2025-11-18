namespace Sistema_HelpDesk.Desk.Application.UseCases.Tickets.DTOs
{
    public class RetornarQuantidadeChamado
    {
        public int ChamadosAbertos { get; set; }
        public int ChamadosPausados { get; set; }
        public int ChamadosFinalizados { get; set; }
    }
}
