using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PatientManagement.Api.Controllers;
using PatientManagement.Application.DTOs;
using PatientManagement.Application.IRepositories;
using Xunit;
using FluentAssertions;

namespace PatientManagement.Test.PatientManagementTest
{
    public class PatientRecordControllerTest
    {
        private readonly Mock<IPatientRecordRepo> _mockService;
        private readonly Mock<ILogger<PatientRecordController>> _mockLogger;
        private readonly PatientRecordController _controller;

        public PatientRecordControllerTest()
        {
            _mockService = new Mock<IPatientRecordRepo>();
            _mockLogger = new Mock<ILogger<PatientRecordController>>();
            _controller = new PatientRecordController(_mockService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task AddPatientRecord_ReturnsOk_WhenRecordIsAddedSuccessfully()
        {
            // Arrange
            var patientId = 1;
            var addPatientRecordDTO = new AddPatientRecordDTO { Description = "Test Description" };
            var responseDTO = new ResponseDTO { Success = true, Message = "Patient record added successfully." };

            _mockService.Setup(repo => repo.AddPatientRecordAsync(patientId, addPatientRecordDTO))
                     .ReturnsAsync(responseDTO);

            // Act
            var result = await _controller.AddPatientRecord(patientId, addPatientRecordDTO);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeEquivalentTo(new
            {
                Message = "Patient record added successfully.",
                PatientRecordDetails = addPatientRecordDTO
            });
        }

        [Fact]
        public async Task GetAllPatientRecords_ReturnsOk_WhenRecordsExist()
        {
            // Arrange
            var records = new List<GetPatientRecordDTO> { new GetPatientRecordDTO { RecordId = 1, Description = "Test Record" } };
            _mockService.Setup(repo => repo.GetAllPatientRecordsAsync()).ReturnsAsync(records);

            // Act
            var result = await _controller.GetAllPatientRecord();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(records);
        }

        [Fact]
        public async Task GetPatientRecordById_ReturnsNotFound_WhenRecordDoesNotExist()
        {
            // Arrange
            var recordId = 99;
            _mockService.Setup(repo => repo.GetPatientRecordByIdAsync(recordId)).ReturnsAsync((GetPatientRecordDTO)null);

            // Act
            var result = await _controller.GetPatientRecordById(recordId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task UpdatePatientRecord_ReturnsOk_WhenUpdateIsSuccessful()
        {
            // Arrange
            var recordId = 1;
            var updatePatientRecordDTO = new UpdatePatientRecordDTO { Description = "Updated Description" };
            var responseDTO = new ResponseDTO { Success = true, Message = "Patient record updated successfully." };

            _mockService.Setup(repo => repo.UpdatePatientRecordAsync(recordId, updatePatientRecordDTO))
                     .ReturnsAsync(responseDTO);

            // Act
            var result = await _controller.UpdatePatientRecord(recordId, updatePatientRecordDTO);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(new
            {
                Message = "Patient record updated successfully.",
                PatientRecordDetails = updatePatientRecordDTO
            });
        }
    }
}
