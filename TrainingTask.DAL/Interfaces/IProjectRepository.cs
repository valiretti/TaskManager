using System;
using System.Collections.Generic;
using System.Text;
using TrainingTask.Common.Models;

namespace TrainingTask.DAL.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        IEnumerable<Project> GetAll();
    }
}
