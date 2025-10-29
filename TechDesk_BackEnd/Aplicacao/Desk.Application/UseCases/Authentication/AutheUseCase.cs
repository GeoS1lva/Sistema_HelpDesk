using Microsoft.AspNetCore.Identity;
using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.Security;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Autenticação.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;
using Sistema_HelpDesk.Desk.Domain.Users.Enum;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Autenticação
{
    public class AutheUseCase(IUnitOfWork unitOfWork, IJwtGerador jwt) : IAutheUseCase
    {
        /*public async Task<ResultModel> CriarLogin(UserLoginCriar dto)
        {
            var novoUser = new UserLogin { UserName = dto.UserName, Email = dto.Email };
            var result = await User.CreateAsync(novoUser, dto.Password);
            await User.AddToRoleAsync(novoUser, dto.Role);

            if (!result.Succeeded)
                return ResultModel.Erro("Usuário não Cadastrado!");

            return ResultModel.Sucesso();
        }*/

        public async Task<ResultModel<string>> FazerLogin(LoginUserAcessar dto)
        {
            var user = await unitOfWork.UserLoginRepository.RetornarLogin(dto.UserName);

            if (user is null)
                return ResultModel<string>.Erro("Credenciais Inválidas.");

            if (user.TipoPerfil == TipoPerfil.usuario)
                return ResultModel<string>.Erro("Credenciais Inválidas. Acesse o Portal do Cliente para abertura de chamado!");

            var result = await unitOfWork.UserLoginRepository.ConfirmarSenhaLogin(user, dto.Password);

            if (!result)
                return ResultModel<string>.Erro("Credenciais Inválidas."); ;

            var roles = await unitOfWork.UserLoginRepository.RetornarPapeisUser(user.UserName);
            var token = jwt.GeradorJwt(user, roles);

            return ResultModel<string>.Sucesso(token);
        }
    }
}
