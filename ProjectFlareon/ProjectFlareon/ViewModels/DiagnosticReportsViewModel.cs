using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Hl7.Fhir.Model;
using Microsoft.Practices.ServiceLocation;
using ProjectFlareon.Services.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace ProjectFlareon.ViewModels
{
    public class DiagnosticReportsViewModel : ViewModelBase
    {
        private bool _requestRunning;
        public bool RequestRunning
        {
            get { return _requestRunning; }
            set { Set(ref _requestRunning, value); }
        }

        private List<DiagnosticReport> _diagnosticReports;
        public List<DiagnosticReport> DiagnosticReports
        {
            get { return _diagnosticReports; }
            set { DispatcherHelper.CheckBeginInvokeOnUI(() => Set(ref _diagnosticReports, value)); }
        }

        private RelayCommand _refreshListCommand;
        public RelayCommand RefreshListCommand => _refreshListCommand ?? (_refreshListCommand = new RelayCommand(() =>
        {
            LoadDataFromServer();
        }, () => true));

        private RelayCommand<DiagnosticReport> _openDiagnosticReportDetailCommand;
        public RelayCommand<DiagnosticReport> OpenDiagnosticReportDetailCommand => _openDiagnosticReportDetailCommand ?? (_openDiagnosticReportDetailCommand = new RelayCommand<DiagnosticReport>((report) =>
        {
            NavigationService.Navigate(typeof(Views.DiagnosticReportDetailPage), report.Id);
        }, (x) => true));

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            LoadDataFromServer();

            await Task.CompletedTask;
        }

        private async void LoadDataFromServer()
        {
            RequestRunning = true;

            IFHIRLabDataService dataService = ServiceLocator.Current.GetInstance<IFHIRLabDataService>();

            Bundle reports = await dataService.DiagnosticReportsForPatientAsync(Services.SettingsServices.SettingsService.Instance.FhirPatientId);
            var resourceList = new List<DiagnosticReport>();
            foreach (var item in reports.Entry)
            {
                resourceList.Add((DiagnosticReport)item.Resource);
            }

            DiagnosticReports = resourceList;
            RequestRunning = false;
        }
    }
}
