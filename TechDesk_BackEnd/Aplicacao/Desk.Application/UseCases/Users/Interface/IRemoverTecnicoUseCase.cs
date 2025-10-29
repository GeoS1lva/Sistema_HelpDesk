using Sistema_HelpDesk.Desk.Application.CommomResult;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Users.Interface
{
    public interface IRemoverTecnicoUseCase
    {
        public Task<ResultModel> RemoverTecnico(string userName, string userNameRequest);
    }
}
