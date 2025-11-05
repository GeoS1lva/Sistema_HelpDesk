using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Category.Interface
{
    public interface IRetornarInformacoesCategoriaUseCase
    {
        public Task<ResultModel<List<Categoria>>> RetornarCategorias();
        public Task<ResultModel<Categoria>> RetornarCategoria(int id);
    }
}
