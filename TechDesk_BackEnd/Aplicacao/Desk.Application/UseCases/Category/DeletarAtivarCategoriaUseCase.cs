using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Category.Interface;
using Sistema_HelpDesk.Desk.Infra.Persistence.UnitOfWork;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Category
{
    public class DeletarAtivarCategoriaUseCase(IUnitOfWork unitOfWork) : IDeletarAtivarCategoriaUseCase
    {
        public async Task<ResultModel> DesativarCategoria(int id)
        {
            var result = await unitOfWork.CategoriaRepository.DesativarCategoria(id);

            if (!result)
                return ResultModel.Erro("Não foi possível completar a operação!");

            await unitOfWork.SaveChangesAsync();

            return ResultModel.Sucesso();
        }

        public async Task<ResultModel> AtivarCategoria(int id)
        {
            var result = await unitOfWork.CategoriaRepository.AtivarCategoria(id);

            if (!result)
                return ResultModel.Erro("Não foi possível completar a operação!");

            await unitOfWork.SaveChangesAsync();

            return ResultModel.Sucesso();
        }
    }
}
