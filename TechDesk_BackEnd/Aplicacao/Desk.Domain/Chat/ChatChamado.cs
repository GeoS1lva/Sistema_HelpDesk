using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;
using Sistema_HelpDesk.Desk.Domain.Users.Enum;

namespace Sistema_HelpDesk.Desk.Domain.Chat
{
    public class ChatChamado
    {
        public int Id { get; set; }

        public int ChamadoId { get; set; }
        public Chamado Chamado { get; set; }
        public string NumeroChamado { get; set; }

        public string Nome { get; set; }
        public TipoPerfil TipoPerfil { get; set; }
        public string Mensagem { get; set; }
        public DateTime HoraEnvio { get; set; }

        public ChatChamado(Chamado chamado, string numeroChamado, string nome, string mensagem)
        {
            ChamadoId = chamado.Id;
            NumeroChamado = chamado.NumeroChamado;
            NumeroChamado = numeroChamado;
            Nome = nome;
            mensagem = mensagem;
            HoraEnvio = DateTime.UtcNow;
        }

        public ChatChamado()
        {
        }
    }
}
