using Microsoft.AspNetCore.SignalR;
using MDL;

namespace ProyectoHub.Hubs
{
    public class JuegoHub : Hub
    {

        private static bool isFirst;

        // Enviar la palabra a ambos jugadores
        public async Task SendWordToPlayers(string word)
        {
            await Clients.All.SendAsync("ReceiveWord", word);
        }

        // Notificar quién ha ganado
        public async Task NotifyWinner(GameInfo infoJuego)
        {
            if(!isFirst)
            {
                isFirst = true;

                await Clients.All.SendAsync("GameOver", infoJuego);
            }
            
        }

        /* public static GameInfo elegirGanador()
        {
            

            return estadoJuego;
        } */
    }
}
