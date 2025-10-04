using Microsoft.EntityFrameworkCore;
using Sistema_HelpDesk.Desk.Application.Contracts.Repositories;
using Sistema_HelpDesk.Desk.Domain.Mesa.Entities;
using Sistema_HelpDesk.Desk.Infra.Context;

namespace Sistema_HelpDesk.Desk.Infra.Persistence.Repositories
{
    public class MesasAtendimentoRepository(SqlServerIdentityDbContext context) : IMesasAtendimentosRepository
    {
        private readonly SqlServerIdentityDbContext _context = context;

        public void AdicionarMesaAtendimento(MesaAtendimento mesa)
            => _context.MesasAtendimento.Add(mesa);

        public async Task<bool> RemoverMesaAtendimento(string nome)
        {
            var mesa = await _context.MesasAtendimento.FirstOrDefaultAsync(x => x.Nome == nome);

            if (mesa is null)
                return false;

            mesa.Desativado();

            return true;
        }

        public async Task<List<MesaAtendimento>> RetornarListaMesasAtendimento()
            => await _context.MesasAtendimento.ToListAsync();

        public async Task<List<MesaAtendimento>> RetornarListaMesasAtendimentoAtivas()
            => await _context.MesasAtendimento.Where(x => x.Status == true).ToListAsync();
    }
}
