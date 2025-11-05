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
        public string Assunto { get; private set; }
        public string Descricao { get; private set; }
        public int CategoriaId { get; private set; }
        public Status Status { get; private set; }
        public long TempoAtendimentoTotal { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime DataFinalizacao { get; private set; }
        public string ComentarioFinalizacao { get; private set; }

        public int EmpresaId { get; private set; }
        public Empresa Empresa { get; set; }

        public int UsuarioEmpresaId { get; private set; }
        public UsuariosEmpresa UsuarioEmpresa { get; set; }

        public int TecnicoFinalizacaoId { get; private set; }
        public UsuarioSistema TecnicoFinalizacao { get; set; }

        public ICollection<ApontamentoHorasChamado> Apontamentos { get; set; }


        public Chamado(string assunto, string descricao, int categoriaId, int empresaId, int usuarioEmpresaId)
        {
            Assunto = assunto;
            Descricao = descricao;
            CategoriaId = categoriaId;
            Status = Status.aberto;
            EmpresaId = empresaId;
            UsuarioEmpresaId = usuarioEmpresaId;
            DataCriacao = DateTime.Now;
        }

        public ResultModel FinalizarChamado(int tecnicoId, string comentarioFinalizacao)
        {
            if (Status == Status.aberto)
                return ResultModel.Erro("Chamado Não Iniciado");

            if (Status == Status.finalizado)
                return ResultModel.Erro("Chamado Já Finalizado");

            Status = Status.finalizado;
            TecnicoFinalizacaoId = tecnicoId;
            ComentarioFinalizacao = comentarioFinalizacao;
            DataFinalizacao = DateTime.Now;

            return ResultModel.Sucesso();
        }

        public ResultModel<TimeSpan> ApontarHorasChamado(long apontamento)
        {
            TempoAtendimentoTotal += apontamento;

            return ResultModel<TimeSpan>.Sucesso(TimeSpan.FromTicks(TempoAtendimentoTotal));
        }
    }
}
