using PatientManagement.Model.Enum;
using PatientManagement.Model.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagement.Application.DTOs
{
    public class AddPatientDTO
    {
        public string? PatientName { get; set; }
        public string? Address { get; set; }
        public string? Occupation { get; set; }

        [Required]
        [StringLength(11, ErrorMessage = "Phone Number must have a maximum length of 11")]
        [RegularExpression("^(080|081|070|091|090)[0-9]*$", ErrorMessage = "Phone Number must follow righr format and must be 11 dights.")]
        public string? PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public BloodGroup BloodGroup { get; set; }
        public Genotype Genotype { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [CustomValidation(typeof(DateTimeValidation), nameof(DateTimeValidation.ValidateDate))]
        public DateTime DateOfBirth { get; set; }

    }
}
