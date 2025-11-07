using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.UseCases.Category.DTOs;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Category.Interface
{
    public interface ICriarCategoriaUseCase
    {
        public Task<ResultModel> AdicionarCategoria(CategoriaCriar categoria);
        public Task<ResultModel> AdicionarSubCategorias(List<SubCategoriaCriar> subCategorias, int categoriaId);
    }
}
