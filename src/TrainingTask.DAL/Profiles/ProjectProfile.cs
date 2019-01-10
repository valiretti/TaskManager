using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Entities;
using TrainingTask.DAL.NHRepositories.Resolvers;

namespace TrainingTask.DAL.Profiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {

            CreateMap<Project, ProjectNh>()
                .ForMember(t => t.Tasks, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<CreateProject, ProjectNh>()
                .ForMember(t => t.Tasks, opt => opt.MapFrom<ProjectTaskResolver, IEnumerable<CreateTask>>(ct => ct.Tasks));
        }
    }
}
