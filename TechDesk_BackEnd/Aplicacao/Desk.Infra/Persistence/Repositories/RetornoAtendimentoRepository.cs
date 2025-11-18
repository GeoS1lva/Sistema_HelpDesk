using Microsoft.EntityFrameworkCore;
using Sistema_HelpDesk.Desk.Application.Contracts.Repositories;
using Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.DTOs;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entities;
using Sistema_HelpDesk.Desk.Infra.Context;

namespace Sistema_HelpDesk.Desk.Infra.Persistence.Repositories
{
    public class RetornoAtendimentoRepository(SqlServerIdentityDbContext context) : IRetornoAtendimentoRepository
    {
        private readonly SqlServerIdentityDbContext _context = context;

        public void AdicionarRetornoAtendimento(RetornoAtendimento retorno)
            => _context.RetornoAtendimento.Add(retorno);

        public async Task<List<RetornoAtendimentoDto>> RetornoAcoesChamados(string numeroChamado)
            => await _context.RetornoAtendimento
            .Where(x => x.NumeroChamado == numeroChamado)
            .Select(x => new RetornoAtendimentoDto
            {
                NumeroChamado = x.NumeroChamado,
                RetornoAcao = x.RetornoAcao,
                UsuarioNome = x.NomeTecnico,
                HoraAtendimento = x.HoraAtendimento,
                Mensagem = x.Mensagem
            }).OrderBy(x => x.HoraAtendimento).ToListAsync();
    }
}
