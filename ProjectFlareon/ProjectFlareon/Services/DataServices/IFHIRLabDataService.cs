using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFlareon.Services.DataServices
{
    public interface IFHIRLabDataService
    {
        Task<Bundle> PatientsAsync(Action<Exception> errorAction, Bundle existingBundle = null);
        Task<Patient> PatientByIdAsync(Action<Exception> errorAction, string patientId);
        Task<Organization> OrganizationByIdAsync(Action<Exception> errorAction, string organizationId);
        Task<Practitioner> PractitionerByIdAsync(Action<Exception> errorAction, string practitionerId);
        Task<Bundle> DiagnosticReportsForPatientAsync(Action<Exception> errorAction, string patId, SummaryType summary = SummaryType.True);
        Task<Bundle> DiagnosticReportsForPatientAsync(Action<Exception> errorAction, string patId, DateTimeOffset? issueDate = null, DateTimeOffset? effectiveDate = null, DateTimeOffset? lastUpdateDate = null, DiagnosticReport.DiagnosticReportStatus? status = null, SummaryType summary = SummaryType.True);
        Task<Bundle> DiagnosticReportsForPatientAsync(Action<Exception> errorAction, Bundle existingBundle = null);
        Task<DiagnosticReport> DiagnosticReportByIdAsync(Action<Exception> errorAction, string reportId, string versionId = null);
        Task<Bundle> DiagnosticReportHistoryByIdAsync(Action<Exception> errorAction, string reportId, SummaryType summary = SummaryType.True);
        Task<Bundle> ObservationsForPatientAsync(Action<Exception> errorAction, string patId);
        Task<Bundle> ObservationsForPatientAsync(Action<Exception> errorAction, string patId, DateTimeOffset? effectiveDate = null, DateTimeOffset? lastUpdateDate = null, string observationCode = null, string observationCategory = null, Observation.ObservationStatus? status = null);
        Task<Observation> ObservationByIdAsync(Action<Exception> errorAction, string observationId);

        // Observation search parameters
        //patient
        //Supported
        //code
        //Supported
        //category
        //Supported
        //status
        //Supported
        //date
        //Supported
        //_lastUpdated
        //Supported
    }
}
