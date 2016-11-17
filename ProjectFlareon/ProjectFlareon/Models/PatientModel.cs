using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFlareon.Models
{
    public class PatientModel
    {
        public string Id;
        public string Identifier;
        public string Name;
        public string FamilyName;
        public string GivenName;
        public IEnumerable<string> FamilyNames;
        public IEnumerable<string> GivenNames;

        public PatientModel(Patient fhirPatient)
        {
            Id = fhirPatient.Id;
            Identifier = fhirPatient.Identifier.FirstOrDefault()?.Value;
            var patName = fhirPatient.Name.FirstOrDefault();
            if(patName != null)
            {
                FamilyNames = patName.Family;
                GivenNames = patName.Given;
                GivenName = String.Join(" ", GivenNames);
                FamilyName = String.Join(" ", FamilyNames);

                Name = $"{GivenName} {FamilyName}";
            }
        }
    }
}
