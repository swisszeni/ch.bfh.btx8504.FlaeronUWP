using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFlareon.Models
{
    public class ObservationModel
    {
        public string ObservationId;
        public string CodeDisplay;
        public decimal? Value;
        public string ValueString;
        public string ValueUnit;
        public string ReferenceRange;

        public ObservationModel(Observation obs)
        {
            ObservationId = obs.Id;
            CodeDisplay = obs.Code.Coding.FirstOrDefault()?.Display;
            if(obs.Value is FhirString)
            {
                ValueString = ((FhirString)obs.Value).Value;
            } else if(obs.Value is Quantity)
            {
                Value = ((Quantity)obs.Value).Value;
                ValueString = Value?.ToString();
                ValueUnit = ((Quantity)obs.Value).Unit;
            }
        }
    }
}
