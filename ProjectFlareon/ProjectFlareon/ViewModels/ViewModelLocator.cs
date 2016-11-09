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
            SimpleIoc.Default.Register<DiagnosticReportDetailPageViewModel>();
            SimpleIoc.Default.Register<PatientDetailPageViewModel>();
            SimpleIoc.Default.Register<PractitionerDetailViewModel>();
            SimpleIoc.Default.Register<OrganizationDetailPageViewModel>();
            SimpleIoc.Default.Register<SettingsPageViewModel>();
        }

        public DiagnosticReportsViewModel DiagnosticReports
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DiagnosticReportsViewModel>();
            }
        }

        public DiagnosticReportDetailPageViewModel DiagnosticReportDetail
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DiagnosticReportDetailPageViewModel>();
            }
        }

        public PatientDetailPageViewModel PatientDetail
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PatientDetailPageViewModel>();
            }
        }

        public PractitionerDetailViewModel PractitionerDetail
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PractitionerDetailViewModel>();
            }
        }

        public OrganizationDetailPageViewModel OrganizationDetail
        {
            get
            {
                return ServiceLocator.Current.GetInstance<OrganizationDetailPageViewModel>();
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
