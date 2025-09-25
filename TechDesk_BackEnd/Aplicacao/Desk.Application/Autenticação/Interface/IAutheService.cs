using Sistema_HelpDesk.Desk.Domain.Models.DTOs.Autenticação;
using Sistema_HelpDesk.Desk.Domain.Models;

namespace Sistema_HelpDesk.Desk.Application.Autenticação.Interface
{
    public interface IAutheService
    {
        public Task<ResultModel> CriarLogin(CadastrarUserDto dto);
        public Task<string> FazerLogin(LoginUserDto dto);
    }
}
