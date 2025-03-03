using _18_CRUD_Personas_UWP_UI.ViewModels.Utilidades;
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
            await Shell.Current.GoToAsync("//ShootPage");
        }


        private void ConnectCommand_Executed()
        {
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
