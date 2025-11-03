using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Category.Interface;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Category
{
    public class RetornarInformacoesCategoriaUseCase(IUnitOfWork unitOfWork) : IRetornarInformacoesCategoriaUseCase
    {
        public async Task<ResultModel<List<Categoria>>> RetornarCategorias()
        {
            var result = await unitOfWork.CategoriaRepository.RetornarCategorias();

            if (result == null)
                return ResultModel<List<Categoria>>.Erro("O Sistema não possui Categorias Cadastradas!");

            return ResultModel<List<Categoria>>.Sucesso(result);
        }

        public async Task<ResultModel<Categoria>> RetornarCategoria(int id)
        {
            var result = await unitOfWork.CategoriaRepository.RetonarCategoria(id);

            if (result == null)
                return ResultModel<Categoria>.Erro("O Sistema não possui essa Categoria Cadastrada!");

            return ResultModel<Categoria>.Sucesso(result);
        }
    }
}
