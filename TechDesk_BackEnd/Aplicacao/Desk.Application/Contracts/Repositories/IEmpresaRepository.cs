using Sistema_HelpDesk.Desk.Domain.Empresas.Entidades;

namespace Sistema_HelpDesk.Desk.Application.Contracts.Repositories
{
    public interface IEmpresaRepository
    {
        public void AdicionarEmpresa(Empresa empresa);
        public Task<bool> RemoverEmpresa(int id);
        public Task<bool> AtualizarEmpresaNome(string cnpj, string nome);
        public Task<bool> AtualizarEmpresaEmail(string cnpj, string email);
        public Task<Empresa?> RetornarEmpresa(string cnpj);
        public Task<List<Empresa>> RetornarListaEmpresas();
        public Task<List<Empresa>> RetornarListaEmpresasAtivas();
    }
}
