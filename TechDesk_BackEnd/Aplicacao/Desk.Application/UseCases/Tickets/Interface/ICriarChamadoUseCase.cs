using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.UseCases.Tickets.DTOs;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Tickets.Interface
{
    public interface ICriarChamadoUseCase
    {
        public Task<ResultModel<RetornarChamadoUsuarioEmpresa>> AbrirChamadoPainel(AbrirChamadoUsuarioEmpresa solicitacao, string userNameRequest);
        public Task<ResultModel<RetornarChamadoUsuarioSistema>> AbrirChamadoSistema(AbrirChamadoUsuarioSistema solicitacao, string userNameRequest);
    }
}
