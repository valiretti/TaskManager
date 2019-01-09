using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Entities;

namespace TrainingTask.DAL.Profiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {

            CreateMap<Project, ProjectNh>()
                .ForMember(t => t.Tasks, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
