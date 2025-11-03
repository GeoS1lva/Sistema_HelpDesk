using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.Interface;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Users
{
    public class RemoverUsuarioSistemaUseCase(IUnitOfWork unitOfWork) : IRemoverUsuarioSistemaUseCase
    {
        public async Task<ResultModel> RemoverUsuario(string userName, string userNameRequest)
        {
            if (userName == userNameRequest)
                return ResultModel.Erro("Não é possível apagar o próprio perfil");

            var login = await unitOfWork.UserLoginRepository.RetornarLogin(userName);

            if (login == null)
                return ResultModel.Erro("Perfil não encontrado");

            await unitOfWork.UserLoginRepository.BloquearLogin(login);

            await unitOfWork.MesasAtendimentosRepository.RemoverRelacionamento(login.Id);

            var result = await unitOfWork.UsuarioEmpresaRepository.RemoverUsuarioEmpresa(login.Id);

            if (!result)
                return ResultModel.Erro("Não foi possivel concluir a operação!");

            await unitOfWork.SaveChangesAsync();

            return ResultModel.Sucesso();
        }
    }
}
