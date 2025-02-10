using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientManagement.Application.DTOs;
using PatientManagement.Application.IRepositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace PatientManagement.Api.Controllers
{
    /// <summary>
    /// Controller for managing patient records.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PatientRecordController : ControllerBase
    {
        private readonly IPatientRecordRepo _patientRecordService;
        private readonly ILogger<PatientRecordController> _logger;

        public PatientRecordController(IPatientRecordRepo patientRecordService, ILogger<PatientRecordController> logger)
        {
            _patientRecordService = patientRecordService;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new patient record.
        /// </summary>
        /// <param name="patientId">The ID of the patient.</param>
        /// <param name="addPatientRecordDTO">The patient record details.</param>
        /// <returns>Response message with record details.</returns>
        [HttpPost("add-patient-record/{patientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddPatientRecord(int patientId, [FromBody] AddPatientRecordDTO addPatientRecordDTO)
        {
            try
            {
                if (addPatientRecordDTO == null)
                    return BadRequest("Patient record details are missing");

                var response = await _patientRecordService.AddPatientRecordAsync(patientId, addPatientRecordDTO);
                if (!response.Success)
                    return BadRequest(new { Message = response.Message });

                return Ok(new { Message = response.Message, PatientRecordDetails = addPatientRecordDTO });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding a patient record: {ex.Message}");
                return StatusCode(500, "An error occurred while adding a patient record.");
            }
        }

        /// <summary>
        /// Retrieves all patient records.
        /// </summary>
        /// <returns>List of patient records.</returns>
        [HttpGet("get-all-patient-records")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllPatientRecord()
        {
            try
            {
                var patientRecords = await _patientRecordService.GetAllPatientRecordsAsync();
                if (patientRecords == null)
                    return NotFound("No patient records found.");

                return Ok(patientRecords);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting all patient records: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves a patient record by its ID.
        /// </summary>
        /// <param name="recordId">The record ID.</param>
        /// <returns>Patient record details.</returns>
        [HttpGet("get-patient-record-by-id/{recordId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPatientRecordById(int recordId)
        {
            try
            {
                var patientRecord = await _patientRecordService.GetPatientRecordByIdAsync(recordId);
                if (patientRecord == null)
                    return NotFound($"No patient record found with ID: {recordId}");

                return Ok(patientRecord);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting patient record with ID {recordId}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates a patient record.
        /// </summary>
        /// <param name="recordId">The record ID.</param>
        /// <param name="updatePatientRecordDTO">Updated patient record details.</param>
        /// <returns>Response message with updated record details.</returns>
        [HttpPatch("update-patient-record/{recordId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePatientRecord(int recordId, [FromBody] UpdatePatientRecordDTO updatePatientRecordDTO)
        {
            try
            {
                if (updatePatientRecordDTO == null)
                    return BadRequest("Patient record details are missing");

                var response = await _patientRecordService.UpdatePatientRecordAsync(recordId, updatePatientRecordDTO);
                if (!response.Success)
                    return BadRequest(new { Message = response.Message });

                return Ok(new { Message = response.Message, PatientRecordDetails = updatePatientRecordDTO });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating patient record with ID {recordId}: {ex.Message}");
                return StatusCode(500, "An error occurred while updating the patient record.");
            }
        }
    }
}
