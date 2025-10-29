namespace Sistema_HelpDesk.Desk.Application.UseCases.Companies.DTOs
{
    public class EmpresaInformacoes
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string Email { get; set; }
        public DateTime DataCriacao { get; set; }
        public bool Status { get; set; }
    }
}
