﻿using AutoMapper;
using TrainingTask.Common.Models;
using TrainingTask.Web.MVC.Models;
using TaskViewModel = TrainingTask.Common.Models.TaskViewModel;

namespace TrainingTask.Web.MVC.Profiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskViewModel, Models.TaskViewModel>()
                .ForMember(tcm => tcm.WorkHours, opt => opt.MapFrom(tvm => tvm.WorkHours.TotalHours))
                .ReverseMap();

            CreateMap<TaskCreationModel, CreateTask>()
                .ReverseMap();

            CreateMap<TaskViewModel, TaskCreationModel>()
                .ForMember(tcm => tcm.WorkHours, opt => opt.MapFrom(tvm => tvm.WorkHours.TotalHours))
                ;
        }
    }
}
