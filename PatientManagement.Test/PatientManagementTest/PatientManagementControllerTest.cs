using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PatientManagement.Api.Controllers;
using PatientManagement.Application.DTOs;
using PatientManagement.Application.IRepositories;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientManagement.Test
{
    public class PatientManagementControllerTests
    {
        private readonly Mock<IPatientManagementRepo> _mockService;
        private readonly Mock<ILogger<PatientManagementController>> _mockLogger;
        private readonly PatientManagementController _controller;

        public PatientManagementControllerTests()
        {
            _mockService = new Mock<IPatientManagementRepo>();
            _mockLogger = new Mock<ILogger<PatientManagementController>>();
            _controller = new PatientManagementController(_mockService.Object, _mockLogger.Object);
        }


        [Fact]
        public async Task AddPatient_ReturnsOk_WhenPatientIsAddedSuccessfully()
        {
            // Arrange
            var addPatientDTO = new AddPatientDTO { PatientName = "John Doe" };
            var responseDTO = new ResponseDTO { Success = true, Message = "Patient added successfully."};

            _mockService.Setup(repo => repo.AddPatientAsync(It.IsAny<AddPatientDTO>()))
                     .ReturnsAsync(responseDTO);

            // Act
            var result = await _controller.AddPatient(addPatientDTO);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeEquivalentTo(new
            {
                Message = "Patient added successfully.",
                PatientDetails = addPatientDTO
            });
        }
        [Fact]
        public async Task AddPatient_ReturnsBadRequest_WhenPatientAlreadyExists()
        {
            // Arrange
            var addPatientDTO = new AddPatientDTO { PatientName = "Jane Doe" };
            var responseDTO = new ResponseDTO { Success = false, Message = "Patient already exists" };

            _mockService.Setup(service => service.AddPatientAsync(It.IsAny<AddPatientDTO>()))
                        .ReturnsAsync(responseDTO);

            // Act
            var result = await _controller.AddPatient(addPatientDTO);

            // Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;

            // Convert the anonymous object to a dictionary for assertion
            var response = badRequestResult.Value.Should().BeEquivalentTo(new
            {
                Message = "Patient already exists"
            });
            response.Should().NotBeNull();

            // Ensure the service method was called exactly once
            _mockService.Verify(service => service.AddPatientAsync(It.IsAny<AddPatientDTO>()), Times.Once);
        }

        [Fact]
        public async Task GetAllPatients_ReturnsOk_WithListOfPatients()
        {
            // Arrange
            var patients = new List<GetPatientDTO> { new GetPatientDTO { PatientId = 1, PatientName = "John Doe" } };
            _mockService.Setup(repo => repo.GetAllPatientAsync()).ReturnsAsync(patients);

            // Act
            var result = await _controller.GetAllPatients();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(patients);
        }

        [Fact]
        public async Task GetAllPatients_ReturnsNotFound_WhenNoPatientsExist()
        {
            // Arrange
            _mockService.Setup(repo => repo.GetAllPatientAsync()).ReturnsAsync((List<GetPatientDTO>)null);

            // Act
            var result = await _controller.GetAllPatients();

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetPatientById_ReturnsOk_WhenPatientExists()
        {
            // Arrange
            var patient = new GetPatientDTO { PatientId = 1, PatientName = "John Doe" };
            _mockService.Setup(repo => repo.GetPatientByIdAsync(1)).ReturnsAsync(patient);

            // Act
            var result = await _controller.GetPatientById(1);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(patient);
        }

        [Fact]
        public async Task GetPatientById_ReturnsNotFound_WhenPatientDoesNotExist()
        {
            // Arrange
            _mockService.Setup(repo => repo.GetPatientByIdAsync(1)).ReturnsAsync((GetPatientDTO)null);

            // Act
            var result = await _controller.GetPatientById(1);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetPatientWithRecords_ReturnsOk_WhenPatientExists()
        {
            // Arrange
            var patientWithRecords = new GetPatientWithRecordsDTO { PatientId = 1, PatientName = "John Doe" };
            _mockService.Setup(repo => repo.GetPatientWithRecordsAsync(1)).ReturnsAsync(patientWithRecords);

            // Act
            var result = await _controller.GetPatientWithRecords(1);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(patientWithRecords);
        }

        [Fact]
        public async Task UpdatePatient_ReturnsOk_WhenUpdateIsSuccessful()
        {
            // Arrange
            var updateDTO = new UpdatePatientDTO { PatientName = "John Doe Updated" };
            var responseDTO = new ResponseDTO { Success = true, Message = "Patient updated successfully." };

            _mockService.Setup(repo => repo.UpdatePatientAsync(1, updateDTO)).ReturnsAsync(responseDTO);

            // Act
            var result = await _controller.UpdatePatient(1, updateDTO);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(new { Message = "Patient updated successfully.", PatientDetails = updateDTO });
        }

        [Fact]
        public async Task UpdatePatient_ReturnsBadRequest_WhenUpdateFails()
        {
            // Arrange
            var updateDTO = new UpdatePatientDTO { PatientName = "John Doe Updated" };
            var responseDTO = new ResponseDTO { Success = false, Message = "Update failed" };

            _mockService.Setup(repo => repo.UpdatePatientAsync(1, updateDTO)).ReturnsAsync(responseDTO);

            // Act
            var result = await _controller.UpdatePatient(1, updateDTO);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task SoftDeletePatient_ReturnsOk_WhenDeleteIsSuccessful()
        {
            // Arrange
            var responseDTO = new ResponseDTO { Success = true, Message = "Patient soft deleted successfully." };

            _mockService.Setup(repo => repo.SoftDeletePatientAsync(1)).ReturnsAsync(responseDTO);

            // Act
            var result = await _controller.SoftDeletePatient(1);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(new { Message = "Patient soft deleted successfully." });
        }

        [Fact]
        public async Task SoftDeletePatient_ReturnsBadRequest_WhenDeleteFails()
        {
            // Arrange
            var responseDTO = new ResponseDTO { Success = false, Message = "Delete failed" };

            _mockService.Setup(repo => repo.SoftDeletePatientAsync(1)).ReturnsAsync(responseDTO);

            // Act
            var result = await _controller.SoftDeletePatient(1);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
