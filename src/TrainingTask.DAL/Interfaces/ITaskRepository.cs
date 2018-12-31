using System.Collections.Generic;
using TrainingTask.Common.Models;

namespace TrainingTask.DAL.Interfaces
{
    public interface ITaskRepository : IRepository<Task>
    {
        IEnumerable<TaskViewModel> GetAll();
        TaskViewModel GetViewModel(int i);
    }
}
