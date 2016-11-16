using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Hl7.Fhir.Model;
using Microsoft.Practices.ServiceLocation;
using ProjectFlareon.Controls;
using ProjectFlareon.Models;
using ProjectFlareon.Services.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Controls;
using Template10.Mvvm;
using Template10.Services.SettingsService;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace ProjectFlareon.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public SettingsPartViewModel SettingsPartViewModel { get; } = new SettingsPartViewModel();
        public AboutPartViewModel AboutPartViewModel { get; } = new AboutPartViewModel();
    }

    public class SettingsPartViewModel : ViewModelBase
    {
        Services.SettingsServices.SettingsService _settings;

        public SettingsPartViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                // designtime
            }
            else
            {
                _settings = Services.SettingsServices.SettingsService.Instance;
            }
        }

        public string ServerUri
        {
            get { return _settings.FhirServerUri; }
            set
            {
                _settings.FhirServerUri = value;
                RaisePropertyChanged(() => ServerUri);
            }
        }

        public string PatientId
        {
            get { return _settings.FhirPatientId; }
            set
            {
                _settings.FhirPatientId = value;
                RaisePropertyChanged(() => PatientId);
            }
        }

        public string PatientName
        {
            get { return _settings.FhirPatientName; }
            set
            {
                _settings.FhirPatientName = value;
                RaisePropertyChanged(() => PatientName);
            }
        }

        private bool _patientRequestRunning;
        public bool PatientRequestRunning
        {
            get { return _patientRequestRunning; }
            set { Set(ref _patientRequestRunning, value); }
        }

        private List<PatientModel> _patientList;
        public List<PatientModel> PatientList
        {
            get { return _patientList; }
            set { DispatcherHelper.CheckBeginInvokeOnUI(() => Set(ref _patientList, value)); }
        }

        private PatientModel _selectedPatient;
        public PatientModel SelectedPatient
        {
            get { return _selectedPatient; }
            set {
                Set(ref _selectedPatient, value);
                SavePatientSelectionCommand.RaiseCanExecuteChanged();
            }
        }

        private RelayCommand _showPatientSelectionCommand;
        public RelayCommand ShowPatientSelectionCommand => _showPatientSelectionCommand ?? (_showPatientSelectionCommand = new RelayCommand(() =>
        {
            LoadPatientsFromServer();
            WindowWrapper.Current().Dispatcher.Dispatch(() =>
            {
                var modal = Window.Current.Content as ModalDialog;
                modal.CanBackButtonDismiss = true;
                modal.ModalBackground = Application.Current.Resources["SystemControlPageBackgroundBaseMediumBrush"] as SolidColorBrush;
                modal.ModalContent = new PatientSelectControl();
                modal.IsModal = true;
            });
        }, () => true));

        private RelayCommand _cancelPatientSelectionCommand;
        public RelayCommand CancelPatientSelectionCommand => _cancelPatientSelectionCommand ?? (_cancelPatientSelectionCommand = new RelayCommand(() =>
        {
            WindowWrapper.Current().Dispatcher.Dispatch(() =>
            {
                var modal = Window.Current.Content as ModalDialog;
                modal.CanBackButtonDismiss = false;
                modal.IsModal = false;
            });
        }, () => true));

        private RelayCommand _savePatientSelectionCommand;
        public RelayCommand SavePatientSelectionCommand => _savePatientSelectionCommand ?? (_savePatientSelectionCommand = new RelayCommand(() =>
        {
            // TODO: remove debug line
            // PatientId = SelectedPatient.Id;
            PatientId = "pat2";
            PatientName = SelectedPatient.Name;

            WindowWrapper.Current().Dispatcher.Dispatch(() =>
            {
                var modal = Window.Current.Content as ModalDialog;
                modal.CanBackButtonDismiss = false;
                modal.IsModal = false;
            });
        }, () => SelectedPatient != null));

        private async void LoadPatientsFromServer()
        {
            PatientRequestRunning = true;

            IFHIRLabDataService dataService = ServiceLocator.Current.GetInstance<IFHIRLabDataService>();

            Bundle patients = await dataService.PatientsAsync(async (e) =>
            {
                var dialog = new MessageDialog("Requested resource is not available on the server.", "Error");
                var result = await dialog.ShowAsync();
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            });
            var resourceList = new List<PatientModel>();
            foreach (var item in patients.Entry)
            {
                
                if(item.Resource.TypeName == "Patient")
                {
                    resourceList.Add(new PatientModel((Patient)item.Resource));
                }
            }

            PatientList = resourceList;
            PatientRequestRunning = false;
        }
    }

    public class AboutPartViewModel : ViewModelBase
    {
        public Uri Logo => Windows.ApplicationModel.Package.Current.Logo;

        public string DisplayName => Windows.ApplicationModel.Package.Current.DisplayName;

        public string Publisher => Windows.ApplicationModel.Package.Current.PublisherDisplayName;

        public string Version
        {
            get
            {
                var v = Windows.ApplicationModel.Package.Current.Id.Version;
                return $"{v.Major}.{v.Minor}.{v.Build}.{v.Revision}";
            }
        }
    }
}

