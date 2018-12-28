using System;
using System.Collections.Generic;
using System.Text;

namespace TrainingTask.DAL.Interfaces
{
    public interface IEmployeeTaskRepository
    {
        void Add(int employeeId, int taskId);

        void DeleteEmployeesFromTask(int taskId);
    }
}
