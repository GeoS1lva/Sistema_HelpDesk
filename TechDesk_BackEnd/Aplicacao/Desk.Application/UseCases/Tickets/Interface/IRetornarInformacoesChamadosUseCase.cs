using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.UseCases.Tickets.DTOs;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Tickets.Interface
{
    public interface IRetornarInformacoesChamadosUseCase
    {
        public Task<ResultModel<List<InformacoesChamadoKanban>>> RetornarInformacoesKanban();
        public Task<ResultModel<List<InformacoesChamadoKanban>>> RetornarInformacoesChamadosUsuarioEmpresa(string userRequest);
        public Task<ResultModel<RetornarDetalhesChamadosUsuarioEmpresa>> RetornarChamadoUsuarioEmpresa(string numeroChamado);
        public Task<ResultModel<RetornarQuantidadeChamado>> RetornarQuantidadeChamadoPorUsuarioEmpresa(string userRequest);
    }
}
