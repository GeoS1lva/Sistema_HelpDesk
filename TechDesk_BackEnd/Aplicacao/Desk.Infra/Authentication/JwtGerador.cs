using Microsoft.IdentityModel.Tokens;
using Sistema_HelpDesk.Desk.Application.Contracts.Security;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sistema_HelpDesk.Desk.Infra.Autenticação
{
    public class JwtGerador : IJwtGerador
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _key;
        private readonly int _minutes;

        public JwtGerador(IConfiguration config)
        {
            var section = config.GetSection("Jwt");
            _issuer = section["Issuer"]!;
            _audience = section["Audience"]!;
            _key = section["Key"]!;
            _minutes = int.Parse(section["AccessTokenMinutes"] ?? "60");
        }

        public string GeradorJwt(UserLogin user, IEnumerable<string> roles, IEnumerable<Claim>? extraClaims = null)
        {
            var claims = new List<Claim>
            {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName)
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var creds = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
            SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_minutes),
            signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
