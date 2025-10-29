using Sistema_HelpDesk.Desk.Application.UseCases.Companies.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Empresas.DTOs;
using Sistema_HelpDesk.Desk.Domain.Empresas.Entidades;

namespace Sistema_HelpDesk.Desk.Application.Contracts.Repositories
{
    public interface IEmpresaRepository
    {
        public void AdicionarEmpresa(Empresa empresa);
        public Task<bool> RemoverEmpresa(int id);
        public Task<bool> AtualizarEmpresaNome(int empresaId, EmpresaAtualizarNome empresaAtualizar);
        public Task<bool> AtualizarEmpresaEmail(int empresaId, EmpresaAtualizarEmail empresaAtualizar);
        public Task<Empresa?> RetornarEmpresa(int id);
        public Task<List<EmpresaInformacoes>> RetornarListaEmpresas();
        public Task<bool> ConsultarCnpj(string cnpj);
    }
}
