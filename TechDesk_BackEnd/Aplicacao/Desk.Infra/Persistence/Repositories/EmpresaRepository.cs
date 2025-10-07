using Microsoft.EntityFrameworkCore;
using Sistema_HelpDesk.Desk.Domain.Empresas.Entidades;
using Sistema_HelpDesk.Desk.Infra.Context;
using Sistema_HelpDesk.Desk.Application.Contracts.Repositories;

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

        public async Task<bool> AtualizarEmpresaNome(string cnpj, string nome)
        {
            var empresa = await _context.Empresas.FirstOrDefaultAsync(x => x.Cnpj == cnpj);

            if (empresa is null)
                return false;

            empresa.Nome = nome;

            return true;
        }

        public async Task<bool> AtualizarEmpresaEmail(string cnpj, string email)
        {
            var empresa = await _context.Empresas.FirstOrDefaultAsync(x => x.Cnpj == cnpj);

            if (empresa is null)
                return false;

            empresa.Email = email;

            return true;
        }

        public async Task<Empresa?> RetornarEmpresa(string cnpj)
            => await _context.Empresas.FirstOrDefaultAsync(x => x.Cnpj == cnpj);

        public async Task<List<Empresa>> RetornarListaEmpresas()
            => await _context.Empresas.ToListAsync();

        public async Task<List<Empresa>> RetornarListaEmpresasAtivas()
            => await _context.Empresas.Where(x => x.Status == true).OrderBy(x => x.Nome).ToListAsync();
    }
}
