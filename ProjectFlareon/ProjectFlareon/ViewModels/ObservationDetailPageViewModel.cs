using Microsoft.Practices.ServiceLocation;
using ProjectFlareon.Services.DataServices;
using ProjectFlareon.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace ProjectFlareon.ViewModels
{
    public class ObservationDetailPageViewModel : ViewModelBase
    {
        private bool _requestRunning;
        public bool RequestRunning
        {
            get { return _requestRunning; }
            set { Set(ref _requestRunning, value); }
        }

        private string _observationId = "";
        public string ObservationId
        {
            get { return _observationId; }
            set { Set(ref _observationId, value); }
        }

        private string _observationCode;
        public string ObservationCode
        {
            get { return _observationCode; }
            set { Set(ref _observationCode, value); }
        }

        private string _observationCodeDisplay;
        public string ObservationCodeDisplay
        {
            get { return _observationCodeDisplay; }
            set { Set(ref _observationCodeDisplay, value); }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            ObservationId = (state.ContainsKey(nameof(ObservationId))) ? state[nameof(ObservationId)]?.ToString() :((ObservationDetailPagePayload)parameter)?.ObservationId;
            ObservationCode = (state.ContainsKey(nameof(ObservationCode))) ? state[nameof(ObservationCode)]?.ToString() :((ObservationDetailPagePayload)parameter)?.ObservationCode;
            ObservationCodeDisplay = (state.ContainsKey(nameof(ObservationCodeDisplay))) ? state[nameof(ObservationCodeDisplay)]?.ToString() :((ObservationDetailPagePayload)parameter)?.ObservationCodeDisplay;

            LoadDataFromServer();
            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
                state[nameof(ObservationId)] = ObservationId;
                state[nameof(ObservationCode)] = ObservationCode;
                state[nameof(ObservationCodeDisplay)] = ObservationCodeDisplay;
            }
            await Task.CompletedTask;
        }

        private async void LoadDataFromServer()
        {
            RequestRunning = true;

            IFHIRLabDataService dataService = ServiceLocator.Current.GetInstance<IFHIRLabDataService>();

            //CurrentDiagnosticReport = await dataService.DiagnosticReportByIdAsync(async (e) =>
            //{
            //    var dialog = new MessageDialog("Requested resource is not available on the server.", "Error");
            //    var result = await dialog.ShowAsync();
            //    if (NavigationService.CanGoBack)
            //    {
            //        NavigationService.GoBack();
            //    }
            //}, DiagnosticReportId);

            //List<Observation> observations = new List<Observation>();
            //foreach (var reference in CurrentDiagnosticReport.Result)
            //{
            //    if (reference.IsContainedReference)
            //    {
            //        var observation = CurrentDiagnosticReport.Contained.Where((x) => (x.Id == reference.Reference.TrimStart('#'))).FirstOrDefault();
            //        if (observation != null)
            //        {
            //            observations.Add((Observation)observation);
            //        }
            //    }
            //}
            //DRResult = observations;

            // Update all bindings
            RaisePropertyChanged("");

            RequestRunning = false;
        }
    }
}
