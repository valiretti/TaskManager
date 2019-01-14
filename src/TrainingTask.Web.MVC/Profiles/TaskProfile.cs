using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TrainingTask.Common.Models;
using TrainingTask.Web.MVC.Models;
using Task = TrainingTask.Common.Models.Task;
using TaskViewModel = TrainingTask.Common.Models.TaskViewModel;

namespace TrainingTask.Web.MVC.Profiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskViewModel, Models.TaskViewModel>()
                .ReverseMap();

            CreateMap<TaskCreationModel, CreateTask>()
                .ReverseMap();

            CreateMap<TaskViewModel, TaskCreationModel>()
                .ForMember(tcm => tcm.WorkHours, opt => opt.MapFrom(tvm => tvm.WorkHours.Hours))
                ;
        }
    }
}
