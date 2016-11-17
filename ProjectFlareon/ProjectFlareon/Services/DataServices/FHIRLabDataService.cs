using GalaSoft.MvvmLight.Threading;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using ProjectFlareon.Models;
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

        public async Task<Bundle> PatientsAsync(Action<Exception> errorAction, Bundle existingBundle = null)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (existingBundle != null)
                    {
                        return client.Continue(existingBundle);
                    }
                    else
                    {
                        return client.Search<Patient>();
                    }
                } catch (Exception e)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() => errorAction(e));
                    return null;
                }
            });
        }

        public async Task<Bundle> SearchPatientAsync(Action<Exception> errorAction, string[] searchTerms, Bundle existingBundle = null)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (existingBundle != null)
                    {
                        return client.Continue(existingBundle);
                    }
                    else
                    {
                        SearchParams sParams = new SearchParams();
                        foreach (string term in searchTerms)
                        {
                            sParams.Add("name", term);
                        }

                        return client.Search<Patient>(sParams);
                    }
                }
                catch (Exception e)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() => errorAction(e));
                    return null;
                }
            });
        }

        public async Task<PatientModel> PatientByIdAsync(Action<Exception> errorAction, string patientId)
        {
            return await Task.Run(() =>
            {
                try
                {
                    return new PatientModel(client.Read<Patient>($"Patient/{patientId}"));
                } catch (Exception e)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() => errorAction(e));
                    return null;
                }
                
            });
        }

        public async Task<Organization> OrganizationByIdAsync(Action<Exception> errorAction, string organizationId)
        {
            return await Task.Run(() =>
            {
                try
                {
                    return client.Read<Organization>($"Organization/{organizationId}");
                } catch (Exception e)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() => errorAction(e));
                    return null;
                }
            });
        }

        public async Task<Practitioner> PractitionerByIdAsync(Action<Exception> errorAction, string practitionerId)
        {
            return await Task.Run(() =>
            {
                try
                {
                    return client.Read<Practitioner>($"Practitioner/{practitionerId}");
                } catch (Exception e)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() => errorAction(e));
                    return null;
                }
            });
        }

        public async Task<Bundle> DiagnosticReportsForPatientAsync(Action<Exception> errorAction, string patId, SummaryType summary = SummaryType.True)
        {
            return await DiagnosticReportsForPatientAsync(errorAction, patId, null, null, null, null, summary);
        }

        public async Task<Bundle> DiagnosticReportsForPatientAsync(Action<Exception> errorAction, string patId, DateTimeOffset? issueDate = null, DateTimeOffset? effectiveDate = null, DateTimeOffset? lastUpdateDate = null, DiagnosticReport.DiagnosticReportStatus? status = null, SummaryType summary = SummaryType.True)
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

            return await Task.Run(() =>
            {
                return client.Search<DiagnosticReport>(sParams);
            });
        }

        public async Task<Bundle> DiagnosticReportsForPatientAsync(Action<Exception> errorAction, Bundle existingBundle = null)
        {
            return await Task.Run(() =>
            {
                return client.Continue(existingBundle);
            });
        }

        public async Task<DiagnosticReport> DiagnosticReportByIdAsync(Action<Exception> errorAction, string reportId, string versionId = null)
        {
            return await Task.Run(() =>
            {
                return client.Read<DiagnosticReport>(versionId == null ? $"DiagnosticReport/{reportId}" : $"DiagnosticReport/{reportId}/_history/{versionId}");
            });
        }

        public async Task<Bundle> DiagnosticReportHistoryByIdAsync(Action<Exception> errorAction, string reportId, SummaryType summary = SummaryType.True)
        {
            return await Task.Run(() =>
            {
                return client.History($"DiagnosticReport/{reportId}", null, null, summary);
            });
        }

        public Task<Bundle> ObservationsForPatientAsync(Action<Exception> errorAction, string patId)
        {
            throw new NotImplementedException();
        }

        public Task<Bundle> ObservationsForPatientAsync(Action<Exception> errorAction, string patId, DateTimeOffset? effectiveDate = default(DateTimeOffset?), DateTimeOffset? lastUpdateDate = default(DateTimeOffset?), string observationCode = null, string observationCategory = null, Observation.ObservationStatus? status = default(Observation.ObservationStatus?))
        {
            throw new NotImplementedException();
        }

        public Task<Observation> ObservationByIdAsync(Action<Exception> errorAction, string observationId)
        {
            throw new NotImplementedException();
        }
    }
}
