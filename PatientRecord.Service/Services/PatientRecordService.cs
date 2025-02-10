using AutoMapper;
using Microsoft.Extensions.Logging;
using PatientManagement.Application.DTOs;
using PatientManagement.Application.IRepositories;
using PatientManagement.Dal.Data;
using PatientManagement.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace PatientManagement.Application.Services
{
    public class PatientRecordService : IPatientRecordRepo
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<PatientRecordService> _logger;

        public PatientRecordService(AppDbContext context, IMapper mapper, ILogger<PatientRecordService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ResponseDTO> AddPatientRecordAsync(int patientId, AddPatientRecordDTO addPatientRecordDTO)
        {
            try
            {
                if (addPatientRecordDTO == null)
                {
                    _logger.LogError("addPatientRecordDTO is null.");
                    return new ResponseDTO { Message = "Invalid Patient record", Success = false };
                }

                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PatientId == patientId);

                if (patient == null)
                {
                    _logger.LogError("Patient does exist");
                    return new ResponseDTO { Message = "Patient does exist", Success = false };
                }

                var patientRecordAdded = _mapper.Map<PatientRecord>(addPatientRecordDTO);
                patientRecordAdded.PatientId = patientId;

                if (patientRecordAdded == null)
                {
                    _logger.LogError("Mapping of addPatientRecord to PatientRecord failed.");
                    return new ResponseDTO { Success = false, Message = "Mapping error." };
                }

                _context.PatientRecords.Add(patientRecordAdded);
                await _context.SaveChangesAsync();

                return new ResponseDTO { Success = true, Message = "Patient record added successfully." };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResponseDTO { Success = false, Message = "An error occurred while adding a Patient Record." };
            }
        }

        public async Task<IEnumerable<GetPatientRecordDTO>> GetAllPatientRecordsAsync()
        {
            try
            {
                var patientsRecord = await _context.PatientRecords.ToListAsync();

                if (patientsRecord == null)
                {
                    return null;
                }

                return _mapper.Map<IEnumerable<GetPatientRecordDTO>>(patientsRecord);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<GetPatientRecordDTO> GetPatientRecordByIdAsync(int recordId)
        {
            try
            {
                var patientRecord = await _context.PatientRecords.FirstOrDefaultAsync(p => p.RecordId == recordId);

                if (patientRecord == null)
                {
                    return null;
                }

                return _mapper.Map<GetPatientRecordDTO>(patientRecord);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<ResponseDTO> UpdatePatientRecordAsync(int recordId, UpdatePatientRecordDTO updatePatientRecordDTO)
        {
            try
            {
                if (updatePatientRecordDTO == null)
                {
                    _logger.LogError("updatePatientRecordDTO is null.");
                    return new ResponseDTO { Message = "Invalid Patient record details", Success = false };
                }

                var existingPatientRecord = await _context.PatientRecords.FirstOrDefaultAsync(p => p.RecordId == recordId);

                if (existingPatientRecord == null)
                {
                    _logger.LogError($"No Patient Record found with the id: {recordId}");
                    return new ResponseDTO { Message = $"No Patient Record found with the id: {recordId}", Success = false };

                }

                existingPatientRecord.RecordId = recordId;
                existingPatientRecord.Description = updatePatientRecordDTO.Description;
                existingPatientRecord.Prescription = updatePatientRecordDTO.Prescription;
                existingPatientRecord.TesToRun = updatePatientRecordDTO.TesToRun;
                existingPatientRecord.DoctorAssigned = updatePatientRecordDTO.DoctorAssigned;
                existingPatientRecord.UpdatedRecord = DateTime.Now;

                await _context.SaveChangesAsync();

                return new ResponseDTO { Success = true, Message = "Patient Record updated successfully." };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResponseDTO { Success = false, Message = "An error occurred while updating a Patient's record." };
            }

        }
    }
}
