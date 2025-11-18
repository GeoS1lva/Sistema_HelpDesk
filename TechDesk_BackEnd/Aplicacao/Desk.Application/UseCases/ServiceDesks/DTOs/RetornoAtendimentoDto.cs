using Sistema_HelpDesk.Desk.Domain.Chamados.Enums;

namespace Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.DTOs
{
    public class RetornoAtendimentoDto
    {
        public string NumeroChamado { get; set; }
        public RetornoAcaoChamaoEnum RetornoAcao { get; set; }
        public string UsuarioNome { get; set; }
        public DateTime HoraAtendimento { get; set; }
        public string Mensagem { get; set; }
    }
}
