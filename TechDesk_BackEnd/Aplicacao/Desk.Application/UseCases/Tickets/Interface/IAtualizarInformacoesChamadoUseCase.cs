using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.UseCases.Tickets.DTOs;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Tickets.Interface
{
    public interface IAtualizarInformacoesChamadoUseCase
    {
        public Task<ResultModel> AlteracoesChamados(AlterarChamado alterarChamado);
    }
}
