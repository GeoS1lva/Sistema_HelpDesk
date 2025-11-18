using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Companies.Interface;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Companies
{
    public class RemoverAtivarEmpresaUseCase(IUnitOfWork unitOfWork) : IRemoverAtivarEmpresaUseCase
    {
        public async Task<ResultModel> RemoverEmpresa(int empresaId)
        {
            var empresa = await unitOfWork.EmpresaRepository.RetornarEmpresa(empresaId);

            if (empresa == null)
                return ResultModel.Erro("Empresa não encontrada");

            var usuariosEmpresa = await unitOfWork.UsuarioEmpresaRepository.RetornarUsuariosPorEmpresa(empresa.Id);
            
            if(usuariosEmpresa.Count == 0)
            {
                empresa.Desativado();
                await unitOfWork.SaveChangesAsync();

                return ResultModel.Sucesso();
            }

            empresa.Desativado();
            foreach(var user in usuariosEmpresa)
            {
                user.Desativado();
                var userLogin = await unitOfWork.UserLoginRepository.RetornarLogin(user.Id);
                await unitOfWork.UserLoginRepository.BloquearLogin(userLogin);
            }

            await unitOfWork.SaveChangesAsync();
            return ResultModel.Sucesso();
        }

        public async Task<ResultModel> AtivarEmpresa(int empresaId)
        {
            var empresa = await unitOfWork.EmpresaRepository.RetornarEmpresa(empresaId);

            if (empresa == null)
                return ResultModel.Erro("Empresa não encontrada");

            var usuariosEmpresa = await unitOfWork.UsuarioEmpresaRepository.RetornarUsuariosPorEmpresa(empresa.Id);

            if (usuariosEmpresa.Count == 0)
            {
                empresa.Ativo();
                await unitOfWork.SaveChangesAsync();

                return ResultModel.Sucesso();
            }

            empresa.Ativo();
            foreach (var user in usuariosEmpresa)
            {
                user.Ativo();
                var userLogin = await unitOfWork.UserLoginRepository.RetornarLogin(user.Id);
                await unitOfWork.UserLoginRepository.DesbloquearLogin(userLogin);
            }

            await unitOfWork.SaveChangesAsync();
            return ResultModel.Sucesso();
        }
    }
}
