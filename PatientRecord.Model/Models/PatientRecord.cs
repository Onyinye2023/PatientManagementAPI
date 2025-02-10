using PatientManagement.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PatientManagement.Model.Models
{
    public class PatientRecord
    {
        public int RecordId { get; set; }
        public string? Description { get; set; }
        public string? Prescription { get; set; }
        public string? TesToRun { get; set; }
        public string? DoctorAssigned { get; set; }
        public DateTime DateOfRecord { get; set; } = DateTime.Now;
        public DateTime? UpdatedRecord { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int PatientId { get; set; }

        [JsonIgnore]
        public Patient Patient { get; set; }
    }
}
