using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Users.ResetPasswords
{
    public interface IResetarSenhaUseCase
    {
        public Task<ResultModel> SolicitarRedefinicaoSenha(UserResetPasswords userReset);
    }
}
