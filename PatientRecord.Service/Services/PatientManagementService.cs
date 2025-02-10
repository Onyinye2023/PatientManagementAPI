using AutoMapper;
using Microsoft.Extensions.Logging;
using PatientManagement.Application.DTOs;
using PatientManagement.Application.IRepositories;
using PatientManagement.Dal.Data;
using PatientManagement.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace PatientManagement.Application.Services
{
    public class PatientManagementService : IPatientManagementRepo
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<PatientManagementService> _logger;

        public PatientManagementService(AppDbContext context, IMapper mapper, ILogger<PatientManagementService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ResponseDTO> AddPatientAsync(AddPatientDTO addPatientDTO)
        {
            try
            {
                if (addPatientDTO == null)
                {
                    _logger.LogError("addPatientDTO is null.");
                    return new ResponseDTO { Message = "Invalid Patient details", Success = false };
                }

                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PatientName == addPatientDTO.PatientName);

                if (patient != null)
                {
                    _logger.LogError("Patient already exists");
                    return new ResponseDTO { Message = "Patient already exists", Success = false };
                }

                
                var patientAdded = _mapper.Map<Patient>(addPatientDTO);

                if (patientAdded == null)
                {
                    _logger.LogError("Mapping of addPatientDTO to Patient failed.");
                    return new ResponseDTO { Success = false, Message = "Mapping error." };
                }

                
                patientAdded.Age = CalculateAge(patientAdded.DateOfBirth);

                
                _context.Patients.Add(patientAdded);
                await _context.SaveChangesAsync();

                return new ResponseDTO { Success = true, Message = "Patient added successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResponseDTO { Success = false, Message = "An error occurred while adding a Patient." };
            }
        }


        public async Task<IEnumerable<GetPatientDTO>> GetAllPatientAsync()
        {
            try
            {
                var patients = await _context.Patients.ToListAsync();

                if (patients == null)
                {
                    return null;
                }

               return _mapper.Map<IEnumerable<GetPatientDTO>>(patients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<GetPatientDTO> GetPatientByIdAsync(int id)
        {
            try
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PatientId == id);

                if (patient == null)
                {
                    return null;
                }

                return _mapper.Map<GetPatientDTO>(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<ResponseDTO> UpdatePatientAsync(int id, UpdatePatientDTO updatePatientDTO)
        {
            try
            {
                if (updatePatientDTO == null)
                {
                    _logger.LogError("updatePatientDTO is null.");
                    return new ResponseDTO { Message = "Invalid Patient details", Success = false };
                }

                var existingPatient = await _context.Patients.FirstOrDefaultAsync(p => p.PatientId == id);

                if (existingPatient == null)
                {
                    _logger.LogError($"No Patient found with the id: {id}");
                    return new ResponseDTO { Message = $"No Patient found with the id: {id}", Success = false };

                }

                existingPatient.PatientId = id;
                existingPatient.PatientName = updatePatientDTO.PatientName;
                existingPatient.Occupation = updatePatientDTO.Occupation;
                existingPatient.Address = updatePatientDTO.Address;
                existingPatient.DateOfBirth = updatePatientDTO.DateOfBirth;
                existingPatient.Gender = updatePatientDTO.Gender;
                existingPatient.PhoneNumber = updatePatientDTO.PhoneNumber;
                existingPatient.BloodGroup = updatePatientDTO.BloodGroup;
                existingPatient.Genotype = updatePatientDTO.Genotype;
                existingPatient.Weight = updatePatientDTO.Weight;
                existingPatient.Height = updatePatientDTO.Height;
                existingPatient.UpdatedPatient = updatePatientDTO.UpdatedPatient;

                await _context.SaveChangesAsync();

                return new ResponseDTO { Success = true, Message = "Patient updated successfully." };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResponseDTO { Success = false, Message = "An error occurred while updating a Patient." };
            }
        }

        public async Task<ResponseDTO> SoftDeletePatientAsync(int id)
        {
            try
            {
                var patient = await _context.Patients
                           .Include(p => p.PatientRecords) 
                           .FirstOrDefaultAsync(p => p.PatientId == id);
                
                if (patient == null)
                {
                    _logger.LogError("Patient with the ID does not exist");
                    return new ResponseDTO { Message = "Patient with the ID deos not exist", Success = false };
                }

                patient.IsDeleted = true;

                // Soft delete all associated patient records
                foreach (var record in patient.PatientRecords)
                {
                    record.IsDeleted = true;
                }

                await _context.SaveChangesAsync();

                return new ResponseDTO { Success = true, Message = "Patient soft deleted successfully." };


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResponseDTO { Success = false, Message = "An error occurred while soft deleting a Patient." };
            }
        }

        public async Task<GetPatientWithRecordsDTO> GetPatientWithRecordsAsync(int patientId)
        {
            try
            {
                var patient = await _context.Patients
                    .Include(p => p.PatientRecords)
                    .FirstOrDefaultAsync(p => p.PatientId == patientId);

                if (patient == null)
                {
                    return null;
                }

                return _mapper.Map<GetPatientWithRecordsDTO>(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching patient with ID {PatientId}", patientId);
                return null;
            }
        }


        private int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            int age = today.Year - dateOfBirth.Year;

            if (dateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
}
