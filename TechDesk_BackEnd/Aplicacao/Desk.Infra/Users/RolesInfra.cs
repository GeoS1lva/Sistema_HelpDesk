using Microsoft.AspNetCore.Identity;
using Sistema_HelpDesk.Desk.Domain.Users;
using Sistema_HelpDesk.Desk.Domain.Users.Interface;

namespace Sistema_HelpDesk.Desk.Infra.Users
{
    public class RolesInfra(RoleManager<UserRole> roleManager, UserManager<UserLogin> userManager) : IRolesInfra
    {
        private readonly RoleManager<UserRole> _roleManager = roleManager;
        private readonly UserManager<UserLogin> _userManager = userManager;

        public async Task InicializarRolesAsync()
        {
            var rolesList = new[] { "Administrador", "Tecnico", "Usuario" };

            foreach (var role in rolesList)
            {
                var verificacao = await _roleManager.RoleExistsAsync(role);
                if (!verificacao)
                {
                    await _roleManager.CreateAsync(new UserRole(role));
                }
            }

            var userAdmin = new UserLogin { UserName = "Admin" };
            await _userManager.CreateAsync(userAdmin, "Admin@123");
            await _userManager.AddToRoleAsync(userAdmin, "Administrador");
        }
    }
}
