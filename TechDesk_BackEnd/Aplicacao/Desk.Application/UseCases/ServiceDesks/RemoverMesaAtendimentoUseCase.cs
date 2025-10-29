using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.Interface;

namespace Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks
{
    public class RemoverMesaAtendimentoUseCase(IUnitOfWork unitOfWork) : IRemoverMesaAtendimentoUseCase
    {
        public async Task<ResultModel> RemoverMesaAtendimento(int id)
        {
            var mesaDelete = await unitOfWork.MesasAtendimentosRepository.RetornarMesaAtendimento(id);

            if (mesaDelete is null)
                return ResultModel.Erro("Mesa Não encontrada!");

            await unitOfWork.MesasAtendimentosRepository.RemoverMesaAtendimento(mesaDelete);
            await unitOfWork.SaveChangesAsync();

            return ResultModel.Sucesso();

        }
    }
}
