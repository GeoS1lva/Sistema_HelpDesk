using Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.DTOs;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entities;

namespace Sistema_HelpDesk.Desk.Application.Contracts.Repositories
{
    public interface IRetornoAtendimentoRepository
    {
        public void AdicionarRetornoAtendimento(RetornoAtendimento retorno);
        public Task<List<RetornoAtendimentoDto>> RetornoAcoesChamados(string numeroChamado);
    }
}
