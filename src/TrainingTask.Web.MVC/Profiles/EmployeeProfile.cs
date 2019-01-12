﻿using AutoMapper;
using TrainingTask.Common.Models;
using TrainingTask.Web.MVC.Models;

namespace TrainingTask.Web.MVC.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile() 
        {
            CreateMap<Employee, EmployeeViewModel>()
                .ReverseMap();
        }
    }
}
