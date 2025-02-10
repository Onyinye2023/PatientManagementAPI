using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientManagement.Application.DTOs;
using PatientManagement.Application.IRepositories;
using PatientManagement.Model.Models;

namespace PatientManagement.Api.Controllers
{
    /// <summary>
    /// Controller for managing patient records.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PatientManagementController : ControllerBase
    {
        private readonly IPatientManagementRepo _patientManagementService;
        private readonly ILogger<PatientManagementController> _logger;

        public PatientManagementController(IPatientManagementRepo patientManagementService, ILogger<PatientManagementController> logger)
        {
            _patientManagementService = patientManagementService;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new patient.
        /// </summary>
        /// <param name="addPatientDTO">The patient data.</param>
        /// <returns>Response message.</returns>
        [HttpPost("add-patient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddPatient([FromBody] AddPatientDTO addPatientDTO)
        {
            try
            {
                if (addPatientDTO == null) return BadRequest("Patient details are missing");

                var response = await _patientManagementService.AddPatientAsync(addPatientDTO);
                if (!response.Success) return BadRequest(new { Message = response.Message });

                return Ok(new { Message = response.Message, PatientDetails = addPatientDTO });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding a patient: {ex.Message}");
                return StatusCode(500, "An error occurred while adding a patient.");
            }
        }

        /// <summary>
        /// Retrieves all patients.
        /// </summary>
        /// <returns>List of patients.</returns>
        [HttpGet("get-all-patients")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllPatients()
        {
            try
            {
                var patients = await _patientManagementService.GetAllPatientAsync();
                if (patients == null || !patients.Any())
                {
                    return new NotFoundObjectResult("No patients found.");
                }
                return Ok(patients);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting all patients: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves a patient by their ID.
        /// </summary>
        /// <param name="id">Patient ID.</param>
        /// <returns>Patient details.</returns>
        [HttpGet("get-patient-by-id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPatientById(int id)
        {
            try
            {
                var patient = await _patientManagementService.GetPatientByIdAsync(id);
                if (patient == null) return NotFound($"No patient found with the ID: {id}");

                return Ok(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting patient with ID {id}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves a patient along with their records.
        /// </summary>
        /// <param name="patientId">Patient ID.</param>
        /// <returns>Patient details and records.</returns>
        [HttpGet("get-patient-with-records/{patientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPatientWithRecords(int patientId)
        {
            try
            {
                var patientWithRecords = await _patientManagementService.GetPatientWithRecordsAsync(patientId);
                if (patientWithRecords == null) return NotFound($"No patient found with ID: {patientId}");

                return Ok(patientWithRecords);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting patient records for ID {patientId}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates patient details.
        /// </summary>
        /// <param name="id">Patient ID.</param>
        /// <param name="updatePatientDTO">Updated patient data.</param>
        /// <returns>Response message.</returns>
        [HttpPatch("update-patient/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] UpdatePatientDTO updatePatientDTO)
        {
            try
            {
                if (updatePatientDTO == null) return BadRequest("Patient details are missing");

                var response = await _patientManagementService.UpdatePatientAsync(id, updatePatientDTO);
                if (!response.Success) return BadRequest(new { Message = response.Message });

                return Ok(new { Message = response.Message, PatientDetails = updatePatientDTO });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating patient with ID {id}: {ex.Message}");
                return StatusCode(500, "An error occurred while updating a patient.");
            }
        }

        /// <summary>
        /// Soft deletes a patient.
        /// </summary>
        /// <param name="id">Patient ID.</param>
        /// <returns>Response message.</returns>
        [HttpDelete("soft-delete-patient/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SoftDeletePatient(int id)
        {
            try
            {
                var response = await _patientManagementService.SoftDeletePatientAsync(id);
                if (!response.Success) return BadRequest(new { Message = response.Message });

                return Ok(new { Message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting patient with ID {id}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
