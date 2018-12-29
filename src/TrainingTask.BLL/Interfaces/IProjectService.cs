using System.Collections.Generic;
using TrainingTask.Common.Models;

namespace TrainingTask.BLL.Interfaces
{
    public interface IProjectService
    {
        int Add(CreateProject project);

        void Update(CreateProject project);

        void Delete(int id);

        Project Get(int id);

        IEnumerable<Project> GetAll();
    }
}
