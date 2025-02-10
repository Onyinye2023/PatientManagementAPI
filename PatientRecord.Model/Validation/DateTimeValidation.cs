using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagement.Model.Validation
{
    public class DateTimeValidation
    {
        public static ValidationResult? ValidateDate(DateTime date, ValidationContext context)
        {
            if (date > DateTime.UtcNow.AddHours(1))
            {
                return new ValidationResult("You cannot be born in the future.");
            }
            return ValidationResult.Success;
        }
    }
}
