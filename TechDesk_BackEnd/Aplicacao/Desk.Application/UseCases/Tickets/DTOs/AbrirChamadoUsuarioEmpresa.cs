namespace Sistema_HelpDesk.Desk.Application.UseCases.Tickets.DTOs
{
    public class AbrirChamadoUsuarioEmpresa
    {
        public string Assunto { get; set; }
        public string Descricao { get; set; }
        public int CategoriaId { get; set; }
        public int SubCategoriaId { get; set; }
    }
}
