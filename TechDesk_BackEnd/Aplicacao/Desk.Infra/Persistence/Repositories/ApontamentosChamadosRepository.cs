using Microsoft.EntityFrameworkCore;
using Sistema_HelpDesk.Desk.Application.Contracts.Repositories;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;
using Sistema_HelpDesk.Desk.Domain.Chamados.Enums;
using Sistema_HelpDesk.Desk.Infra.Context;

namespace Sistema_HelpDesk.Desk.Infra.Persistence.Repositories
{
    public class ApontamentosChamadosRepository(SqlServerIdentityDbContext context) : IApontamentosChamadosRepository
    {
        private readonly SqlServerIdentityDbContext _context = context;

        public void AdicionarApontamentoChamado(ApontamentoHorasChamado apontamento)
            => _context.HorasChamados.Add(apontamento);

        public async Task<ApontamentoHorasChamado?> RetornarApontamentoAtivo(int chamadoId)
            => await _context.HorasChamados.FirstOrDefaultAsync(x => x.ChamadoId == chamadoId && x.Status == StatusApontamento.emAndamento);

    }
}
