using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies.DTOs;

namespace Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies.Interface
{
    public interface IRetornarInformacoesUsuarioEmpresaUseCase
    {
        public Task<ResultModel<UserEmpresaInformacoes>> RetornarInformacoesUsuarios(string userName);
    }
}
