using _18_CRUD_Personas_UWP_UI.ViewModels.Utilidades;
using MDL;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuegoUI.ViewModels
{
    class MauiVM
    {
        private HubConnection _hubConnection;
        private GameInfo infoJuego;
        private string estadoPartida = "";
        private string textoPartida = "";
        private string resultadoPartida = "";
        private bool hasShooted = false;
        private DateTime _startTime;
        private const int _timeLimit = 5000; // Tiempo para mostrar la palabra (en ms)
        private DelegateCommand disparoCommand;




        // Comando que se ejecuta cuando se hace clic en el botón
        public DelegateCommand CasillaCommand
        {
            get { return disparoCommand; }
        }

        public GameInfo InfoJuego
        {
            get { return infoJuego; }
            set { infoJuego.NombreGanador = value.NombreGanador; }
        }



        // Configurar la conexión al Hub
        private void InitializeHub()
        {
            _hubConnection = new HubConnectionBuilder().WithUrl("").Build();

            // Recibir la palabra para mostrarla en la interfaz
            _hubConnection.On<string>("ReceiveWord", (word) =>
            {
                // Mostrar la palabra en la UI
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    textoPartida = word;
                    _startTime = DateTime.Now;
                });
            });

            // Recibir la notificación del ganador
            _hubConnection.On<string>("GameOver", (nombreGanador) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    resultadoPartida = $"{nombreGanador} ha ganado!";
                });
            });

            // Iniciar la conexión
            _hubConnection.StartAsync();
        }

        public MauiVM(GameInfo infoPartida)
        {

            // Inicializamos el comando con la acción y la validación (canExecute)
            disparoCommand = new DelegateCommand(DisparoCommand_Executed, DisparoCommand_CanExecute);

            infoJuego.NombreGanador = infoPartida.NombreGanador;
            InitializeHub();
        }

        // Método para iniciar el juego
        private async void StartGame()
        {
            // Mostrar el mensaje de que ambos jugadores están listos
            estadoPartida = "Esperando al otro jugador...";

            // Aquí es donde el cliente controla el flujo de la partida
            // Después de que los jugadores estén conectados y listos, se puede empezar el juego



            await _hubConnection.SendAsync("SendWordToPlayers", "¡Disparen!");
        }

        // Evento cuando la página se carga y los jugadores se conectan
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await StartGame(); // Inicia el juego cuando la página aparece
        }

        private async Task NotificarAsincrono()
        {
            await _hubConnection.SendAsync("NotifyWinner", infoJuego);
        }

        private void DisparoCommand_Executed()
        {
            var reactionTime = DateTime.Now - _startTime;

            // Enviar al servidor quien ha ganado
            if (reactionTime.TotalMilliseconds < _timeLimit)
            {
                hasShooted = true;
                NotificarAsincrono();
            }
        }

        private bool DisparoCommand_CanExecute()
        {
            bool res = false;

            if (!hasShooted)
            {


                res = true;
            }

            return res;

        }
    }
}
