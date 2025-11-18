using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Tickets.DTOs;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entities;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Tickets.Interface
{
    public interface IAcoesChamadoUseCase
    {
        public Task<ResultModel<RetornoAtendimentoDto>> PlayChamado(string numeroChamado, string userRequest);
        public Task<ResultModel<RetornoAtendimentoDto>> PausarChamado(PausaChamado pausaChamado, string userRequest);
        public Task<ResultModel<RetornoAtendimentoDto>> FinalizarChamado(FinalizarChamado finalizarChamado, string userRequest);
        public Task<ResultModel<RetornoAtendimentoDto>> AdicionarComentarioChamado(AdicionarComentario comentario, string userRequest);
        public Task<List<RetornoAtendimentoDto>> RetornoTodasAcoesChamado(string numeroChamado);
    }
}
