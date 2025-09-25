using Microsoft.AspNetCore.Identity;
using Sistema_HelpDesk.Desk.Application.Autenticação.Interface;
using Sistema_HelpDesk.Desk.Domain.Models;
using Sistema_HelpDesk.Desk.Domain.Models.DTOs.Autenticação;
using Sistema_HelpDesk.Desk.Domain.Users;

namespace Sistema_HelpDesk.Desk.Application.Autenticação.Service
{
    public class AutheService(UserManager<UserLogin> user, IJwtGerador jwtGerador) : IAutheService
    {
        private readonly UserManager<UserLogin> User = user;
        private readonly IJwtGerador JwtGerador = jwtGerador;

        public async Task<ResultModel> CriarLogin(CadastrarUserDto dto)
        {
            var novoUser = new UserLogin { UserName = dto.UserName, Email = dto.Email };
            var result = await User.CreateAsync(novoUser, dto.Password);
            await User.AddToRoleAsync(novoUser, dto.Role);

            if (!result.Succeeded)
                return ResultModel.Erro("Usuário não Cadastrado!");

            return ResultModel.Sucesso();
        }

        public async Task<string> FazerLogin(LoginUserDto dto)
        {
            var user = await User.FindByNameAsync(dto.UserName);

            if (user is null)
                return new string("Credenciais Inválidas.");

            var confirSenha = await User.CheckPasswordAsync(user, dto.Password);

            if (!confirSenha)
                return new string("Credenciais Inválidas."); ;

            var roles = await User.GetRolesAsync(user);
            var token = JwtGerador.GeradorJwt(user, roles);

            return token;
        } 
    }
}
