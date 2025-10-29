using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.Interface;
using Sistema_HelpDesk.Desk.Domain.Mesa.Entities;

namespace Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks
{
    public class CriarMesaAtendimentoUseCase(IUnitOfWork unitOfWork) : ICriarMesaAtendimentoUseCase
    {
        public async Task<ResultModel> CriarMesaAtendimento(MesaAtendimentoDTO mesa)
        {
            if (await unitOfWork.MesasAtendimentosRepository.ConfirmarMesaCadastrada(mesa.Nome))
                return ResultModel.Erro("Mesa de Atendimento já cadastrada");

            var novaMesa = new MesaAtendimento(mesa.Nome);
            unitOfWork.MesasAtendimentosRepository.AdicionarMesaAtendimento(novaMesa);

            await unitOfWork.SaveChangesAsync();

            return ResultModel.Sucesso();
        }
    }
}
