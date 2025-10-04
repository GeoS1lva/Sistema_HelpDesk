using Sistema_HelpDesk.Desk.Domain.Users.Entities;
using System.Security.Claims;

namespace Sistema_HelpDesk.Desk.Application.Contracts.Security
{
    public interface IJwtGerador
    {
        public string GeradorJwt(UserLogin user, IEnumerable<string> roles, IEnumerable<Claim>? extraClaims = null);
    }
}
