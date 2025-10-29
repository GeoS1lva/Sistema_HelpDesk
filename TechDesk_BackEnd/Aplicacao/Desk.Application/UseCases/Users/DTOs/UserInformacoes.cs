namespace Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs
{
    public class UserInformacoes
    {
        public string? Nome { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public IList<string> Role { get; set; }
        public List<string>? Mesas { get; set; }
        public bool Status { get; set; }
    }
}
