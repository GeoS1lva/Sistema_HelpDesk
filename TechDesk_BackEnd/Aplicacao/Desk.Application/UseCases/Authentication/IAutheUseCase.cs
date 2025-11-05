using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.UseCases.Autenticação.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Autenticação
{
    public interface IAutheUseCase
    {
        public Task<ResultModel<string>> FazerLoginSistema(LoginUserAcessar dto);
        public Task<ResultModel<string>> FazerLoginPainel(LoginUserAcessar dto);
    }
}
