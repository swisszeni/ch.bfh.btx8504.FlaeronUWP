using GalaSoft.MvvmLight.Command;
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
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            RefreshListCommand.Execute(this);
            await Task.CompletedTask;
        }

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
            set { Set(ref _diagnosticReports, value); RaisePropertyChanged(() => DiagnosticReports); }
        }

        private RelayCommand _refreshListCommand;
        public RelayCommand RefreshListCommand => _refreshListCommand ?? (_refreshListCommand = new RelayCommand(() =>
        {
            IFHIRLabDataService dataService = ServiceLocator.Current.GetInstance<IFHIRLabDataService>();

            Bundle reports = dataService.DiagnosticReportsForPatient("pat2");
            var resourceList = new List<DiagnosticReport>();
            foreach (var item in reports.Entry)
            {
                resourceList.Add((DiagnosticReport)item.Resource);
            }

            DiagnosticReports = resourceList;
            var test = DiagnosticReports;
        }, () => true));
    }
}
