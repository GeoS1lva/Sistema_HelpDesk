using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using Sistema_HelpDesk.Desk.Application.Contracts.Repositories;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;
using Sistema_HelpDesk.Desk.Infra.Context;

namespace Sistema_HelpDesk.Desk.Infra.Persistence.Repositories
{
    public class UsuarioSistemaRepository(SqlServerIdentityDbContext context, UserManager<UserLogin> userManager) : IUsuarioSistemaRepository
    {
        private readonly SqlServerIdentityDbContext _context = context;

        public void AdicionarUsuario(UsuarioSistema usuario)
            => _context.Tecnicos.Add(usuario);

        public async Task<bool> RemoverUsuario(int id)
        {
            var usuario = await _context.Tecnicos.FirstOrDefaultAsync(x => x.Id == id);

            if (usuario is null)
                return false;

            usuario.Desativado();

            return true;
        }

        public async Task<UsuarioSistema?> RetornarUsuario(int id)
            => await _context.Tecnicos.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<UsuarioSistema>> RetornarUsuarios()
            => await _context.Tecnicos.ToListAsync();

        public async Task<List<UsuarioSistema>> RetornarUsuariosAtivos()
            => await _context.Tecnicos.Where(x => x.Status == true).OrderBy(x => x.Nome).ToListAsync();
    }
}
