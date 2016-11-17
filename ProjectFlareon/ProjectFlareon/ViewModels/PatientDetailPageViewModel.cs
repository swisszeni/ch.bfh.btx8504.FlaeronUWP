using GalaSoft.MvvmLight.Threading;
using Hl7.Fhir.Model;
using Microsoft.Practices.ServiceLocation;
using ProjectFlareon.Models;
using ProjectFlareon.Services.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;

namespace ProjectFlareon.ViewModels
{
    public class PatientDetailPageViewModel : ViewModelBase
    {
        private bool _requestRunning;
        public bool RequestRunning
        {
            get { return _requestRunning; }
            set { Set(ref _requestRunning, value); }
        }

        private PatientModel _currentPatient;
        public PatientModel CurrentPatient
        {
            get { return _currentPatient; }
            set { DispatcherHelper.CheckBeginInvokeOnUI(() => Set(ref _currentPatient, value)); }
        }

        public string Name => CurrentPatient?.Name;
        public string FamilyName => CurrentPatient?.FamilyName;
        public string GivenName => CurrentPatient?.GivenName;
        public string Identifier => CurrentPatient?.Identifier;

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            LoadDataFromServer();
            await Task.CompletedTask;
        }

        private async void LoadDataFromServer()
        {
            RequestRunning = true;

            IFHIRLabDataService dataService = ServiceLocator.Current.GetInstance<IFHIRLabDataService>();
            CurrentPatient = await dataService.PatientByIdAsync(async (e) =>
            {
                var dialog = new MessageDialog("Requested resource is not available on the server.", "Error");
                var result = await dialog.ShowAsync();
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            }, Services.SettingsServices.SettingsService.Instance.FhirPatientId);

            // Update all bindings
            RaisePropertyChanged("");

            RequestRunning = false;
        }
    }
}
