using Microsoft.AspNetCore.Identity;
using Sistema_HelpDesk.Desk.Application.Contracts.Repositories;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;

namespace Sistema_HelpDesk.Desk.Infra.Identity
{
    public class UserLoginRepository(UserManager<UserLogin> userManager) : IUserLoginRepository
    {
        private readonly UserManager<UserLogin> _userManager = userManager;

        public async Task<bool> CriarLogin(UserLogin userLogin, string passwords, string role)
        {
            var result = await _userManager.CreateAsync(userLogin, passwords);
            await _userManager.AddToRoleAsync(userLogin, role);

            return result.Succeeded ? true : false;
        }

        public async Task BloquearLogin(UserLogin user)
        {
            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            await _userManager.AccessFailedAsync(user);
        }

        public async Task<UserLogin?> RetornarLogin(string userName)
            => await _userManager.FindByNameAsync(userName);

        public async Task<bool> ConfirmarSenhaLogin(UserLogin user, string passwords)
            => await _userManager.CheckPasswordAsync(user, passwords);

        public async Task<IList<string>> RetornarPapeisUser(UserLogin user)
            => await _userManager.GetRolesAsync(user);

        public async Task<string> TokenResetSenha(UserLogin user)
            => await _userManager.GeneratePasswordResetTokenAsync(user);
    }
}
