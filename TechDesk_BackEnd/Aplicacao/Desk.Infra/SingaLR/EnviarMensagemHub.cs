using Microsoft.AspNetCore.SignalR;

namespace Sistema_HelpDesk.Desk.Infra.SingaLR
{
    public class EnviarMensagemHub : Hub
    {
        public async Task EnviarMensagem(string user, string message)
        {
            await Clients.All.SendAsync("ReceberMensagem", user, message);
        }
    }
}
