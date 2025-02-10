using PatientManagement.Model.Enum;
using PatientManagement.Model.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PatientManagement.Model.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string? PatientName { get; set; }
        public string? Address { get; set; }
        public string? Occupation { get; set; }
        public string? PhoneNumber { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public BloodGroup BloodGroup { get; set; }
        public Genotype Genotype { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? UpdatedPatient { get; set; }

        public bool IsDeleted { get; set; } = false;
        public int Age { get; set; }

        [JsonIgnore]
        public ICollection<PatientRecord> PatientRecords { get; set; }
       
    }
}
