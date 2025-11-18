using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies.DTOs;

namespace Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies.Interface
{
    public interface IAtualizarInformacoesUsuarioEmpresaUseCase
    {
        public Task<ResultModel<UserEmpresaInformacoes>> AtualizarDadosUsuarioEmpresa(UserLoginAtualizar user);
    }
}
