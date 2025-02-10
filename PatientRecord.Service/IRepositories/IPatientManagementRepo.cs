using PatientManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagement.Application.IRepositories
{
    public interface IPatientManagementRepo
    {
        Task<ResponseDTO> AddPatientAsync(AddPatientDTO addPatientDTO);
        Task<IEnumerable<GetPatientDTO>> GetAllPatientAsync();
        Task <GetPatientDTO> GetPatientByIdAsync(int id);
        Task<GetPatientWithRecordsDTO> GetPatientWithRecordsAsync(int patientId);
        Task<ResponseDTO> UpdatePatientAsync(int id, UpdatePatientDTO updatePatientDTO);
        Task<ResponseDTO> SoftDeletePatientAsync(int id);

    }
}
