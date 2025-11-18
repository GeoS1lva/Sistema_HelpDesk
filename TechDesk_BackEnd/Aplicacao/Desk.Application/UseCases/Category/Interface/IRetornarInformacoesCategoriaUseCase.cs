using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.UseCases.Category.DTOs;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Category.Interface
{
    public interface IRetornarInformacoesCategoriaUseCase
    {
        public Task<ResultModel<List<CategoriaInformacoesDTO>>> RetornarCategorias();
        public Task<ResultModel<CategoriaInformacoesDTO>> RetornarCategoria(int id);
    }
}
