using Microsoft.EntityFrameworkCore;
using Sistema_HelpDesk.Desk.Application.Contracts.Repositories;
using Sistema_HelpDesk.Desk.Application.UseCases.Tickets.DTOs;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;
using Sistema_HelpDesk.Desk.Domain.Chamados.Enums;
using Sistema_HelpDesk.Desk.Infra.Context;

namespace Sistema_HelpDesk.Desk.Infra.Persistence.Repositories
{
    public class ChamadoRepository(SqlServerIdentityDbContext context) : IChamadoRepository
    {
        private readonly SqlServerIdentityDbContext _context = context;

        public void AdicionarChamado(Chamado chamado)
            => _context.Chamados.Add(chamado);

        public async Task<Chamado?> RetornarChamado(string numeroChamado)
            => await _context.Chamados
            .Include(x => x.TecnicoResponsavel)
            .FirstOrDefaultAsync(x => x.NumeroChamado == numeroChamado);

        public async Task<List<Chamado>> RetornarChamados()
            => await _context.Chamados
            .Include(x => x.SubCategoria)
            .Include(x => x.Empresa)
            .Include(x => x.UsuarioEmpresa)
            .Include(x => x.TecnicoResponsavel)
            .Include(x => x.MesaAtendimento)
            .ToListAsync();

        public async Task<List<Chamado>> RetornarChamadosUsuarioEmpresa(int usuarioEmpresaId)
            => await _context.Chamados
            .Include(x => x.SubCategoria)
            .Include(x => x.Empresa)
            .Include(x => x.UsuarioEmpresa)
            .Include(x => x.TecnicoResponsavel)
            .Include(x => x.MesaAtendimento)
            .Where(x => x.UsuarioEmpresaId == usuarioEmpresaId)
            .ToListAsync();

        public async Task<Chamado?> RetornarChamadoUsuarioEmpresa(string numeroChamado)
            => await _context.Chamados
            .Include(x => x.SubCategoria)
            .Include(x => x.Empresa)
            .Include(x => x.UsuarioEmpresa)
            .Include(x => x.TecnicoResponsavel)
            .Include(x => x.MesaAtendimento)
            .FirstOrDefaultAsync(x => x.NumeroChamado == numeroChamado);

        public async Task<List<Chamado>?> RetornarChamadosEmAndamento()
            => await _context.Chamados.Include(x => x.SubCategoria).Where(x => x.Status == Status.emAndamento).ToListAsync();

        public async Task<int> RetornarQuantidadeChamadoAbertoPorUserEmpresa(int usuarioEmpresaId)
            => await _context.Chamados
            .Where(x => x.UsuarioEmpresaId == usuarioEmpresaId && x.Status == Status.aberto)
            .CountAsync();

        public async Task<int> RetornarQuantidadeChamadoPausadoPorUserEmpresa(int usuarioEmpresaId)
            => await _context.Chamados
            .Where(x => x.UsuarioEmpresaId == usuarioEmpresaId && x.Status == Status.pausado)
            .CountAsync();

        public async Task<int> RetornarQuantidadeChamadoFinalizadoPorUserEmpresa(int usuarioEmpresaId)
            => await _context.Chamados
            .Where(x => x.UsuarioEmpresaId == usuarioEmpresaId && x.Status == Status.finalizado)
            .CountAsync();
    }
}
