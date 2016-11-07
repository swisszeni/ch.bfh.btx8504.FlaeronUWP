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
        }

        public void Patients()
        {
            //client.Search
        }

        public async Task<Bundle> DiagnosticReportsForPatientAsync(string patId, bool summary = true)
        {
            return await Task.Run(() =>
            {
                var paramList = new List<Tuple<string, string>>();
                paramList.Add(Tuple.Create("subject", patId));

                if (summary)
                {
                    paramList.Add(Tuple.Create("_summary", "true"));
                }

                SearchParams sParams = SearchParams.FromUriParamList(paramList);
                return client.Search<DiagnosticReport>(sParams);
            });
        }

        public async Task<DiagnosticReport> DiagnosticReportByIdAsync(string reportId)
        {
            return await Task.Run(() =>
            {
                return client.Read<DiagnosticReport>($"DiagnosticReport/{reportId}");
            });
        }
    }
}
