using Sistema_HelpDesk.Desk.Domain.Mesa.Entities;

namespace Sistema_HelpDesk.Desk.Application.Contracts.Repositories
{
    public interface IMesasAtendimentosRepository
    {
        public void AdicionarMesaAtendimento(MesaAtendimento mesa);
        public Task<bool> RemoverMesaAtendimento(string nome);
        public Task<List<MesaAtendimento>> RetornarListaMesasAtendimento();
        public Task<List<MesaAtendimento>> RetornarListaMesasAtendimentoAtivas();
    }
}
