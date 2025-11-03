using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Users.Interface
{
    public interface ICriarUsuarioSistemaUseCase
    {
        public Task<ResultModel> CriarUsuario(UserCriar usuario);
    }
}
