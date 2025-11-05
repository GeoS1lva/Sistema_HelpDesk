using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;

namespace Sistema_HelpDesk.Desk.Application.Contracts.Repositories
{
    public interface IChamadoRepository
    {
        public void AdicionarChamado(Chamado chamado);
    }
}
