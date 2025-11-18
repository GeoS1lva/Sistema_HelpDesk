using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entities;
using Sistema_HelpDesk.Desk.Domain.Chat;
using Sistema_HelpDesk.Desk.Domain.Empresas.Entidades;
using Sistema_HelpDesk.Desk.Domain.Mesa.Entities;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;

namespace Sistema_HelpDesk.Desk.Infra.Context
{
    public class SqlServerIdentityDbContext : IdentityDbContext<UserLogin, UserRole, int>
    {
        public SqlServerIdentityDbContext(DbContextOptions<SqlServerIdentityDbContext> options) : base(options) { }

        public DbSet<UsuarioSistema> Tecnicos { get; set; }
        public DbSet<UsuariosEmpresa> UsuariosEmpresa { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Chamado> Chamados { get; set; }
        public DbSet<ApontamentoHorasChamado> HorasChamados { get; set; }
        public DbSet<MesaAtendimento> MesasAtendimento { get; set; }
        public DbSet<MesaTecnicos> MesaTecnicos { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<SubCategoria> SubCategoria { get; set; }
        public DbSet<RetornoAtendimento> RetornoAtendimento { get; set; }
        public DbSet<ChatChamado> Chat { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserLogin>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<UsuarioSistema>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<UsuarioSistema>()
                .Property(x => x.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<UsuarioSistema>()
                .HasOne(x => x.UserLogin)
                .WithOne(x => x.UsuarioSitema)
                .HasForeignKey<UsuarioSistema>(x => x.Id);

            modelBuilder.Entity<UsuariosEmpresa>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<UsuariosEmpresa>()
                .Property(x => x.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<UsuariosEmpresa>()
                .HasOne(x => x.UserLogin)
                .WithOne(x => x.UsuariosEmpresa)
                .HasForeignKey<UsuariosEmpresa>(x => x.Id);

            modelBuilder.Entity<Chamado>()
                .HasIndex(c => c.NumeroChamado)
                .IsUnique();

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
                .HasOne(x => x.TecnicoResponsavel)
                .WithMany(e => e.Chamados)
                .HasForeignKey(x => x.TecnicoResponsavelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Chamado>()
                .HasOne(x => x.SubCategoria)
                .WithMany()
                .HasForeignKey(x => x.SubCategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Chamado>()
                .HasOne(x => x.MesaAtendimento)
                .WithMany()
                .HasForeignKey(x => x.MesaAtendimentoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MesaAtendimento>()
                .HasMany(x => x.Tecnico)
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

            modelBuilder.Entity<Categoria>()
                .HasMany(c => c.SubCategorias)
                .WithOne(x => x.Categoria)
                .HasForeignKey(s => s.CategoriaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RetornoAtendimento>()
                .HasOne(x => x.Chamado)
                .WithMany(x => x.Retornos)
                .HasForeignKey(x => x.ChamadoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChatChamado>()
                .HasOne(x => x.Chamado)
                .WithMany(x => x.Chat)
                .HasForeignKey(x => x.ChamadoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
