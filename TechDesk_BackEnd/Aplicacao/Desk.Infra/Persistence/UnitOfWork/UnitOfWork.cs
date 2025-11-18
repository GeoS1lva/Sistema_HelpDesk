using Microsoft.AspNetCore.Identity;
using Sistema_HelpDesk.Desk.Application.Contracts.Repositories;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;
using Sistema_HelpDesk.Desk.Infra.Context;
using Sistema_HelpDesk.Desk.Infra.Identity;
using Sistema_HelpDesk.Desk.Infra.Persistence.Repositories;

namespace Sistema_HelpDesk.Desk.Infra.Persistence.UnitOfWork
{
    public class UnitOfWork(SqlServerIdentityDbContext context, UserManager<UserLogin> userManager) : IUnitOfWork
    {
        public IUserLoginRepository UserLoginRepository { get; } = new UserLoginRepository(userManager);
        public IUsuarioSistemaRepository UsuarioSistemaRepository { get; } = new UsuarioSistemaRepository(context, userManager);
        public IEmpresaRepository EmpresaRepository { get; } = new EmpresaRepository(context);
        public IUsuarioEmpresaRepository UsuarioEmpresaRepository { get; } = new UsuarioEmpresaRepository(context, userManager);
        public IMesasAtendimentosRepository MesasAtendimentosRepository { get; set; } = new MesasAtendimentoRepository(context);
        public ICategoriaRepository CategoriaRepository { get; set; } = new CategoriaRepository(context);
        public ISubCategoriasRepository SubCategoriasRepository { get; set; } = new SubCategoriasRepository(context);
        public IChamadoRepository ChamadoRepository { get; set; } = new ChamadoRepository(context);
        public IApontamentosChamadosRepository ApontamentosChamadosRepository { get; set; } = new ApontamentosChamadosRepository(context);
        public IRetornoAtendimentoRepository RetornoAtendimentoRepository { get; set; } = new RetornoAtendimentoRepository(context);

        public async Task SaveChangesAsync()
            => await context.SaveChangesAsync();
    }
}
