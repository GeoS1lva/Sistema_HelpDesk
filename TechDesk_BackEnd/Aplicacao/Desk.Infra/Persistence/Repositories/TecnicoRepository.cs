using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using Sistema_HelpDesk.Desk.Application.Contracts.Repositories;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;
using Sistema_HelpDesk.Desk.Infra.Context;

namespace Sistema_HelpDesk.Desk.Infra.Persistence.Repositories
{
    public class TecnicoRepository(SqlServerIdentityDbContext context, UserManager<UserLogin> userManager) : ITecnicoRepository
    {
        private readonly SqlServerIdentityDbContext _context = context;
        private readonly UserManager<UserLogin> _userManager = userManager;

        public void AdicionarTecnico(Tecnico tecnico)
            => _context.Tecnicos.Add(tecnico);

        public async Task<bool> RemoverTecnico(int id)
        {
            var tecnico = await _context.Tecnicos.FirstOrDefaultAsync(x => x.Id == id);


            if (tecnico is null)
                return false;

            tecnico.Desativado();

            return true;
        }

        public async Task<Tecnico?> RetornarTecnico(int id)
            => await _context.Tecnicos.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<Tecnico>> RetornarTecnicos()
            => await _context.Tecnicos.ToListAsync();

        public async Task<List<Tecnico>> RetornarTecnicosAtivos()
            => await _context.Tecnicos.Where(x => x.Status == true).OrderBy(x => x.Nome).ToListAsync();
    }
}
