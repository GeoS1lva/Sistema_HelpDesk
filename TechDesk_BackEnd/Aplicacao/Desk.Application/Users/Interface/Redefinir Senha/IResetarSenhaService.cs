using Sistema_HelpDesk.Desk.Domain.Models;
using Sistema_HelpDesk.Desk.Domain.Users.ResetarSenhaUser;

namespace Sistema_HelpDesk.Desk.Application.Users.Interface
{
    public interface IResetarSenhaService
    {
        public Task<ResultModel> SolicitarRedefinicaoSenha(UserResetSenha userReset);
    }
}
