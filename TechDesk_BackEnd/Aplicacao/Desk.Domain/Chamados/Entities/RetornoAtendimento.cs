using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;
using Sistema_HelpDesk.Desk.Domain.Chamados.Enums;

namespace Sistema_HelpDesk.Desk.Domain.Chamados.Entities
{
    public class RetornoAtendimento
    {
        public int Id { get; set; }

        public int ChamadoId { get; set; }
        public Chamado Chamado { get; set; }

        public string NumeroChamado { get; set; }
        public RetornoAcaoChamaoEnum RetornoAcao { get; set; }

        public string NomeTecnico { get; set; }
        public DateTime HoraAtendimento { get; set; }
        public string Mensagem { get; set; }

        public RetornoAtendimento(Chamado chamado, RetornoAcaoChamaoEnum retornoAcao, string nomeTecnico, string mensagem)
        {
            ChamadoId = chamado.Id;
            NumeroChamado = chamado.NumeroChamado;
            RetornoAcao = retornoAcao;
            NomeTecnico = nomeTecnico;
            HoraAtendimento = DateTime.Now;
            Mensagem = mensagem;
        }

        public RetornoAtendimento()
        {
        }
    }
}
