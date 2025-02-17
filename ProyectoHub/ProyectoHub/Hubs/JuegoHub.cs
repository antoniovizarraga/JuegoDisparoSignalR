using Microsoft.AspNetCore.SignalR;

namespace ProyectoHub.Hubs
{
    public class JuegoHub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
