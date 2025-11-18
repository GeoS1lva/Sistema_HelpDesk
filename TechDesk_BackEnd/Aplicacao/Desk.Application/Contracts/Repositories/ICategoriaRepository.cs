using Sistema_HelpDesk.Desk.Application.UseCases.Category.DTOs;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;

namespace Sistema_HelpDesk.Desk.Application.Contracts.Repositories
{
    public interface ICategoriaRepository
    {
        public void AdicionarCategoria(Categoria categoria);
        public Task<List<CategoriaInformacoesDTO>?> RetornarCategorias();
        public Task<CategoriaInformacoesDTO?> RetornarCategoriaSubCategoria(int id);
        public Task<Categoria?> RetornarCategoria(int? id);
        public Task<bool> ConsultarCategoria(string nome);
    }
}
