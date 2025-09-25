using Sistema_HelpDesk.Desk.Domain.Users;
using System.Security.Claims;

namespace Sistema_HelpDesk.Desk.Application.Autenticação.Interface
{
    public interface IJwtGerador
    {
        public string GeradorJwt(UserLogin user, IEnumerable<string> roles, IEnumerable<Claim>? extraClaims = null);
    }
}
