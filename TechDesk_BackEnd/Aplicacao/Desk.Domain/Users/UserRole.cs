using Microsoft.AspNetCore.Identity;

namespace Sistema_HelpDesk.Desk.Domain.Users
{
    public class UserRole : IdentityRole<int>
    {
        public UserRole() { }
        public UserRole(string roleName) : base(roleName)
        {
        }
    }
}
