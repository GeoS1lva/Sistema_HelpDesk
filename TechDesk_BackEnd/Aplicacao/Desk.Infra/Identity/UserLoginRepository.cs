using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sistema_HelpDesk.Desk.Application.Contracts.Repositories;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;
using Sistema_HelpDesk.Desk.Infra.Context;

namespace Sistema_HelpDesk.Desk.Infra.Identity
{
    public class UserLoginRepository(UserManager<UserLogin> userManager) : IUserLoginRepository
    {
        private readonly UserManager<UserLogin> _userManager = userManager;

        public async Task CriarLogin(UserLogin userLogin, string passwords, string role)
        {
            var result = await _userManager.CreateAsync(userLogin, passwords);

            await _userManager.AddToRoleAsync(userLogin, role);
        }

        public async Task BloquearLogin(UserLogin user)
        {
            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            await _userManager.AccessFailedAsync(user);
        }

        public async Task DesbloquearLogin(UserLogin user)
        {
            await _userManager.SetLockoutEndDateAsync(user, null);
            await _userManager.ResetAccessFailedCountAsync(user);
            await _userManager.SetLockoutEnabledAsync(user, true);
        }

        public async Task<bool> AtualizarUserLogin(UserLogin user)
        {
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return false;

            return true;
        }

        public async Task TrocarSenhaUserLogin(UserLogin user, string novaSenha)
        {
            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, novaSenha);
        }

        public async Task<bool> ConfirmarSituacaoUsuario(UserLogin user)
            => await _userManager.IsLockedOutAsync(user);

        public async Task<UserLogin?> RetornarLogin(string userName)
            => await _userManager.FindByNameAsync(userName);
        public async Task<UserLogin?> RetornarLogin(int id)
            => await _userManager.FindByIdAsync(id.ToString());

        public async Task<bool> ConfirmarSenhaLogin(UserLogin user, string passwords)
            => await _userManager.CheckPasswordAsync(user, passwords);

        public async Task<IList<string>> RetornarPapeisUser(string userName)
            => await _userManager.GetRolesAsync(await RetornarLogin(userName));

        public async Task<string> TokenResetSenha(UserLogin user)
            => await _userManager.GeneratePasswordResetTokenAsync(user);

        public async Task<IdentityResult> ResetarSenha(UserLogin? user, string token, string novaSenha)
            => await _userManager.ResetPasswordAsync(user, token, novaSenha);

        public async Task<bool> ConfirmarUserNameCadastrado(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            return user != null ? true : false;
        }

        public async Task<List<UserLoginInformacoes>> RetornarLoginsCriados()
        {
            var lista = await _userManager.Users
                                          .AsNoTracking()
                                          .Select(x => new UserLoginInformacoes { 
                                              Id = x.Id, 
                                              Email = x.Email, 
                                              UserName = x.UserName 
                                          })
                                          .ToListAsync();

            return lista;
        }
    }
}
