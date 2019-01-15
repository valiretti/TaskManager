using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using TrainingTask.Common.Models;
using TrainingTask.Web.MVC.Models;

namespace TrainingTask.Web.MVC.Profiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Project, ProjectViewModel>();

            CreateMap<ProjectCreationModel, CreateProject>()
                .ForMember(p => p.Tasks,
                    opt => opt.MapFrom(p => JsonConvert.DeserializeObject<IEnumerable<CreateTask>>(p.Tasks)));
        }
    }
}
