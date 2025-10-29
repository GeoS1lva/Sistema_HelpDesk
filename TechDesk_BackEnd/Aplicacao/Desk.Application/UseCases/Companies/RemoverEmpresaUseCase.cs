using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Companies.Interface;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Companies
{
    public class RemoverEmpresaUseCase(IUnitOfWork unitOfWork) : IRemoverEmpresaUseCase
    {
        //public async Task<ResultModel> RemoverEmpresa(int empresaId)
        //{
        //    var empresa = await unitOfWork.EmpresaRepository.RetornarEmpresa(empresaId);

        //    if (empresa == null)
        //        return ResultModel.Erro("Empresa não encontrada");
        //}
    }
}
