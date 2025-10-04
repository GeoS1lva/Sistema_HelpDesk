using Microsoft.AspNetCore.Identity;
using Sistema_HelpDesk.Desk.Application.Contracts.Repositories;
using Sistema_HelpDesk.Desk.Domain.Users;

namespace Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IUserLoginRepository UserLoginRepository { get; }
        public ITecnicoRepository TecnicoRepository { get; }
        public IEmpresaRepository EmpresaRepository { get; }
        public IUsuarioEmpresaRepository UsuarioEmpresaRepository { get; }
        public IMesasAtendimentosRepository MesasAtendimentosRepository { get; set; }
        public Task SaveChangesAsync();
    }
}
