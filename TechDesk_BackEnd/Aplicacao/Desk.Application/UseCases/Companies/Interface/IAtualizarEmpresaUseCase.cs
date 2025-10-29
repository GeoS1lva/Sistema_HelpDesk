using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.UseCases.Companies.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Empresas.DTOs;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Companies.Interface
{
    public interface IAtualizarEmpresaUseCase
    {
        public Task<ResultModel> AtualizarNomeEmpresa(int empresaId, EmpresaAtualizarNome novoNome);
        public Task<ResultModel> AtualizarEmailEmpresa(int empresaId, EmpresaAtualizarEmail novoEmail);
    }
}
