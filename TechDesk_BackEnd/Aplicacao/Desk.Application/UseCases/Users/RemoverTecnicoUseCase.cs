using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.Interface;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Users
{
    public class RemoverTecnicoUseCase(IUnitOfWork unitOfWork) : IRemoverTecnicoUseCase
    {
        public async Task<ResultModel> RemoverTecnico(string userName, string userNameRequest)
        {
            if (userName == userNameRequest)
                return ResultModel.Erro("Não é possível apagar o próprio perfil");

            var tecnicoLogin = await unitOfWork.UserLoginRepository.RetornarLogin(userName);

            if (tecnicoLogin == null)
                return ResultModel.Erro("Perfil de Tecnico não encontrado");

            await unitOfWork.UserLoginRepository.BloquearLogin(tecnicoLogin);

            await unitOfWork.MesasAtendimentosRepository.RemoverRelacionamento(tecnicoLogin.Id);

            await unitOfWork.TecnicoRepository.RemoverTecnico(tecnicoLogin.Id);

            return ResultModel.Sucesso();
        }
    }
}
