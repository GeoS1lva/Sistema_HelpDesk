using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies.Interface;

namespace Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies
{
    public class RemoverAtivarUsuarioEmpresaUseCase(IUnitOfWork unitOfWork) : IRemoverAtivarUsuarioEmpresaUseCase
    {
        public async Task<ResultModel> RemoverUsuarioEmpresa(int usuarioEmpresaId)
        {
            var user = await unitOfWork.UsuarioEmpresaRepository.RetornarUsuario(usuarioEmpresaId);

            if (user == null)
                return ResultModel.Erro("Usuario não encontrado!");

            if (user.Status == false)
                return ResultModel.Erro("Esse Usuário já está desativado!");

            var userLogin = await unitOfWork.UserLoginRepository.RetornarLogin(user.Id);

            user.Desativado();
            await unitOfWork.UserLoginRepository.BloquearLogin(userLogin);

            await unitOfWork.SaveChangesAsync();
            return ResultModel.Sucesso();
        }

        public async Task<ResultModel> AtivarUsuarioEmpresa(int usuarioEmpresaId)
        {
            var user = await unitOfWork.UsuarioEmpresaRepository.RetornarUsuario(usuarioEmpresaId);

            if (user == null)
                return ResultModel.Erro("Usuario não encontrado!");

            if (user.Status == true)
                return ResultModel.Erro("Esse Usuário já está ativo!");

            var userLogin = await unitOfWork.UserLoginRepository.RetornarLogin(user.Id);

            user.Ativo();
            await unitOfWork.UserLoginRepository.DesbloquearLogin(userLogin);

            await unitOfWork.SaveChangesAsync();
            return ResultModel.Sucesso();
        }
    }
}
