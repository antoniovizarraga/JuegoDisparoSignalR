using Microsoft.AspNetCore.SignalR;
using MDL;

namespace ProyectoHub.Hubs
{
    public class JuegoHub : Hub
    {
        private static GameInfo estadoJuego;

        private static bool primeroEnLlegar;

        public async Task SendMessage(GameInfo juegoInfo)
        {
            await Clients.All.SendAsync("ReceiveMessage", juegoInfo);
        }

        public static GameInfo elegirGanador()
        {
            

            return estadoJuego;
        }
    }
}
