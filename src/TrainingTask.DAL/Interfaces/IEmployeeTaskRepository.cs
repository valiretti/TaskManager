namespace TrainingTask.DAL.Interfaces
{
    public interface IEmployeeTaskRepository
    {
        /// <summary>
        /// Associates the employee with the task.
        /// </summary>
        /// <param name="employeeId">The employee identifier.</param>
        /// <param name="taskId">The task identifier.</param>
        void Add(int employeeId, int taskId);

        /// <summary>
        /// Deletes the employees from task.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        void DeleteEmployeesFromTask(int taskId);
    }
}
