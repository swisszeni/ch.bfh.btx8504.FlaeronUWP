using GalaSoft.MvvmLight.Command;
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
    public class DiagnosticReportsViewModel : ViewModelBase
    {
        private bool _requestRunning;
        public bool RequestRunning
        {
            get { return _requestRunning; }
            set { Set(ref _requestRunning, value); }
        }

        private List<DiagnosticReportModel> _diagnosticReports;
        public List<DiagnosticReportModel> DiagnosticReports
        {
            get { return _diagnosticReports; }
            set { DispatcherHelper.CheckBeginInvokeOnUI(() => Set(ref _diagnosticReports, value)); }
        }

        private RelayCommand _refreshListCommand;
        public RelayCommand RefreshListCommand => _refreshListCommand ?? (_refreshListCommand = new RelayCommand(() =>
        {
            LoadDataFromServer();
        }, () => true));

        private RelayCommand<DiagnosticReportModel> _openDiagnosticReportDetailCommand;
        public RelayCommand<DiagnosticReportModel> OpenDiagnosticReportDetailCommand => _openDiagnosticReportDetailCommand ?? (_openDiagnosticReportDetailCommand = new RelayCommand<DiagnosticReportModel>((report) =>
        {
            NavigationService.Navigate(typeof(Views.DiagnosticReportDetailPage), report.DiagnosticReportId);
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

            Bundle reports = await dataService.DiagnosticReportsForPatientAsync(async (e) =>
            {
                var dialog = new MessageDialog("Requested resource is not available on the server.", "Error");
                var result = await dialog.ShowAsync();
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            }, Services.SettingsServices.SettingsService.Instance.FhirPatientId, Hl7.Fhir.Rest.SummaryType.True);
            var resourceList = new List<DiagnosticReportModel>();
            foreach (var item in reports?.Entry)
            {
                resourceList.Add(new DiagnosticReportModel((DiagnosticReport)item.Resource));
            }

            DiagnosticReports = resourceList;
            RequestRunning = false;
        }
    }
}
