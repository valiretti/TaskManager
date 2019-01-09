using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Entities;

namespace TrainingTask.DAL.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeNh>()
                .ForMember(e => e.Tasks, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
