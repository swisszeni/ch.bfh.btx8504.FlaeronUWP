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
    public class DiagnosticReportHistoryPageViewModel : ViewModelBase
    {
        private bool _requestRunning;
        public bool RequestRunning
        {
            get { return _requestRunning; }
            set { Set(ref _requestRunning, value); }
        }

        private string _diagnosticReportId = "";
        public string DiagnosticReportId
        {
            get { return _diagnosticReportId; }
            set { Set(ref _diagnosticReportId, value); }
        }

        private List<DiagnosicReportServerRequestModel> _diagnosticReportVersions;
        public List<DiagnosicReportServerRequestModel> DiagnosticReportVersions
        {
            get { return _diagnosticReportVersions; }
            set { DispatcherHelper.CheckBeginInvokeOnUI(() => Set(ref _diagnosticReportVersions, value)); }
        }

        //private RelayCommand _refreshListCommand;
        //public RelayCommand RefreshListCommand => _refreshListCommand ?? (_refreshListCommand = new RelayCommand(() =>
        //{
        //    LoadDataFromServer();
        //}, () => true));

        //private RelayCommand<DiagnosticReport> _openDiagnosticReportDetailCommand;
        //public RelayCommand<DiagnosticReport> OpenDiagnosticReportDetailCommand => _openDiagnosticReportDetailCommand ?? (_openDiagnosticReportDetailCommand = new RelayCommand<DiagnosticReport>((report) =>
        //{
        //    NavigationService.Navigate(typeof(Views.DiagnosticReportDetailPage), report.Id);
        //}, (x) => true));

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            DiagnosticReportId = (state.ContainsKey(nameof(DiagnosticReportId))) ? state[nameof(DiagnosticReportId)]?.ToString() : parameter?.ToString();
            LoadDataFromServer();

            await Task.CompletedTask;
        }
        public override async Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
                state[nameof(DiagnosticReportId)] = DiagnosticReportId;
            }
            await Task.CompletedTask;
        }

        private async void LoadDataFromServer()
        {
            RequestRunning = true;

            IFHIRLabDataService dataService = ServiceLocator.Current.GetInstance<IFHIRLabDataService>();

            Bundle reports = await dataService.DiagnosticReportHistoryByIdAsync(async (e) =>
            {
                var dialog = new MessageDialog("Requested resource is not available on the server.", "Error");
                var result = await dialog.ShowAsync();
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            }, DiagnosticReportId);

            var resourceList = new List<DiagnosicReportServerRequestModel>();
            foreach (var item in reports.Entry)
            {
                resourceList.Add(new DiagnosicReportServerRequestModel(item));
            }

            DiagnosticReportVersions = resourceList;

            RequestRunning = false;
        }
    }
}
