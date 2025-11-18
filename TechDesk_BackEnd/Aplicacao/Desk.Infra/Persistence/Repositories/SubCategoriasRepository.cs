using Microsoft.EntityFrameworkCore;
using Sistema_HelpDesk.Desk.Application.Contracts.Repositories;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entities;
using Sistema_HelpDesk.Desk.Infra.Context;

namespace Sistema_HelpDesk.Desk.Infra.Persistence.Repositories
{
    public class SubCategoriasRepository(SqlServerIdentityDbContext context) : ISubCategoriasRepository
    {
        private readonly SqlServerIdentityDbContext _context = context;

        public void AdicionarSubCategoria(SubCategoria subCategoria)
            => _context.SubCategoria.Add(subCategoria);

        public async Task<SubCategoria?> RetornarSubCategoria(int? id)
            => await _context.SubCategoria.Include(x => x.MesaAtendimento).FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<SubCategoria>> RetornarSubCategoriaPorCategoria(int categoriaId)
            => await _context.SubCategoria.Where(x => x.CategoriaId == categoriaId).ToListAsync();
    }
}
