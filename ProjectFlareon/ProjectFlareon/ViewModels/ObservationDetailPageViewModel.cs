using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
