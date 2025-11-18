using Microsoft.AspNetCore.Identity;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;

namespace Sistema_HelpDesk.Desk.Application.Contracts.Repositories
{
    public interface IUserLoginRepository
    {
        public Task CriarLogin(UserLogin userLogin, string passwords, string role);
        public Task BloquearLogin(UserLogin user);
        public Task DesbloquearLogin(UserLogin user);
        public Task<bool> AtualizarUserLogin(UserLogin user);
        public Task TrocarSenhaUserLogin(UserLogin user, string novaSenha);
        public Task<bool> ConfirmarSituacaoUsuario(UserLogin user);
        public Task<UserLogin?> RetornarLogin(string userName);
        public Task<UserLogin?> RetornarLogin(int id);
        public Task<bool> ConfirmarSenhaLogin(UserLogin user, string passwords);
        public Task<IList<string>> RetornarPapeisUser(string userName);
        public Task<string> TokenResetSenha(UserLogin user);
        public Task<IdentityResult> ResetarSenha(UserLogin user, string token, string novaSenha);
        public Task<bool> ConfirmarUserNameCadastrado(string userName);
        public Task<List<UserLoginInformacoes>> RetornarLoginsCriados();

    }
}
