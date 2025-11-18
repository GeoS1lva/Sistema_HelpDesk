using Microsoft.AspNetCore.SignalR;
using Quartz;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Tickets.DTOs;
using Sistema_HelpDesk.Desk.Infra.SignaLR;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Tickets
{
    [DisallowConcurrentExecution]
    public class AtualizarSlaJob : IJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ChamadosHub> _hubContext;

        public AtualizarSlaJob(IUnitOfWork unitOfWork, IHubContext<ChamadosHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var chamadosEmAndamento = await _unitOfWork.ChamadoRepository.RetornarChamadosEmAndamento();

            List<AtualizarSlaDto> chamadosAtualizar = [];

            foreach (var chamado in chamadosEmAndamento)
            {
                var apontamentoAtivo = await _unitOfWork.ApontamentosChamadosRepository.RetornarApontamentoAtivo(chamado.Id);

                var segundosCorridos = (DateTimeOffset.UtcNow - apontamentoAtivo.HoraInicio).TotalSeconds;

                var valorPorcentagemEmRisco = chamado.SubCategoria.SLA * 0.8;

                if (segundosCorridos < chamado.SubCategoria.SLA && segundosCorridos < valorPorcentagemEmRisco)
                    chamado.SlaEmConformidade();

                if (segundosCorridos > valorPorcentagemEmRisco && segundosCorridos < chamado.SubCategoria.SLA)
                    chamado.SlaEmRisco();

                if (segundosCorridos >= chamado.SubCategoria.SLA)
                    chamado.SlaVencido();

                chamadosAtualizar.Add(new AtualizarSlaDto { NumeroChamado = chamado.NumeroChamado, StatusSla = chamado.StatusSLA });
            }

            await _unitOfWork.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("slaAtualizado");
        }
    }
}
