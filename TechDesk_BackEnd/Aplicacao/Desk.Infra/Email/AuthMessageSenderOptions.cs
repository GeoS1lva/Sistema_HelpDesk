namespace Sistema_HelpDesk.Desk.Infra.Email
{
    public class AuthMessageSenderOptions
    {
        public string? SendGridKey { get; set; }
        public string? FromEmail { get; set; }
        public string? FromName { get; set; }
    }
}
