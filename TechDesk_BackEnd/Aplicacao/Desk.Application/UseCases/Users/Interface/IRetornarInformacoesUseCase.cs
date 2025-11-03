using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Users.Interface
{
    public interface IRetornarInformacoesUseCase
    {
        public Task<ResultModel<UserInformacoes>> RetornarInformacoesUsuario(string UserName);
        public Task<ResultModel<List<UserInformacoes>>> RetornarInformacoesUsuariosCriado();
    }
}
