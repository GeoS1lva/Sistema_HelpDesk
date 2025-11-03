using Sistema_HelpDesk.Desk.Application.CommomResult;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Users.Interface
{
    public interface IRemoverUsuarioSistemaUseCase
    {
        public Task<ResultModel> RemoverUsuario(string userName, string userNameRequest);
    }
}
