using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;
using Sistema_HelpDesk.Desk.Domain.Empresas.Entidades;
using Sistema_HelpDesk.Desk.Domain.Mesa;
using Sistema_HelpDesk.Desk.Domain.Users;

namespace Sistema_HelpDesk.Desk.Infra.Context
{
    public class SqlServerIdentityDbContext : IdentityDbContext<UserLogin, UserRole, int>
    {
        public SqlServerIdentityDbContext(DbContextOptions<SqlServerIdentityDbContext> options) : base(options) { }

        public DbSet<Tecnico> Tecnicos { get; set; }
        public DbSet<UsuariosEmpresa> UsuariosEmpresa { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Chamado> Chamados { get; set; }
        public DbSet<ApontamentoHorasChamado> HorasChamados { get; set; }
        public DbSet<MesaAtendimento> MesasAtendimento { get; set; }
        public DbSet<MesaTecnicos> MesaTecnicos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tecnico>(t =>
            {
                t.HasKey(x => x.Id);
                t.Property(x => x.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<UsuariosEmpresa>(u =>
            {
                u.HasKey(x => x.Id);
                u.Property(x => x.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<UserLogin>()
                .HasOne(x => x.Tecnico)
                .WithOne(t => t.UserLogin)
                .HasForeignKey<Tecnico>(t => t.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserLogin>()
                .HasOne(x => x.Cliente)
                .WithOne(t => t.UserLogin)
                .HasForeignKey<UsuariosEmpresa>(t => t.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UsuariosEmpresa>()
                .HasOne(x => x.Empresa)
                .WithMany(e => e.Usuarios)
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Chamado>()
                .HasOne(x => x.Empresa)
                .WithMany(e => e.Chamados)
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Chamado>()
                .HasOne(x => x.UsuarioEmpresa)
                .WithMany()
                .HasForeignKey(x => x.UsuarioEmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Chamado>()
                .HasOne(x => x.TecnicoFinalizacao)
                .WithMany(e => e.Chamados)
                .HasForeignKey(x => x.TecnicoFinalizacaoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MesaAtendimento>()
                .HasMany(x => x.Tecnicos)
                .WithMany(t => t.Mesas)
                .UsingEntity<MesaTecnicos>();

            modelBuilder.Entity<ApontamentoHorasChamado>()
                .HasOne(x => x.Chamado)
                .WithMany(c => c.Apontamentos)
                .HasForeignKey(x => x.ChamadoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApontamentoHorasChamado>()
                .HasOne(x => x.Tecnico)
                .WithMany()
                .HasForeignKey(x => x.TecnicoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
