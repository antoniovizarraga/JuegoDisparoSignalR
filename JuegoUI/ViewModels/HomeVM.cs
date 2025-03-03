using _18_CRUD_Personas_UWP_UI.ViewModels.Utilidades;
using JuegoUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuegoUI.ViewModels
{
    class HomeVM
    {
        private DelegateCommand playCommand;


        public DelegateCommand PlayCommand
        {
            get { return playCommand; }
        }

        public HomeVM()
        {
            playCommand = new DelegateCommand(PlayCommand_Executed);
        }

        private async void cambioPagina()
        {
            await Shell.Current.GoToAsync("//GamePage");
        }


        private void PlayCommand_Executed()
        {
            cambioPagina();
        }
    }
}
