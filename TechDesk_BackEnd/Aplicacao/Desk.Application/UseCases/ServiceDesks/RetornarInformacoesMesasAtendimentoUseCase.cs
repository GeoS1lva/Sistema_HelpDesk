using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.Interface;
using Sistema_HelpDesk.Desk.Domain.Mesa.Entities;

namespace Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks
{
    public class RetornarInformacoesMesasAtendimentoUseCase(IUnitOfWork unitOfWork) : IRetornarInformacoesMesasAtendimentoUseCase
    {
        public async Task<ResultModel<List<MesaAtendimentoInformacoes>>> RetornarMesasAtendimento()
        {
            var mesasNome = await unitOfWork.MesasAtendimentosRepository.RetornarListaMesasAtendimento();

            if (mesasNome is null)
                return ResultModel<List<MesaAtendimentoInformacoes>>.Erro("Não foi encontrado nenhuma mesa de atendimento!");

            return ResultModel<List<MesaAtendimentoInformacoes>>.Sucesso(mesasNome);
        }
    }
}
