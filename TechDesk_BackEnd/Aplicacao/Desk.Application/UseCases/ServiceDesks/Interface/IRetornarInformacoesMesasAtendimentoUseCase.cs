using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.DTOs;
using Sistema_HelpDesk.Desk.Domain.Mesa.Entities;

namespace Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.Interface
{
    public interface IRetornarInformacoesMesasAtendimentoUseCase
    {
        public Task<ResultModel<List<MesaAtendimentoInformacoes>>> RetornarMesasAtendimento();
    }
}
