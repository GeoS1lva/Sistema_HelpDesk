using Sistema_HelpDesk.Desk.Domain.Users.Entities;

namespace Sistema_HelpDesk.Desk.Application.Contracts.Repositories
{
    public interface IUserLoginRepository
    {
        public Task<bool> CriarLogin(UserLogin userLogin, string passwords, string role);
        public Task BloquearLogin(UserLogin user);
        public Task<UserLogin?> RetornarLogin(string userName);
        public Task<bool> ConfirmarSenhaLogin(UserLogin user, string passwords);
        public Task<IList<string>> RetornarPapeisUser(UserLogin user);
        public Task<string> TokenResetSenha(UserLogin user);
    }
}
