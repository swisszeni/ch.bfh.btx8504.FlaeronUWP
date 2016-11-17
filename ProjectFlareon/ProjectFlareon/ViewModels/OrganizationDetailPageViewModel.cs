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
    public class OrganizationDetailPageViewModel : ViewModelBase
    {
        private bool _requestRunning;
        public bool RequestRunning
        {
            get { return _requestRunning; }
            set { Set(ref _requestRunning, value); }
        }

        private string _organizationId = "";
        public string OrganizationId
        {
            get { return _organizationId; }
            set { Set(ref _organizationId, value); }
        }

        private Organization _currentOrganization;
        public Organization CurrentOrganization
        {
            get { return _currentOrganization; }
            set { DispatcherHelper.CheckBeginInvokeOnUI(() => Set(ref _currentOrganization, value)); }
        }

        public string Name => CurrentOrganization?.Name;
        public string IdentifierDisplay => CurrentOrganization?.Identifier.FirstOrDefault()?.Type?.Coding.FirstOrDefault()?.Display;

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            OrganizationId = (state.ContainsKey(nameof(OrganizationId))) ? state[nameof(OrganizationId)]?.ToString() : parameter?.ToString();
            LoadDataFromServer();
            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
                state[nameof(OrganizationId)] = OrganizationId;
            }
            await Task.CompletedTask;
        }

        private async void LoadDataFromServer()
        {
            RequestRunning = true;

            IFHIRLabDataService dataService = ServiceLocator.Current.GetInstance<IFHIRLabDataService>();
            CurrentOrganization = await dataService.OrganizationByIdAsync(async (e) =>
            {
                var dialog = new MessageDialog("Requested resource is not available on the server.", "Error");
                var result = await dialog.ShowAsync();
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            }, OrganizationId);

            // Update all bindings
            RaisePropertyChanged("");

            RequestRunning = false;
        }
    }
}
