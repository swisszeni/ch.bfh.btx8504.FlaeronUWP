using GalaSoft.MvvmLight.Threading;
using Hl7.Fhir.Model;
using Microsoft.Practices.ServiceLocation;
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
    public class PractitionerDetailPageViewModel : ViewModelBase
    {
        private bool _requestRunning;
        public bool RequestRunning
        {
            get { return _requestRunning; }
            set { Set(ref _requestRunning, value); }
        }

        private string _practitionerId = "";
        public string PractitionerId
        {
            get { return _practitionerId; }
            set { Set(ref _practitionerId, value); }
        }

        private Practitioner _currentPractitioner;
        public Practitioner CurrentPractitioner
        {
            get { return _currentPractitioner; }
            set { DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                if (Set(ref _currentPractitioner, value))
                {
                    if (CurrentPractitioner != null)
                    {
                        var prefixes = String.Join(" ", CurrentPractitioner.Name.Prefix);
                        var givenName = String.Join(" ", CurrentPractitioner.Name.Given);
                        var familyName = String.Join(" ", CurrentPractitioner.Name.Family);

                        Name = $"{prefixes} {givenName} {familyName}";
                    }
                }
            }); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            PractitionerId = (state.ContainsKey(nameof(PractitionerId))) ? state[nameof(PractitionerId)]?.ToString() : parameter?.ToString();
            LoadDataFromServer();
            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
                state[nameof(PractitionerId)] = PractitionerId;
            }
            await Task.CompletedTask;
        }

        private async void LoadDataFromServer()
        {
            RequestRunning = true;

            IFHIRLabDataService dataService = ServiceLocator.Current.GetInstance<IFHIRLabDataService>();
            CurrentPractitioner = await dataService.PractitionerByIdAsync(async (e) =>
            {
                var dialog = new MessageDialog("Requested resource is not available on the server.", "Error");
                var result = await dialog.ShowAsync();
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            }, PractitionerId);

            // Update all bindings
            RaisePropertyChanged("");

            RequestRunning = false;
        }
    }
}
