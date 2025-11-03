using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Category.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Category.Interface;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Category
{
    public class CriarCategoriaUseCase(IUnitOfWork unitOfWork) : ICriarCategoriaUseCase
    {
        public async Task<ResultModel> AdicionarCategoria(CategoriaCriar categoria)
        {
            if (await unitOfWork.CategoriaRepository.ConsultarCategoria(categoria.Nome))
                return ResultModel.Erro("Essa Categoria já existe!");

            unitOfWork.CategoriaRepository.AdicionarCategoria(new Categoria(categoria.Nome));

            await unitOfWork.SaveChangesAsync();

            return ResultModel.Sucesso();
        }
    }
}
