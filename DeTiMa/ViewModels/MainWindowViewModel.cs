using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

using DeTiMa.Models.AirDB;
using DeTiMa.Utils.Commands;


namespace DeTiMa.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        #region Actual tickets list
        private List<AirTicket> _Tickets = new();

        public List<AirTicket> GetTickets
        {
            get => _Tickets;
            private set => Set(ref _Tickets, value);
        }
        #endregion

        #region Selected ticket from the list
        private AirTicket _selectedTicket = null;

        public AirTicket SelectedTicket
        {
            get => _selectedTicket;
            set => Set(ref _selectedTicket, value);
        }
        #endregion

        #region commands

        #region Update tickets Command
        public ICommand UpdateTicketsCommand { get; }

        private bool CanUpdateTicketsCommandExecute(object p) => true;

        private async void OnUpdateTicketsCommandExecute(object p)
        {
            try
            {
                GetTickets = await Utils.DBControllers.Air.MainController.FlyTicketsController.GetTickets();
            }
            catch
            {
                GetTickets = new()
                {
                    new AirTicket()
                };
            }

        }
        #endregion

        #region Generate random AirTicket Command
        public ICommand GenerateRandomAirTicketCommand { get; }

        private bool CanGenerateRandomAirTicketCommandExecute(object p) => true;

        private async void OnGenerateRandomAirTicketCommandExecute(object p)
        {
            try
            {
                await Utils.DBControllers.Air.MainController.FlyTicketsController.GenerateAndCreateTickets(1);

                if (UpdateTicketsCommand.CanExecute(null))
                    UpdateTicketsCommand.Execute(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }
        #endregion

        #endregion

        public MainWindowViewModel()
        {
            #region Commands
            UpdateTicketsCommand = new LambdaCommand(OnUpdateTicketsCommandExecute, CanUpdateTicketsCommandExecute);
            GenerateRandomAirTicketCommand = new LambdaCommand(OnGenerateRandomAirTicketCommandExecute, CanGenerateRandomAirTicketCommandExecute);
            #endregion

            if (UpdateTicketsCommand.CanExecute(null))
                UpdateTicketsCommand.Execute(null);
        }
    }
}
