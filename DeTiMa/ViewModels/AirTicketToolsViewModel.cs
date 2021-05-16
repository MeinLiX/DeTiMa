using System;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using DeTiMa.Models.AirDB;
using DeTiMa.Utils.Commands;

namespace DeTiMa.ViewModels
{
    class AirTicketToolsViewModel : ViewModelBase
    {
        #region Fields

        private DateTime _dateField = DateTime.Now;

        public DateTime DateField
        {
            get => _dateField;
            set => Set(ref _dateField, value);
        }

        private string _companyField;

        public string CompanyField
        {
            get => _companyField;
            set => Set(ref _companyField, value);
        }

        private int? _codeField;

        public int? CodeField
        {
            get => _codeField;
            set => Set(ref _codeField, value);
        }

        private string _airoportOfDepartureField;

        public string AiroportOfDepartureField
        {
            get => _airoportOfDepartureField;
            set => Set(ref _airoportOfDepartureField, value);
        }

        private string _airoportOfArrivalField;

        public string AiroportOfArrivalField
        {
            get => _airoportOfArrivalField;
            set => Set(ref _airoportOfArrivalField, value);
        }

        private TimeSpan _departureTimeField = TimeSpan.Zero;

        public TimeSpan DepartureTimeField
        {
            get => _departureTimeField;
            set => Set(ref _departureTimeField, value);
        }

        private TimeSpan _arrivalTimeField = TimeSpan.Zero;

        public TimeSpan ArrivalTimeField
        {
            get => _arrivalTimeField;
            set => Set(ref _arrivalTimeField, value);
        }

        #endregion

        #region Snackbar

        private string _snackbarMessage;

        public string SnackbarMessage
        {
            get => _snackbarMessage;
            set => Set(ref _snackbarMessage, value);
        }

        private bool _snackbarVisible = false;

        public bool SnackbarVisible
        {
            get => _snackbarVisible;
            set => Set(ref _snackbarVisible, value);
        }

        private async Task ShowSackbar(string message, int showTimeSeconds)
        {
            SnackbarMessage = message;
            SnackbarVisible = true;
            await Task.Delay(showTimeSeconds * 1000);
            SnackbarVisible = false;
            SnackbarMessage = null;
        }

        #endregion

        #region Commands

        #region Create new AirTicket Command
        public ICommand CreateAirTicketCommand { get; }

        private bool CanCreateAirTicketCommandExecute(object p) =>
            CompanyField is not null && CompanyField?.Trim() is not ""
            && CodeField is not null
            && AiroportOfDepartureField is not null && AiroportOfDepartureField?.Trim() is not ""
            && AiroportOfArrivalField is not null && AiroportOfArrivalField?.Trim() is not "";

        private async void OnCreateAirTicketCommandExecute(object p)
        {
            try
            {
                AirTicket newAirTicket = new((int)CodeField, CompanyField, AiroportOfDepartureField, DepartureTimeField, AiroportOfArrivalField, ArrivalTimeField, DateField);

                if (!Validator.TryValidateObject(instance: newAirTicket, validationContext: new ValidationContext(newAirTicket),validationResults: null, validateAllProperties: true))
                {
                    await ShowSackbar("Airoport not valid fields\nExample: Kyiv (IEV)", 2);
                    return;
                }
                else
                {
                    await Utils.DBControllers.Air.MainController.FlyTicketsController.CreateTickets(newAirTicket);

                    await ShowSackbar("AirLine ticket created.", 3);
                }
            }
            catch (Exception ex)
            {
                await ShowSackbar(ex.Message, 3);
            }

        }
        #endregion

        #endregion


        public AirTicketToolsViewModel()
        {
            #region Commands
            CreateAirTicketCommand = new LambdaCommand(OnCreateAirTicketCommandExecute, CanCreateAirTicketCommandExecute);
            #endregion
        }
    }

   /* TODO: VALIDATION FIELDS
      public class AiroportValidationRule : ValidationRule
    {
        public override System.Windows.Controls.ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(Validator.TryValidateValue(value,
                new ValidationContext(value, null, null),
                null,
                typeof(AirTicket)
                    .GetProperty("AirportOfDeparture")
                    .GetCustomAttributes(false)
                    .OfType<ValidationAttribute>()
                    .ToArray()))
            {
                return new System.Windows.Controls.ValidationResult(false, "Airoport isn't valid.\nExample: Kyiv (IEV)");
            }
            else
            {
                return System.Windows.Controls.ValidationResult.ValidResult;
            }
        }
    }*/
}
