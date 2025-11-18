using Sistema_HelpDesk.Desk.Domain.Empresas.Entidades;

namespace Sistema_HelpDesk.Desk.Application.Contracts.Repositories
{
    public interface IUsuarioEmpresaRepository
    {
        public void AdicionarUsuarioEmpresa(UsuariosEmpresa user);
        public Task<bool> RemoverUsuarioEmpresa(int id);
        public Task<UsuariosEmpresa?> RetornarUsuario(int id);
        public Task<List<UsuariosEmpresa>> RetornarUsuariosEmpresas();
        public Task<List<UsuariosEmpresa>> RetornarUsuariosPorEmpresa(int id);
    }
}
