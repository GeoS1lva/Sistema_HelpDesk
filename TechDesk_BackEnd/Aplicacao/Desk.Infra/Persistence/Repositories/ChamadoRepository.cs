using Sistema_HelpDesk.Desk.Application.Contracts.Repositories;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;
using Sistema_HelpDesk.Desk.Infra.Context;

namespace Sistema_HelpDesk.Desk.Infra.Persistence.Repositories
{
    public class ChamadoRepository(SqlServerIdentityDbContext context) : IChamadoRepository
    {
        private readonly SqlServerIdentityDbContext _context = context;

        public void AdicionarChamado(Chamado chamado)
            => _context.Chamados.Add(chamado);
    }
}
