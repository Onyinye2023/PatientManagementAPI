using PatientManagement.Model.Enum;
using PatientManagement.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagement.Application.DTOs
{
    public class GetPatientWithRecordsDTO
    {
        public int PatientId { get; set; }
        public string? PatientName { get; set; }
        public string? Address { get; set; }
        public string? Occupation { get; set; }
        public string? PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public ICollection<PatientRecord> PatientRecords { get; set; }

    }
}
