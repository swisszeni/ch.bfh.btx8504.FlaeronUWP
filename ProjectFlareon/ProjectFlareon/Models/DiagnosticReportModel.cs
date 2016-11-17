using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFlareon.Models
{
    public class DiagnosticReportModel
    {
        public string DiagnosticReportId;
        public string CodeDisplay;
        public DateTimeOffset? EffectiveDate;
        public string PerformerDisplay;
        public string Status;
        public DiagnosticReportModel(DiagnosticReport report)
        {
            DiagnosticReportId = report.Id;
            CodeDisplay = report.Code.Coding.FirstOrDefault()?.Display;
            if (report.Effective?.TypeName == "dateTime")
            {
                var effectiveInstant = (FhirDateTime)report.Effective;
                EffectiveDate = effectiveInstant.ToDateTimeOffset();
            }
            PerformerDisplay = report.Performer.Display;
            Status = report.Status.ToString();
        }
    }
}
