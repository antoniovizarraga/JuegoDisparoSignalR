using Microsoft.AspNetCore.SignalR;
using MDL;
using System.Runtime.CompilerServices;

namespace ProyectoHub.Hubs
{
    public class JuegoHub : Hub
    {

        private static bool isFirst = false;

        private static bool hayDosJugadores = false;

        private static Dictionary<string, GameInfo> jugadores = new();

        // Enviar la palabra a ambos jugadores
        public async Task SendWordToPlayers(string word)
        {


            await Clients.All.SendAsync("ReceiveWord", word);
        }

        public async Task ConnectPlayer(GameInfo infoJuego)
        {
            jugadores[Context.ConnectionId] = infoJuego;

            if (jugadores.Count >= 2)
            {
                hayDosJugadores = true;
            }
            // Revisar esta línea de código
            await Clients.All.SendAsync("StartGame", hayDosJugadores);

        }

        public async Task DisconnectPlayer()
        {
            jugadores.Remove(Context.ConnectionId);

        }

        // Notificar quién ha ganado
        public async Task NotifyWinner(GameInfo infoJuego)
        {
            if(!isFirst)
            {
                isFirst = true;

                await Clients.All.SendAsync("GameOver", infoJuego.NombreGanador);
            }
            
        }

        public async Task GamePrepare()
        {

        }

        /* public static GameInfo elegirGanador()
        {
            

            return estadoJuego;
        } */
    }
}
