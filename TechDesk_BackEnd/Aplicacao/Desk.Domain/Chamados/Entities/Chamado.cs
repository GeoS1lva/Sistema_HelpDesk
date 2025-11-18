 using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entities;
using Sistema_HelpDesk.Desk.Domain.Chamados.Enums;
using Sistema_HelpDesk.Desk.Domain.Chat;
using Sistema_HelpDesk.Desk.Domain.Common;
using Sistema_HelpDesk.Desk.Domain.Empresas.Entidades;
using Sistema_HelpDesk.Desk.Domain.Mesa.Entities;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_HelpDesk.Desk.Domain.Chamados.Entidades
{
    public class Chamado : Entidade
    {
        public string NumeroChamado { get; private set; }

        public string Assunto { get; private set; }
        public string Descricao { get; private set; }
        public int CategoriaId { get; private set; }

        public int SubCategoriaId { get; set; }
        public SubCategoria SubCategoria { get; private set; }

        public StatusSla StatusSLA { get; set; }

        public int MesaAtendimentoId { get; private set; }
        public MesaAtendimento MesaAtendimento { get; private set; }

        public DateTime DataCriacao { get; private set; }

        public Status Status { get; private set; }

        public long TempoAtendimentoTotal { get; private set; }

        [NotMapped]
        public TimeSpan TempoGastoTotalAtendimento => new TimeSpan(TempoAtendimentoTotal);

        public DateTime? DataFinalizacao { get; private set; }
        public string? ComentarioFinalizacao { get; private set; }

        public int EmpresaId { get; private set; }
        public Empresa Empresa { get; set; }

        public int UsuarioEmpresaId { get; private set; }
        public UsuariosEmpresa UsuarioEmpresa { get; set; }

        public int? TecnicoResponsavelId { get; private set; }
        public UsuarioSistema? TecnicoResponsavel { get; set; }

        public int? TecnicoAberturaChamadoId { get; set; }
        public UsuarioSistema TecnicoAberturaChamado { get; set; }

        public ICollection<ApontamentoHorasChamado> Apontamentos { get; set; } = [];
        public ICollection<RetornoAtendimento> Retornos { get; set; }
        public ICollection<ChatChamado> Chat { get; set; }

        public Chamado(string assunto, string descricao, int categoriaId, int subCategoriaId, int mesaAtendimentoId, int empresaId, int usuariosEmpresaId)
        {
            NumeroChamado = $"CH{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";

            Assunto = assunto;
            Descricao = descricao;
            CategoriaId = categoriaId;
            SubCategoriaId = subCategoriaId;
            EmpresaId = empresaId;
            UsuarioEmpresaId = usuariosEmpresaId;

            MesaAtendimentoId = mesaAtendimentoId;
            StatusSLA = StatusSla.naoIniciado;
            DataCriacao = DateTime.UtcNow;
            Status = Status.aberto;
            TempoAtendimentoTotal = 0;
        }

        public Chamado(string assunto, string descricao, int categoriaId, int subCategoriaId, int mesaAtendimentoId, int empresaId, int usuariosEmpresaId, int tecnicoAberturaChamadoId)
        {
            NumeroChamado = $"CH{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";

            Assunto = assunto;
            Descricao = descricao;
            CategoriaId = categoriaId;
            SubCategoriaId = subCategoriaId;
            EmpresaId = empresaId;
            UsuarioEmpresaId = usuariosEmpresaId;
            TecnicoAberturaChamadoId = tecnicoAberturaChamadoId;
            MesaAtendimentoId = mesaAtendimentoId;

            StatusSLA = StatusSla.naoIniciado;
            DataCriacao = DateTime.UtcNow;
            Status = Status.aberto;
            TempoAtendimentoTotal = 0;
        }

        public Chamado() { }

        public void CalcularTempoGasto(long apontamento)
        {
            TempoAtendimentoTotal += apontamento;
        }

        public void Play()
        {
            Status = Status.emAndamento;
        }

        public void Pausar()
        {
            Status = Status.pausado;
        }

        public void Finalizar(string comentarioFinalizacao)
        {
            Status = Status.finalizado;
            DataFinalizacao = DateTime.UtcNow;
            ComentarioFinalizacao = comentarioFinalizacao;
        }

        public void AlterarMesa(MesaAtendimento novaMesa)
        {
            MesaAtendimento = novaMesa;
            MesaAtendimentoId = novaMesa.Id;
        }

        public void AlterarTecnicoResponsavel(UsuarioSistema usuario)
        {
            TecnicoResponsavelId = usuario.Id;
            TecnicoResponsavel = usuario;
        }

        public void AlterarCategoriaSubCategoria(Categoria categoria, SubCategoria subCategoria)
        {
            CategoriaId = categoria.Id;
            SubCategoriaId = subCategoria.Id;
            SubCategoria = subCategoria;
        }

        public void SlaEmConformidade()
        {
            StatusSLA = StatusSla.emConformidade;
        }

        public void SlaEmRisco()
        {
            StatusSLA = StatusSla.emRisco;
        }

        public void SlaVencido()
        {
            StatusSLA = StatusSla.vencido;
        }
    }
}
