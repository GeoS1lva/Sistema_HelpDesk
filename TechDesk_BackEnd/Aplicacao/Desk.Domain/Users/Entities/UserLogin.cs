using Microsoft.AspNetCore.Identity;
using Sistema_HelpDesk.Desk.Domain.Empresas.Entidades;
using Sistema_HelpDesk.Desk.Domain.Users.Enum;

namespace Sistema_HelpDesk.Desk.Domain.Users.Entities
{
    public class UserLogin : IdentityUser<int>
    {
        public TipoPerfil TipoPerfil { get; set; }

        public Tecnico? Tecnico { get; set; }
        public UsuariosEmpresa? UsuariosEmpresa { get; set; }
    }
}
