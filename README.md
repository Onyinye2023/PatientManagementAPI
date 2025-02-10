PATIENT MANAGEMENT API

Overview

The Patient Management API is a simple CRUD-based .NET 8 Web API designed to manage patient and their records efficiently. The API allows users to create, retrieve, update, and soft-delete patient. The users can also create, retrieve, and update patient records while ensuring data integrity and validation. The application is containerized using Docker and includes Swagger API documentation for easy exploration and testing.


Tech Stack

.NET 8 Web API – Backend framework

SQLite – Lightweight data persistence

Entity Framework Core – ORM for database interactions

Moq & xUnit – Unit testing

FluentAssertions – Improved test readability

Docker – Containerization

Swagger – API documentation


Key Features

CRUD Operations: 	Create Read, Update, and Soft-Delete patients.
		 	Create, Read, Update patient's Record

Validation & Error Handling: Ensures patient data integrity.

Unit Tests: Robust testing with Moq & xUnit.

Swagger Integration: Interactive API documentation.

Containerized Deployment: Easily deployable with Docker.



Design Decisions

1. 	Entity Design

	Patient Entity includes fields like PatientId, PatientName, Address, Occupation, PhoneNumber, Weight, Height, BloodGroup, Genotype, Gender, DateOfBirth, UpdatedPatient, IsDeleted, Age(automatically calculated), and PatientRecord(a collection of patient records).

	PatientRecord Entity includes fields like RecordId, PatientId(Foreign key), Description, Prescription, TestToRun, DoctorAssigned, DateOfRecord, UpdatedRecord, IsDeleted.


2.	 Repository Pattern

•	Why? Decouples database logic from the controller for better maintainability and testability.

•	IPatientManagementRepo defines CRUD operations, implemented by PatientManagementService.

•	IPatientRecordRepo defines CRUD operations, implemented by PatientRecordService

3.	Service Layer

•	Why? Adds a business logic layer between controllers and repositories.

•	PatientManagementService and PatientRecordService ensures validation and data consistency before database operations.

4.	Exception Handling & Logging

•	Centralized exception handling using try-catch in controllers.

•	Logging errors using ILogger to capture issues in production.

5.	DTO (Data Transfer Object)

Used to achieve
•	Encapsulation & Data Security
•	Data Transformation & Mapping
•	Decoupling & Maintainability
•	Performance Optimization
•	Validation & Input Handling

6.	Testing Strategy

•	Unit Tests: Validate repository and service layer functions.

•	Mocking Dependencies: Uses Moq to isolate components in tests.

•	FluentAssertions: Provides better test readability and debugging insights.


7.	Containerization with Docker

•	Why? Ensures consistency across environments and simplifies deployment.

•	Includes a Dockerfile to build and run the API in a container.


API Endpoints

Patient Management

Method	        Endpoint	                                      Description

POST	          /add-patient	                                  Add a new patient

GET	        /get-all-patients	                          Retrieve all patients

GET	        /get-patient-by-id/{id}	                    Get a patient by ID

GET	        /get-patient-with-records/{patientId}	      Get a patient by ID including the records

PATCH	      /update-patient{id}	                        Update a patient

DELETE	    /soft-delete-patient{id}	                  Soft delete a patient



Patient Record

Method	    Endpoint	                                  Description

POST	      /add-patient-record{patiendId}	            Add a new patient record

GET	        /get-all-patients-records	                  Retrieve all patient records

GET	        /get-patient-record-by-id/{recordId}	      Get a patient record by ID

PATCH	      /update-patient-record{recordId}	          Update a patient record



