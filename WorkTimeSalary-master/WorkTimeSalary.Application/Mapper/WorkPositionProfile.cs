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
    public class WorkPositionProfile : Profile
    {
        public WorkPositionProfile()
        {
            CreateMap<WorkPosition, WorkPositionDTO>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                  .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                  .ReverseMap();
        }
    }
}
