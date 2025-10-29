using Microsoft.EntityFrameworkCore;
using Sistema_HelpDesk.Desk.Domain.Empresas.Entidades;
using Sistema_HelpDesk.Desk.Infra.Context;
using Sistema_HelpDesk.Desk.Application.Contracts.Repositories;
using Sistema_HelpDesk.Desk.Application.UseCases.Empresas.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Companies.DTOs;

namespace Sistema_HelpDesk.Desk.Infra.Persistence.Repositories
{
    public class EmpresaRepository(SqlServerIdentityDbContext context) : IEmpresaRepository
    {
        private readonly SqlServerIdentityDbContext _context = context;

        public void AdicionarEmpresa(Empresa empresa)
            => _context.Empresas.Add(empresa);

        public async Task<bool> RemoverEmpresa(int id)
        {
            var empresa = await _context.Empresas.FirstOrDefaultAsync(x => x.Id == id);

            if (empresa is null)
                return false;

            empresa.Desativado();

            return true;
        }

        public async Task<bool> AtualizarEmpresaNome(int empresaId, EmpresaAtualizarNome empresaAtualizar)
        {
            var empresa = await RetornarEmpresa(empresaId);

            if (empresa is null)
                return false;

            empresa.Nome = empresaAtualizar.Nome;

            return true;
        }

        public async Task<bool> AtualizarEmpresaEmail(int empresaId, EmpresaAtualizarEmail empresaAtualizar)
        {
            var empresa = await RetornarEmpresa(empresaId);

            if (empresa is null)
                return false;

            empresa.Email = empresaAtualizar.Email;

            return true;
        }

        public async Task<Empresa?> RetornarEmpresa(int id)
            => await _context.Empresas.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<EmpresaInformacoes>> RetornarListaEmpresas()
            => await _context.Empresas.Select(x => new EmpresaInformacoes { Id = x.Id, Nome = x.Nome, Cnpj = x.Cnpj, Email = x.Email, DataCriacao = x.DataCriacao, Status = x.Status })
                                      .ToListAsync();

        public async Task<bool> ConsultarCnpj(string cnpj)
            => await _context.Empresas.AnyAsync(x => x.Cnpj == cnpj);
    }
}
