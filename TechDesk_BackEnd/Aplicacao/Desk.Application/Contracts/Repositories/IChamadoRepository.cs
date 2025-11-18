using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;

namespace Sistema_HelpDesk.Desk.Application.Contracts.Repositories
{
    public interface IChamadoRepository
    {
        public void AdicionarChamado(Chamado chamado);
        public Task<Chamado?> RetornarChamado(string numeroChamado);
        public Task<List<Chamado>> RetornarChamados();
        public Task<List<Chamado>> RetornarChamadosUsuarioEmpresa(int usuarioEmpresaId);
        public Task<Chamado?> RetornarChamadoUsuarioEmpresa(string numeroChamado);
        public Task<List<Chamado>?> RetornarChamadosEmAndamento();
        public Task<int> RetornarQuantidadeChamadoAbertoPorUserEmpresa(int usuarioEmpresaId);
        public Task<int> RetornarQuantidadeChamadoPausadoPorUserEmpresa(int usuarioEmpresaId);
        public Task<int> RetornarQuantidadeChamadoFinalizadoPorUserEmpresa(int usuarioEmpresaId);
    }
}
