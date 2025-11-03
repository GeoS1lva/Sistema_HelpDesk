using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sistema_HelpDesk.Desk.Application.Contracts.Repositories;
using Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies.DTOs;
using Sistema_HelpDesk.Desk.Domain.Empresas.Entidades;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;
using Sistema_HelpDesk.Desk.Infra.Context;

namespace Sistema_HelpDesk.Desk.Infra.Persistence.Repositories
{
    public class UsuarioEmpresaRepository(SqlServerIdentityDbContext context, UserManager<UserLogin> userManager) : IUsuarioEmpresaRepository
    {
        private readonly SqlServerIdentityDbContext _context = context;
        private readonly UserManager<UserLogin> _userManager = userManager;

        public void AdicionarUsuarioEmpresa(UsuariosEmpresa user)
            => _context.UsuariosEmpresa.Add(user);

        public async Task<bool> RemoverUsuarioEmpresa(int id)
        {
            var user = await _context.UsuariosEmpresa.FirstOrDefaultAsync(x => x.Id == id);

            if (user is null)
                return false;

            user.Desativado();

            return true;
        }

        public async Task<bool> RemoverTodosUsuarioPorEmpresa(int empresaId)
        {
            try
            {
                var processo = await _context.Database.ExecuteSqlRawAsync("UPDATE dbo.UsuariosEmpresa SET Status = 0 WHERE EmpresaId = {0}", empresaId);

                return processo > 0;
            } 
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<UsuariosEmpresa?> RetornarUsuario(int id)
            => await _context.UsuariosEmpresa.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<UsuariosEmpresa>> RetornarUsuariosEmpresas()
            => await _context.UsuariosEmpresa.ToListAsync();

        public async Task<List<UsuariosEmpresa>> RetornarUsuariosPorEmpresa(int id)
            => await _context.UsuariosEmpresa.Where(x => x.EmpresaId == id).ToListAsync();
    }
}
