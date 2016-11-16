using GalaSoft.MvvmLight.Threading;
using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFlareon.ViewModels
{
    public class PatientDetailPageViewModel : ViewModelBase
    {
        private Patient _currentPatient;
        public Patient CurrentPatient
        {
            get { return _currentPatient; }
            set { DispatcherHelper.CheckBeginInvokeOnUI(() => Set(ref _currentPatient, value)); }
        }
    }
}
