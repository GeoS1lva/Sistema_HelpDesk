using Sistema_HelpDesk.Desk.Application.CommomResult;

namespace Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies.Interface
{
    public interface IRemoverAtivarUsuarioEmpresaUseCase
    {
        public Task<ResultModel> RemoverUsuarioEmpresa(int usuarioEmpresaId);
        public Task<ResultModel> AtivarUsuarioEmpresa(int usuarioEmpresaId);
    }
}
