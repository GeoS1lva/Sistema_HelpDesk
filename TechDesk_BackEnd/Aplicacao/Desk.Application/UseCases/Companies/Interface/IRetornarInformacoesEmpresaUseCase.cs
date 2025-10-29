using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.UseCases.Companies.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies.DTOs;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Companies.Interface
{
    public interface IRetornarInformacoesEmpresaUseCase
    {
        public Task<ResultModel<EmpresaInformacoes>> RetornarEmpresa(int id);
        public Task<ResultModel<List<EmpresaInformacoes>>> RetornarTodasEmpresas();
        public Task<ResultModel<List<UserEmpresaInformacoes>>> RetornarUsuariosEmpresas(int id);
    }
}
