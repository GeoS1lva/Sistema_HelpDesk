namespace Sistema_HelpDesk.Desk.Application.UseCases.Category.DTOs
{
    public class CategoriaInformacoesDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool Status { get; set; }
        public List<SubCategoriasInformacoesDTO> SubCategorias { get; set; } = [];
    }
}
