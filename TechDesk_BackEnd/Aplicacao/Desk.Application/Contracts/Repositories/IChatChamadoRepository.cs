using Sistema_HelpDesk.Desk.Domain.Chat;

namespace Sistema_HelpDesk.Desk.Application.Contracts.Repositories
{
    public interface IChatChamadoRepository
    {
        public void SalvarMensagem(ChatChamado chatChamado);
        public Task<List<ChatChamado>> RetornarMensagensChat(string numeroChamado);
    }
}
