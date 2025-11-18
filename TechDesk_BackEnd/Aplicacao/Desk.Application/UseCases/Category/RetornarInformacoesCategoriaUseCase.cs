using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Category.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Category.Interface;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Category
{
    public class RetornarInformacoesCategoriaUseCase(IUnitOfWork unitOfWork) : IRetornarInformacoesCategoriaUseCase
    {
        public async Task<ResultModel<List<CategoriaInformacoesDTO>>> RetornarCategorias()
        {
            var result = await unitOfWork.CategoriaRepository.RetornarCategorias();

            if (result.Count == 0)
                return ResultModel<List<CategoriaInformacoesDTO>>.Erro("O Sistema não possui Categorias Cadastradas!");

            return ResultModel<List<CategoriaInformacoesDTO>>.Sucesso(result);
        }

        public async Task<ResultModel<CategoriaInformacoesDTO>> RetornarCategoria(int id)
        {
            var result = await unitOfWork.CategoriaRepository.RetornarCategoriaSubCategoria(id);

            if (result == null)
                return ResultModel<CategoriaInformacoesDTO>.Erro("O Sistema não possui essa Categoria Cadastrada!");

            return ResultModel<CategoriaInformacoesDTO>.Sucesso(result);
        }
    }
}
