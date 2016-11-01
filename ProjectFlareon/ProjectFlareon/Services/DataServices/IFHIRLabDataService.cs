using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFlareon.Services.DataServices
{
    public interface IFHIRLabDataService
    {
        Bundle DiagnosticReportsForPatient(string patId, bool summary = true);
    }
}
