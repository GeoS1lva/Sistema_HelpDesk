using Sistema_HelpDesk.Desk.Domain.Users.Entities;

namespace Sistema_HelpDesk.Desk.Application.Contracts.Repositories
{
    public interface IUsuarioSistemaRepository
    {
        public void AdicionarUsuario(UsuarioSistema usuario);
        public Task<bool> RemoverUsuario(int id);
        public Task<UsuarioSistema?> RetornarUsuario(int id);
        public Task<List<UsuarioSistema>> RetornarUsuarios();
        public Task<List<UsuarioSistema>> RetornarUsuariosAtivos();
    }
}
