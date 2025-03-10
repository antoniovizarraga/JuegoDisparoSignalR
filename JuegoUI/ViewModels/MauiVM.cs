﻿using _18_CRUD_Personas_UWP_UI.ViewModels.Utilidades;
using MDL;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JuegoUI.ViewModels
{
    class MauiVM : IQueryAttributable, INotifyPropertyChanged
    {
        private HubConnection _hubConnection;
        private GameInfo infoJuego;
        private string ganadorNombre = "";
        private string textoPartida = "Esperando jugadores...";
        private string resultadoPartida = "";
        private bool hasShooted = true;
        private bool alguienGano = false;
        private DateTime _startTime;
        private const int _timeLimit = 5000; // Tiempo para mostrar la palabra (en ms)
        private DelegateCommand disparoCommand;




        // Comando que se ejecuta cuando se hace clic en el botón
        public DelegateCommand DisparoCommand
        {
            get { return disparoCommand; }
        }

        public GameInfo InfoJuego
        {
            get { return infoJuego; }
            set {
                infoJuego.NombreGanador = value.NombreGanador;
                NotifyPropertyChanged("infoJuego");
            }
        }

        public string TextoPartida
        {
            get { return textoPartida; }
            set { textoPartida = value; }
        }

        public bool AlguienGano
        {
            get { return alguienGano; }
            set { alguienGano = value; }
        }



        // Configurar la conexión al Hub
        private void InitializeHub()
        {
            _hubConnection = new HubConnectionBuilder().WithUrl("http://localhost:5073/juegohub").Build();

            // Recibir la palabra para mostrarla en la interfaz
            _hubConnection.On<string>("ReceiveWord", (word) =>
            {
                // Mostrar la palabra en la UI
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    textoPartida = word;
                    NotifyPropertyChanged("TextoPartida");
                    //_startTime = DateTime.Now;
                });
            });

            // Recibir la notificación del ganador
            _hubConnection.On<string>("GameOver", (nombreGanador) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    textoPartida = $"¡{nombreGanador} ha ganado!";
                    alguienGano = true;
                    NotifyPropertyChanged("TextoPartida");
                    DisparoCommand.RaiseCanExecuteChanged();
                    ganadorNombre = nombreGanador;
                    mostrarAlerta();

                });
            });

            // Iniciar la conexión
            _hubConnection.StartAsync();
        }

        private async void mostrarAlerta()
        {
            await Application.Current.MainPage.DisplayAlert("¡Ganó " + ganadorNombre + "!", ganadorNombre + "ganó la partida.", "OK");

            await _hubConnection.InvokeAsync("DisconnectPlayer");
            alguienGano = false;
            hasShooted = true;

            disparoCommand.RaiseCanExecuteChanged();
            await Shell.Current.GoToAsync("//MainPage");
        }

        public MauiVM()
        {
            // Inicializamos el comando con la acción y la validación (canExecute)
            disparoCommand = new DelegateCommand(DisparoCommand_Executed, DisparoCommand_CanExecute);
        }

        // Método para iniciar el juego
        private async void EmpezarJuego(bool haEmpezado)
        {
            if (!haEmpezado)
            {
                // Mostrar el mensaje de que ambos jugadores están listos
                textoPartida = "Esperando al otro jugador...";
                NotifyPropertyChanged("TextoPartida");

            }
            else
            {

                // Aquí es donde el cliente controla el flujo de la partida
                // Después de que los jugadores estén conectados y listos, se puede empezar el juego




                await _hubConnection.SendAsync("SendWordToPlayers", "¡Disparen!");

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    hasShooted = false;
                    disparoCommand.RaiseCanExecuteChanged();
                });


            }


        }

        private async void PrepareGame(GameInfo infoPartida)
        {
            _hubConnection.On<bool>("StartGame", EmpezarJuego);
            await _hubConnection.InvokeAsync("ConnectPlayer", infoPartida);
            

            
        }

       

        private async Task NotificarAsincrono()
        {
            await _hubConnection.SendAsync("NotifyWinner", infoJuego);
        }

        private void DisparoCommand_Executed()
        {

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    hasShooted = true;
                    disparoCommand.RaiseCanExecuteChanged();
                });


                NotificarAsincrono();
        }

        private bool DisparoCommand_CanExecute()
        {
            bool res = false;

            if (!hasShooted && !alguienGano)
            {


                res = true;
            }

            return res;

        }



        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {

            infoJuego = query["Partida"] as GameInfo;

            InitializeHub();

            PrepareGame(infoJuego);


        }

        #region Notify
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName = "")

        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        #endregion
    }
}
