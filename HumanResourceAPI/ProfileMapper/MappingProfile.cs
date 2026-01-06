using AutoMapper;
using Entities.Dto;
using Entities.Models;

namespace HumanResourceAPI.ProfileMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>()
                    .ForMember(dest => dest.FullAddress,
                               opt => opt.MapFrom(src => $"{src.Address}, {src.Country}"));

            CreateMap<CompanyCreationDto, Company>();
            //CreateMap<CompanyUpdateDto, Company>();
            CreateMap<CompanyUpdateDto, Company>().ReverseMap();

            // Employee Mappings
            CreateMap<EmployeeDto, Employee>().ReverseMap();
            CreateMap<EmployeeForCreationDto, Employee>().ReverseMap();

            // User Mappings
            //CreateMap<UserForRegistertrationDto, User>().ReverseMap();

            //CreateMap<UserForRegistertrationDto, User>()
            //        .ForMember(dest => dest.Id, opt => opt.Ignore())
            //        .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
            //        .ForSourceMember(src => src.Roles, opt => opt.DoNotValidate());
        }
    }
}
