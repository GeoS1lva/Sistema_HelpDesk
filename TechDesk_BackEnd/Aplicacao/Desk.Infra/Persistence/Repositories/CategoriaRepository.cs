using Microsoft.EntityFrameworkCore;
using Sistema_HelpDesk.Desk.Application.Contracts.Repositories;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;
using Sistema_HelpDesk.Desk.Infra.Context;

namespace Sistema_HelpDesk.Desk.Infra.Persistence.Repositories
{
    public class CategoriaRepository(SqlServerIdentityDbContext context) : ICategoriaRepository
    {
        private readonly SqlServerIdentityDbContext _context = context;

        public void AdicionarCategoria(Categoria categoria)
            => _context.Categoria.Add(categoria);

        public async Task<bool> DesativarCategoria(int id)
        {
            var categoria = await _context.Categoria.FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null)
                return false;

            categoria.Desativado();
            return true;
        }

        public async Task<bool> AtivarCategoria(int id)
        {
            var categoria = await _context.Categoria.FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null)
                return false;

            categoria.Ativo();
            return true;
        }

        public async Task<List<Categoria>?> RetornarCategorias()
            => await _context.Categoria.ToListAsync();

        public async Task<Categoria?> RetonarCategoria(int id)
            => await _context.Categoria.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<bool> ConsultarCategoria(string nome)
            => await _context.Categoria.AnyAsync(x => x.Nome == nome);

    }
}
