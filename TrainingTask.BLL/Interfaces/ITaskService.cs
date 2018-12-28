using System;
using System.Collections.Generic;
using System.Text;
using TrainingTask.Common.Models;

namespace TrainingTask.BLL.Interfaces
{
    public interface ITaskService
    {
        int Add(CreateTask task);

        void Update(CreateTask task);

        void Delete(int id);

        Task Get(int id);

        IEnumerable<TaskViewModel> GetAll();
    }
}
