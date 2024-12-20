using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeSalary.Application.Models;
using WorkTimeSalary.Domain.Entities;

namespace WorkTimeSalary.Application.Mapper
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile() 
        {
            CreateMap<Employee, EmployeeDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.WorkPosition, opt => opt.MapFrom(src => src.WorkPosition))
                .ForMember(dest => dest.WorkLogs, opt => opt.MapFrom(src => src.WorkLogs))
                .ReverseMap();

        }
    }
}
