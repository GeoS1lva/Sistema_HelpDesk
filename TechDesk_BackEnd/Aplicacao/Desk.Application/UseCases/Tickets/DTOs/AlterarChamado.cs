namespace Sistema_HelpDesk.Desk.Application.UseCases.Tickets.DTOs
{
    public class AlterarChamado
    {
        public string NumeroChamado { get; set; }
        public int? CategoriaId { get; set; }
        public int? SubCategoriaId { get; set; }
        public int? MesaAtendimentoId { get; set; }
        public string? TecnicoResponsavelUserName { get; set; }
    }
}
