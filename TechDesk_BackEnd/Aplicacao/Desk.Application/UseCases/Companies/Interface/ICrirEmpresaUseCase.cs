using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.UseCases.Companies.DTOs;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Companies.Interface
{
    public interface ICrirEmpresaUseCase
    {
        public Task<ResultModel> CadastrarEmpresa(EmpresaCriar empresa);
    }
}
