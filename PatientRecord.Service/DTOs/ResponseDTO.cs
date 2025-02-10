using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagement.Application.DTOs
{
    public class ResponseDTO
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
