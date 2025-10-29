using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.DTOs;

namespace Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.Interface
{
    public interface ICriarMesaAtendimentoUseCase
    {
        public Task<ResultModel> CriarMesaAtendimento(MesaAtendimentoDTO mesa);
    }
}
