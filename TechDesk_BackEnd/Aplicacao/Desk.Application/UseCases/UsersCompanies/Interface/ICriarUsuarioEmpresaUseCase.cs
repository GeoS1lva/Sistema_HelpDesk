using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies.DTOs;

namespace Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies.Interface
{
    public interface ICriarUsuarioEmpresaUseCase
    {
        public Task<ResultModel> CadastrarUsuarioEmpresa(UserEmpresaCriar userEmpresa);
    }
}
