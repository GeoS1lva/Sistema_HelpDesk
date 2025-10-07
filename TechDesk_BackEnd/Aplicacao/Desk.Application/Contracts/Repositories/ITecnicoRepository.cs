using Sistema_HelpDesk.Desk.Domain.Users.Entities;

namespace Sistema_HelpDesk.Desk.Application.Contracts.Repositories
{
    public interface ITecnicoRepository
    {
        public void AdicionarTecnico(Tecnico tecnico);
        public Task<bool> RemoverTecnico(int id);
        public Task<Tecnico?> RetornarTecnico(int id);
        public Task<List<Tecnico>> RetornarTecnicos();
        public Task<List<Tecnico>> RetornarTecnicosAtivos();
    }
}
