using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hl7.Fhir.Model.Bundle;

namespace ProjectFlareon.Models
{
    public class DiagnosticReportServerRequestModel
    {
        public string method;
        public string methodNice;
        public string diagnosticReportId;
        public string versionIssueDate;
        public string versionIssuer;

        public DiagnosticReportServerRequestModel(EntryComponent entry)
        {

        }
    }
}
