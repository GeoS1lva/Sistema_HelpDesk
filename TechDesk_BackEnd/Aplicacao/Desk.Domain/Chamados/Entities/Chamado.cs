using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Domain.Chamados.Enums;
using Sistema_HelpDesk.Desk.Domain.Common;
using Sistema_HelpDesk.Desk.Domain.Empresas.Entidades;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;
using System.ComponentModel;

namespace Sistema_HelpDesk.Desk.Domain.Chamados.Entidades
{
    public class Chamado : Entidade
    {
        public string NumeroChamado { get; private set; }

        public string Assunto { get; private set; }
        public string Descricao { get; private set; }
        public int CategoriaId { get; private set; }
        public Prioridade Prioridade { get; private set; }

        public DateTime DataCriacao { get; private set; }

        public Status Status { get; private set; }

        public long TempoAtendimentoTotal { get; private set; }
        public DateTime? DataFinalizacao { get; private set; }
        public string? ComentarioFinalizacao { get; private set; }

        public int EmpresaId { get; private set; }
        public Empresa Empresa { get; set; }

        public int UsuarioEmpresaId { get; private set; }
        public UsuariosEmpresa UsuarioEmpresa { get; set; }

        public int? TecnicoFinalizacaoId { get; private set; }
        public UsuarioSistema TecnicoFinalizacao { get; set; }

        public int? TecnicoAberturaChamadoId { get; set; }
        public UsuarioSistema TecnicoAberturaChamado { get; set; }

        public ICollection<ApontamentoHorasChamado> Apontamentos { get; set; }

        public Chamado(string assunto, string descricao, Categoria categoria, int empresaId, int usuarioEmpresaId)
        {
            NumeroChamado = $"CH{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";

            Assunto = assunto;
            Descricao = descricao;
            CategoriaId = categoria.Id;
            Prioridade = categoria.Prioridade;
            EmpresaId = empresaId;
            UsuarioEmpresaId = usuarioEmpresaId;

            DataCriacao = DateTime.UtcNow;
            Status = Status.aberto;
            TempoAtendimentoTotal = 0;
        }

        public Chamado(string assunto, string descricao, Categoria categoria, int empresaId, int usuarioEmpresaId, int tecnicoAberturaChamadoId)
        {
            NumeroChamado = $"CH{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";

            Assunto = assunto;
            Descricao = descricao;
            CategoriaId = categoria.Id;
            Prioridade = categoria.Prioridade;
            EmpresaId = empresaId;
            UsuarioEmpresaId = usuarioEmpresaId;
            TecnicoAberturaChamadoId = tecnicoAberturaChamadoId;

            DataCriacao = DateTime.UtcNow;
            Status = Status.aberto;
            TempoAtendimentoTotal = 0;
        }

        public Chamado() { }
    }
}
