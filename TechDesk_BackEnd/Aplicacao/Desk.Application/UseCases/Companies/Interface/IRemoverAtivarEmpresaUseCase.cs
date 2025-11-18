using Sistema_HelpDesk.Desk.Application.CommomResult;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Companies.Interface
{
    public interface IRemoverAtivarEmpresaUseCase
    {
        public Task<ResultModel> RemoverEmpresa(int empresaId);
        public Task<ResultModel> AtivarEmpresa(int empresaId);
    }
}
