namespace TrainingTask.DAL.Interfaces
{
    public interface IEmployeeTaskRepository
    {
        void Add(int employeeId, int taskId);

        void DeleteEmployeesFromTask(int taskId);
    }
}
