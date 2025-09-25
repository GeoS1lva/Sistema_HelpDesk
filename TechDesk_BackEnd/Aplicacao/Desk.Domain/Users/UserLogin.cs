using Microsoft.AspNetCore.Identity;
using Sistema_HelpDesk.Desk.Domain.Empresas.Entidades;

namespace Sistema_HelpDesk.Desk.Domain.Users
{
    public class UserLogin : IdentityUser<int>
    {
        public Tecnico? Tecnico { get; set; }
        public UsuariosEmpresa? Cliente { get; set; }
    }
}
