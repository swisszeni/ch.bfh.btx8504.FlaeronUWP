using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using ProjectFlareon.Services.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFlareon.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IFHIRLabDataService, DesignFHIRLabDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IFHIRLabDataService, FHIRLabDataService>();
            }

            SimpleIoc.Default.Register<MainPageViewModel>();
            SimpleIoc.Default.Register<DetailPageViewModel>();
            SimpleIoc.Default.Register<DiagnosticReportsViewModel>();
            SimpleIoc.Default.Register<SettingsPageViewModel>();
        }

        public DiagnosticReportsViewModel DiagnosticReports
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DiagnosticReportsViewModel>();
            }
        }

        public SettingsPageViewModel Settings
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingsPageViewModel>();
            }
        }
    }
}
