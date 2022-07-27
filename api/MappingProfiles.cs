using AutoMapper;
using Entities.Models;
using Shared.DTOs;

namespace api
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<Company, CompanyDTO>()
                .ForMember(c => c.FullAddress, 
                Opt => Opt.MapFrom(s => string.Join(' ', s.Address, s.Country)));

            CreateMap<Employee, EmployeeDto>();

            CreateMap<CompanyForCreationDto, Company>();

            CreateMap<EmployeeForCreationDto, Employee>();
        }
    }
}
