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
        public string Name;
        public IEnumerable<string> FamilyNames;
        public IEnumerable<string> GivenNames;

        public PatientModel(Patient fhirPatient)
        {
            Id = fhirPatient.Id;
            var patName = fhirPatient.Name.FirstOrDefault();
            if(patName != null)
            {
                FamilyNames = patName.Family;
                GivenNames = patName.Given;
                var givenName = String.Join(" ", GivenNames);
                var familyName = String.Join(" ", FamilyNames);

                Name = $"{givenName} {familyName}";
            }
        }
    }
}
