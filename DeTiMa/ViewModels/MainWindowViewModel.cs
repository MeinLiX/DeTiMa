using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

using DeTiMa.Models.AirDB;
using DeTiMa.Utils.Commands;
using DeTiMa.Views.Windows;

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
                SelectedTicket = null;
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

        #region Delete selected AirTicket Command
        public ICommand DeleteSelctedAirTicketCommand { get; }

        private bool CanDeleteSelctedAirTicketCommandExecute(object p) => SelectedTicket is not null;

        private async void OnDeleteSelctedAirTicketCommandExecute(object p)
        {
            try
            {
                await Utils.DBControllers.Air.MainController.FlyTicketsController.DeleteByPK(SelectedTicket);

                if (UpdateTicketsCommand.CanExecute(null))
                    UpdateTicketsCommand.Execute(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }
        #endregion

        #region Open AirTicket tools window Command
        public ICommand OpenAirTicketToolsWindowCommand { get; }

        private bool CanOpenAirTicketToolsWindowCommandExecute(object p) => true;

        private void OnOpenAirTicketToolsWindowCommandExecute(object p)
        {
            try
            {
                new AirTicketToolsWindow().ShowDialog();

                if (UpdateTicketsCommand.CanExecute(null))
                    UpdateTicketsCommand.Execute(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }
        #endregion

        #region Select Most Popular Company Command
        public ICommand SelectMostPopularCompanyCommand { get; }

        private bool CanSelectMostPopularCompanyCommandExecute(object p) => GetTickets.Count>0;

        private void OnSelectMostPopularCompanyExecute(object p)
        {
            try
            {
                string outmsg = string.Empty;
                var MostPopular=GetTickets
                    .GroupBy(e=>e.Company)
                    .OrderByDescending(e => e.Count())
                    .Take(5)
                    .ToList();

                MostPopular.ForEach(i => outmsg += $"Company: { i.Key}, Airport: " +
                $"{GetTickets.Where(e=>e.Company==i.Key).GroupBy(e=>e.AirportOfDeparture).OrderByDescending(e=>e.Count()).FirstOrDefault().Key}, Time: " +
                $"{GetTickets.Where(e => e.Company == i.Key).GroupBy(e => e.DepartureTime).OrderByDescending(e => e.Count()).FirstOrDefault().Key}\n");
                
                MessageBox.Show(outmsg, "Most Popular Company");
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
            DeleteSelctedAirTicketCommand = new LambdaCommand(OnDeleteSelctedAirTicketCommandExecute, CanDeleteSelctedAirTicketCommandExecute);
            OpenAirTicketToolsWindowCommand = new LambdaCommand(OnOpenAirTicketToolsWindowCommandExecute, CanOpenAirTicketToolsWindowCommandExecute);
            SelectMostPopularCompanyCommand = new LambdaCommand(OnSelectMostPopularCompanyExecute, CanSelectMostPopularCompanyCommandExecute);
            #endregion

            if (UpdateTicketsCommand.CanExecute(null))
                UpdateTicketsCommand.Execute(null);
        }
    }
}
