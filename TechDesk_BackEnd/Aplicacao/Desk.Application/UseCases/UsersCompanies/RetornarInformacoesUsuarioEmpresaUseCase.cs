using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies.Interface;
using Sistema_HelpDesk.Desk.Domain.Users.Enum;

namespace Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies
{
    public class RetornarInformacoesUsuarioEmpresaUseCase(IUnitOfWork unitOfWork) : IRetornarInformacoesUsuarioEmpresaUseCase
    {
        public async Task<ResultModel<UserEmpresaInformacoes>> RetornarInformacoesUsuarios(string userName)
        {
            var infoLogin = await unitOfWork.UserLoginRepository.RetornarLogin(userName);
            if (infoLogin == null)
                return ResultModel<UserEmpresaInformacoes>.Erro("Usuário não encontrado!");

            var infoUser = await unitOfWork.UsuarioEmpresaRepository.RetornarUsuario(infoLogin.Id);
            var infoEmpresa = await unitOfWork.EmpresaRepository.RetornarEmpresa(infoUser.EmpresaId);;

            return ResultModel<UserEmpresaInformacoes>.Sucesso(new UserEmpresaInformacoes
            {
                Nome = infoUser.Nome,
                UserName = infoLogin.UserName,
                Email = infoLogin.Email,
                EmpresaId = infoEmpresa.Id,
                EmpresaNome = infoEmpresa.Nome,
                Status = infoUser.Status
            });
        }
    }
}
