using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;

namespace Sistema_HelpDesk.Desk.Application.Contracts.Repositories
{
    public interface IApontamentosChamadosRepository
    {
        public void AdicionarApontamentoChamado(ApontamentoHorasChamado apontamento);
        public Task<ApontamentoHorasChamado?> RetornarApontamentoAtivo(int chamadoId);
    }
}
