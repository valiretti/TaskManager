using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Entities;
using TrainingTask.DAL.NHRepositories.Resolvers;

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

            CreateMap<TaskNh, TaskViewModel>()
                .ForMember(t => t.WorkHours, opt => opt.MapFrom(t => TimeSpan.FromMinutes(t.WorkHours)))
                .ForMember(t => t.Employees, opt => opt.MapFrom(t => t.Employees.Select(e => e.Id)))
                .ForMember(t => t.FullNames, opt => opt.MapFrom(t => t.Employees.Select(e => $"{e.FirstName} {e.LastName} {e.Patronymic}")));

            CreateMap<CreateTask, TaskNh>()
                .ForMember(t => t.WorkHours, opt => opt.MapFrom(t => (int)(t.WorkHours * 60)))
                .ForMember(t => t.Project, opt => opt.MapFrom<EntityResolver<CreateTask, TaskNh, int, ProjectNh>, int>(ct => ct.ProjectId))
                .ForMember(t => t.Employees, opt => opt.MapFrom<CollectionResolver<CreateTask, TaskNh, int, EmployeeNh>, IEnumerable<int>>(ct => ct.Employees));
        }
    }
}
