using PatientManagement.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagement.Application.DTOs
{
    public class UpdatePatientDTO
    {
        public string? PatientName { get; set; }
        public string? Address { get; set; }
        public string? Occupation { get; set; }
        public string? PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public BloodGroup BloodGroup { get; set; }
        public Genotype Genotype { get; set; }
        public DateTime? UpdatedPatient { get; set; }
    }
}
