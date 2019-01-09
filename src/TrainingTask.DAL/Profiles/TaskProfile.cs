using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Entities;

namespace TrainingTask.DAL.Profiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<CreateTask, Task>()
                .ForMember(t => t.WorkHours, opt => opt.MapFrom(t => TimeSpan.FromHours(t.WorkHours)))
                .ReverseMap()
                .ForMember(t => t.Employees, opt => opt.Ignore());

            CreateMap<Task, TaskNh>()
                .ForMember(t => t.Employees, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(t => t.ProjectId, opt => opt.MapFrom(t => t.Project.Id));

            CreateMap<TaskNh, TaskViewModel>();
        }
    }
}
