using AutoMapper;
using PatientManagement.Application.DTOs;
using PatientManagement.Model.Models;

namespace PatientManagement.Api.AutoMappings
{
    public class PatientManagementMappingProfile : Profile
    {
        public PatientManagementMappingProfile()
        {
            CreateMap<Patient, AddPatientDTO>().ReverseMap();
            CreateMap<Patient, GetPatientDTO>().ReverseMap();
            CreateMap<Patient, GetPatientWithRecordsDTO>().ReverseMap();
            CreateMap<Patient, UpdatePatientDTO>().ReverseMap();

            CreateMap<PatientRecord, AddPatientRecordDTO>().ReverseMap();
            CreateMap<PatientRecord, GetPatientRecordDTO>().ReverseMap();
            CreateMap<PatientRecord, UpdatePatientRecordDTO>().ReverseMap();
        }
    }
}
