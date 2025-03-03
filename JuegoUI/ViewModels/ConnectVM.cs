using _18_CRUD_Personas_UWP_UI.ViewModels.Utilidades;
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
    class ConnectVM : INotifyPropertyChanged
    {


        private string nombreJugador = "";

        private DelegateCommand connectCommand;

        private GameInfo partidaJugador = new GameInfo();





        public string NombreJugador
        {
            get { return nombreJugador; }
            set
            {
                nombreJugador = value;
                connectCommand.RaiseCanExecuteChanged();

            }
        }

        public DelegateCommand ConnectCommand
        {
            get { return connectCommand; }
        }

        public ConnectVM()
        {
            connectCommand = new DelegateCommand(ConnectCommand_Executed, ConnectCommand_CanExecute);
        }

        private async void cambioPagina()
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "Partida", partidaJugador }
            };

            await Shell.Current.GoToAsync("//ShootPage", navigationParameter);
        }


        private void ConnectCommand_Executed()
        {
            partidaJugador.NombreGanador = nombreJugador;



            cambioPagina();
        }

        private bool ConnectCommand_CanExecute()
        {
            bool res = false;

            if (!string.IsNullOrEmpty(nombreJugador))
            {


                res = true;
            }

            return res;
        }


        #region Notify
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")

        {

            PropertyChanged?.Invoke(this, new
            PropertyChangedEventArgs(propertyName));

        }
        #endregion


    }
}
