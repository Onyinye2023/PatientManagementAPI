using PatientManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagement.Application.IRepositories
{
    public  interface IPatientRecordRepo
    {
        Task<ResponseDTO> AddPatientRecordAsync(int patientId, AddPatientRecordDTO addPatientRecordDTO);
        Task<IEnumerable<GetPatientRecordDTO>> GetAllPatientRecordsAsync();
        Task<GetPatientRecordDTO> GetPatientRecordByIdAsync(int recordId);
        Task<ResponseDTO> UpdatePatientRecordAsync(int recordId, UpdatePatientRecordDTO updatePatientRecordDTO);
    }
}
