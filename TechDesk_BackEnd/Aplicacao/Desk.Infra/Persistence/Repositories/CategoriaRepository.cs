using Microsoft.EntityFrameworkCore;
using Sistema_HelpDesk.Desk.Application.Contracts.Repositories;
using Sistema_HelpDesk.Desk.Application.UseCases.Category.DTOs;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;
using Sistema_HelpDesk.Desk.Infra.Context;

namespace Sistema_HelpDesk.Desk.Infra.Persistence.Repositories
{
    public class CategoriaRepository(SqlServerIdentityDbContext context) : ICategoriaRepository
    {
        private readonly SqlServerIdentityDbContext _context = context;

        public void AdicionarCategoria(Categoria categoria)
            => _context.Categoria.Add(categoria);

        public async Task<List<CategoriaInformacoesDTO>?> RetornarCategorias()
            => await _context.Categoria
                             .Include(c => c.SubCategorias)
                             .Select(c => new CategoriaInformacoesDTO
                             {
                                 Id = c.Id,
                                 Nome = c.Nome,
                                 Status = c.Status,
                                 SubCategorias = c.SubCategorias.Select(s => new SubCategoriasInformacoesDTO
                                 {
                                     Id = s.Id,
                                     Nome = s.Nome,
                                     Prioridade = s.Prioridade,
                                     SLA = s.SLA,
                                     Status = s.Status
                                 }).ToList()
                             }).ToListAsync();

        public async Task<CategoriaInformacoesDTO?> RetornarCategoriaSubCategoria(int categoriaId)
             => await _context.Categoria
                             .Include(c => c.SubCategorias)
                             .Select(c => new CategoriaInformacoesDTO
                             {
                                 Id = c.Id,
                                 Nome = c.Nome,
                                 Status = c.Status,
                                 SubCategorias = c.SubCategorias.Select(s => new SubCategoriasInformacoesDTO
                                 {
                                     Id = s.Id,
                                     Nome = s.Nome,
                                     Prioridade = s.Prioridade,
                                     SLA = s.SLA,
                                     Status = s.Status
                                 }).ToList()
                             }).FirstOrDefaultAsync(x => x.Id == categoriaId);

        public async Task<Categoria?> RetornarCategoria(int? id)
            => await _context.Categoria.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Categoria?> RetornarCategoria(string nome)
            => await _context.Categoria.FirstOrDefaultAsync(x => x.Nome == nome);

        public async Task<bool> ConsultarCategoria(string nome)
            => await _context.Categoria.AnyAsync(x => x.Nome == nome);

    }
}
