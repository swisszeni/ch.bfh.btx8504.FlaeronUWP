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
    public class DiagnosticReportDetailPageViewModel : ViewModelBase
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

        private DiagnosticReport _currentDiagnosticReport;
        public DiagnosticReport CurrentDiagnosticReport
        {
            get { return _currentDiagnosticReport; }
            set { DispatcherHelper.CheckBeginInvokeOnUI(() => Set(ref _currentDiagnosticReport, value)); }
        }

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
            CurrentDiagnosticReport = await dataService.DiagnosticReportByIdAsync(DiagnosticReportId);

            var test = CurrentDiagnosticReport.Code.Coding;
            // Update all bindings
            RaisePropertyChanged();

            RequestRunning = false;
        }
    }
}
