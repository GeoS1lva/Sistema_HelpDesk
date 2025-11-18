using Microsoft.EntityFrameworkCore;
using Sistema_HelpDesk.Desk.Application.Contracts.Repositories;
using Sistema_HelpDesk.Desk.Domain.Chat;
using Sistema_HelpDesk.Desk.Infra.Context;

namespace Sistema_HelpDesk.Desk.Infra.Persistence.Repositories
{
    public class ChatChamadoRepository(SqlServerIdentityDbContext context) : IChatChamadoRepository
    {
        private readonly SqlServerIdentityDbContext _context = context;

        public void SalvarMensagem(ChatChamado chatChamado)
            => _context.Chat.Add(chatChamado);

        public async Task<List<ChatChamado>> RetornarMensagensChat(string numeroChamado)
            => await _context.Chat.Where(x => x.NumeroChamado == numeroChamado).ToListAsync();
    }
}
