using PatientManagement.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagement.Application.DTOs
{
    public class AddPatientRecordDTO
    {
        public string? Description { get; set; }
        public string? Prescription { get; set; }
        public string? TesToRun { get; set; }
        public string? DoctorAssigned { get; set; }
    }
}
