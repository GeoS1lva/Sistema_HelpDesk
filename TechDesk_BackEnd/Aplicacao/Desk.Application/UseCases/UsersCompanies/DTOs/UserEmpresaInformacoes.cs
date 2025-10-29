namespace Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies.DTOs
{
    public class UserEmpresaInformacoes
    {
        public string Nome { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }

        public int EmpresaId { get; set; }
        public string EmpresaNome { get; set; }

        public bool Status { get; set; }
    }
}
