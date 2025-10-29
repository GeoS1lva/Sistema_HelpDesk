using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Users.Interface
{
    public interface ICriarTecnicoUseCase
    {
        public Task<ResultModel> CriarTecnico(UserCriar userTecnico);
    }
}
