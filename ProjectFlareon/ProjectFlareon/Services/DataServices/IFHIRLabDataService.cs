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
        Task<Bundle> DiagnosticReportsForPatientAsync(string patId, SummaryType summary = SummaryType.True);
        Task<DiagnosticReport> DiagnosticReportByIdAsync(string reportId, string versionId = null);
        Task<Bundle> DiagnosticReportHistoryByIdAsync(string reportId, SummaryType summary = SummaryType.True);
    }
}
