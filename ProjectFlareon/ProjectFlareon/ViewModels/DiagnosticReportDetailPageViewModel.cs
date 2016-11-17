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

        public string DRId => CurrentDiagnosticReport?.Id;
        public string DRVersionId => CurrentDiagnosticReport?.Meta.VersionId;
        public DateTimeOffset? DRIssueDate => CurrentDiagnosticReport?.Issued;
        public string DRCodeDisplay => CurrentDiagnosticReport?.Code.Coding.FirstOrDefault()?.Display;
        public string DRStatus => CurrentDiagnosticReport?.Status.ToString();
        public string DRPerformerName {
            get {
                return CurrentDiagnosticReport?.Performer.Display != null ? CurrentDiagnosticReport.Performer.Display : "Performer missing";
            }
            
        }
        public string DRSubjectName => CurrentDiagnosticReport?.Subject.Display;

        public DateTimeOffset? DREffectiveDate
        {
            get
            {
                DateTimeOffset? effectiveDate = null;
                var effective = CurrentDiagnosticReport?.Effective;
                if(effective?.TypeName == "dateTime")
                {
                    var effectiveInstant = (FhirDateTime)effective;
                    effectiveDate = effectiveInstant.ToDateTimeOffset();
                }
                return effectiveDate;
            }
        }

        private List<ObservationModel> _drResult;
        public List<ObservationModel> DRResult
        {
            get { return _drResult; }
            set { Set(ref _drResult, value); }
        }
        public string DRConclusion => CurrentDiagnosticReport?.Conclusion;

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

        private RelayCommand _openPerformerDetailCommand;
        public RelayCommand OpenPerformerDetailCommand => _openPerformerDetailCommand ?? (_openPerformerDetailCommand = new RelayCommand(() =>
        {
            var performerReference = CurrentDiagnosticReport.Performer.Reference;
            // Hacky hack to know where to go next...
            if (performerReference.Contains("Organization/"))
            {
                NavigationService.Navigate(typeof(Views.OrganizationDetailPage), ExtractIdFromUri(performerReference, "Organization/"));
            } else if(performerReference.Contains("Practitioner/"))
            {
                NavigationService.Navigate(typeof(Views.PractitionerDetailPage), ExtractIdFromUri(performerReference, "Practitioner/"));
            }
            
        }, () => true));

        private RelayCommand _openReportHistoryCommand;
        public RelayCommand OpenReportHistoryCommand => _openReportHistoryCommand ?? (_openReportHistoryCommand = new RelayCommand(() =>
        {
            NavigationService.Navigate(typeof(Views.DiagnosticReportHistoryPage), DiagnosticReportId);
        }, () => true));

        private RelayCommand<Observation> _openObservationDetailCommand;
        public RelayCommand<Observation> OpenObservationDetailCommand => _openObservationDetailCommand ?? (_openObservationDetailCommand = new RelayCommand<Observation>((obs) =>
        {
            // NavigationService.Navigate(typeof(Views.ObservationDetailPage), DiagnosticReportId);
        }, (x) => true));

        private async void LoadDataFromServer()
        {
            RequestRunning = true;

            IFHIRLabDataService dataService = ServiceLocator.Current.GetInstance<IFHIRLabDataService>();
            Action<Exception> errorAction = async (e) =>
            {
                var dialog = new MessageDialog("Requested resource is not available on the server.", "Error");
                var result = await dialog.ShowAsync();
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            };

            CurrentDiagnosticReport = await dataService.DiagnosticReportByIdAsync(errorAction, DiagnosticReportId);

            List<ObservationModel> observations = new List<ObservationModel>();
            foreach (var reference in CurrentDiagnosticReport.Result)
            {
                if(reference.IsContainedReference)
                {
                    var observation = CurrentDiagnosticReport.Contained.Where((x) => (x.Id == reference.Reference.TrimStart('#'))).FirstOrDefault();
                    if(observation != null)
                    {
                        observations.Add(new ObservationModel((Observation)observation));
                    }
                } else if(reference.Reference != null)
                {
                    
                    // Hacky hack to know where to go next...
                    if (reference.Reference.Contains("Observation/"))
                    {
                        var observationUri = ExtractIdFromUri(reference.Reference, "Observation/");
                        var observation = await dataService.ObservationByIdAsync(errorAction, observationUri);
                        if(observation == null)
                        {
                            break;
                        }
                        observations.Add(new ObservationModel(observation));
                    }
                }
            }
            DRResult = observations;

            // Update all bindings
            RaisePropertyChanged("");

            RequestRunning = false;
        }

        private string ExtractIdFromUri(string uri, string resourceString)
        {
            return uri.Substring(uri.IndexOf(resourceString) + resourceString.Length);
        }
    }
}
