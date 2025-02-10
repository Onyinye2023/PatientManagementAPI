using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagement.Application.DTOs
{
    public class GetPatientRecordDTO
    {
        public int RecordId { get; set; }
        public string? Description { get; set; }
        public string? Prescription { get; set; }
        public string? TesToRun { get; set; }
        public string? DoctorAssigned { get; set; }
        public DateTime Date { get; set; }
        public DateTime? UpdatedRecord { get; set; }
        public int PatientId { get; set; }
    }
}
