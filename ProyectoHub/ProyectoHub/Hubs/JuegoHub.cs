using Microsoft.AspNetCore.SignalR;
using MDL;

namespace ProyectoHub.Hubs
{
    public class JuegoHub : Hub
    {
        public async Task SendMessage(GameInfo juegoInfo)
        {
            await Clients.All.SendAsync("ReceiveMessage", juegoInfo);
        }
    }
}
