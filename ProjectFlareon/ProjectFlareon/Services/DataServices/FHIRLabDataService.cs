using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFlareon.Services.DataServices
{
    public class FHIRLabDataService : IFHIRLabDataService
    {
        private FhirClient client;

        public FHIRLabDataService()
        {
            client = new FhirClient(SettingsServices.SettingsService.Instance.FhirServerUri);
            client.PreferredFormat = ResourceFormat.Json;
        }

        public async Task<Bundle> PatientsAsync(Bundle existingBundle = null)
        {
            return await Task.Run(() =>
            {
                if(existingBundle != null)
                {
                    return client.Continue(existingBundle);
                } else
                {
                    return client.Search<Patient>();
                }
            });
        }

        public async Task<Patient> PatientByIdAsync(string patientId)
        {
            return await Task.Run(() =>
            {
                return client.Read<Patient>($"Patient/{patientId}");
            });
        }

        public Task<Organization> OrganizationByIdAsync(string organizationId)
        {
            throw new NotImplementedException();
        }

        public Task<Practitioner> PractitionerByIdAsync(string practitionerId)
        {
            throw new NotImplementedException();
        }

        public async Task<Bundle> DiagnosticReportsForPatientAsync(string patId, SummaryType summary = SummaryType.True)
        {
            return await DiagnosticReportsForPatientAsync(patId, null, null, null, null, summary);
        }

        public async Task<Bundle> DiagnosticReportsForPatientAsync(string patId, DateTimeOffset? issueDate = null, DateTimeOffset? effectiveDate = null, DateTimeOffset? lastUpdateDate = null, DiagnosticReport.DiagnosticReportStatus? status = null, SummaryType summary = SummaryType.True)
        {
            SearchParams sParams = new SearchParams();

            sParams.Add("subject", patId);

            if(issueDate != null)
            {
                //sParams.Add
            }

            if(effectiveDate != null)
            {

            }

            if(lastUpdateDate != null)
            {

            }

            if(status != null)
            {
                //sParams.
            }

            sParams.Summary = summary;
            //var paramList = new List<Tuple<string, string>>();
            //paramList.Add(Tuple.Create("subject", patId));
            //paramList.Add(Tuple.Create("_summary", summary.ToString()));
            //issueDate paramList.Add(Tuple.Create("i", issueDate));




            return await DiagnosticReportsForParametersAsync(sParams);
        }

        private async Task<Bundle> DiagnosticReportsForParametersAsync(SearchParams queryparams)
        {
            return await Task.Run(() =>
            {
                return client.Search<DiagnosticReport>(queryparams);
            });
        }

        public async Task<Bundle> DiagnosticReportsForPatientAsync(Bundle existingBundle = null)
        {
            return await Task.Run(() =>
            {
                return client.Continue(existingBundle);
            });
        }

        public async Task<DiagnosticReport> DiagnosticReportByIdAsync(string reportId, string versionId = null)
        {
            return await Task.Run(() =>
            {
                return client.Read<DiagnosticReport>(versionId == null ? $"DiagnosticReport/{reportId}" : $"DiagnosticReport/{reportId}/_history/{versionId}");
            });
        }

        public async Task<Bundle> DiagnosticReportHistoryByIdAsync(string reportId, SummaryType summary = SummaryType.True)
        {
            return await Task.Run(() =>
            {
                return client.History($"DiagnosticReport/{reportId}/_history", null, null, summary);
            });
        }

        public Task<Bundle> ObservationsForPatientAsync(string patId)
        {
            throw new NotImplementedException();
        }

        public Task<Bundle> ObservationsForPatientAsync(string patId, DateTimeOffset? effectiveDate = default(DateTimeOffset?), DateTimeOffset? lastUpdateDate = default(DateTimeOffset?), string observationCode = null, string observationCategory = null, Observation.ObservationStatus? status = default(Observation.ObservationStatus?))
        {
            throw new NotImplementedException();
        }

        public Task<Observation> ObservationByIdAsync(string observationId)
        {
            throw new NotImplementedException();
        }
    }
}
