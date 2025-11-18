using Sistema_HelpDesk.Desk.Domain.Chamados.Entities;

namespace Sistema_HelpDesk.Desk.Application.Contracts.Repositories
{
    public interface ISubCategoriasRepository
    {
        public void AdicionarSubCategoria(SubCategoria subCategoria);
        public Task<SubCategoria?> RetornarSubCategoria(int? id);
        public Task<List<SubCategoria>> RetornarSubCategoriaPorCategoria(int categoriaId);
    }
}
