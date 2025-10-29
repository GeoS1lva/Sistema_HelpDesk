using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.DTOs;

namespace Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.Interface
{
    public interface IRemoverMesaAtendimentoUseCase
    {
        public Task<ResultModel> RemoverMesaAtendimento(int id);
    }
}
