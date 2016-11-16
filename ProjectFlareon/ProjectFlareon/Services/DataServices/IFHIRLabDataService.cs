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
        Task<Bundle> PatientsAsync(Bundle existingBundle = null);
        Task<Patient> PatientByIdAsync(string patientId);
        Task<Organization> OrganizationByIdAsync(string organizationId);
        Task<Practitioner> PractitionerByIdAsync(string practitionerId);
        Task<Bundle> DiagnosticReportsForPatientAsync(string patId, SummaryType summary = SummaryType.True);
        Task<Bundle> DiagnosticReportsForPatientAsync(string patId, DateTimeOffset? issueDate = null, DateTimeOffset? effectiveDate = null, DateTimeOffset? lastUpdateDate = null, DiagnosticReport.DiagnosticReportStatus? status = null, SummaryType summary = SummaryType.True);
        Task<Bundle> DiagnosticReportsForPatientAsync(Bundle existingBundle = null);
        Task<DiagnosticReport> DiagnosticReportByIdAsync(string reportId, string versionId = null);
        Task<Bundle> DiagnosticReportHistoryByIdAsync(string reportId, SummaryType summary = SummaryType.True);
        Task<Bundle> ObservationsForPatientAsync(string patId);
        Task<Bundle> ObservationsForPatientAsync(string patId, DateTimeOffset? effectiveDate = null, DateTimeOffset? lastUpdateDate = null, string observationCode = null, string observationCategory = null, Observation.ObservationStatus? status = null);
        Task<Observation> ObservationByIdAsync(string observationId);

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
