using System;
using System.Data.SqlClient;
using TrainingTask.Common.Exceptions;
using TrainingTask.Common.Interfaces;
using TrainingTask.DAL.Interfaces;

namespace TrainingTask.DAL.Repositories
{
    public class EmployeeTaskRepository : BaseRepository, IEmployeeTaskRepository
    {
        private readonly string _connectionString;
        private readonly ILog _log;

        public EmployeeTaskRepository(string connectionString, ILog log) : base(connectionString)
        {
            _connectionString = connectionString;
            _log = log;
        }

        public void Add(int employeeId, int taskId)
        {
            string sqlExpression =
                "INSERT INTO EmployeeTasks (EmployeeId, TaskId) VALUES (@employeeId, @taskId)";
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlParameter employeeIdParam = new SqlParameter("@employeeId", employeeId);
                    SqlParameter taskIdParam = new SqlParameter("@taskId", taskId);
                    command.Parameters.AddRange(new[] { employeeIdParam, taskIdParam });
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                var message =
                    $"The employee with the EmployeeId {employeeId} or the task with TaskId {taskId} have already deleted.";
                _log.Error($"{message} SqlException message : {ex.Message}");
                throw new ForeignKeyViolationException(message, ex);
            }

        }

        public void DeleteEmployeesFromTask(int taskId)
        {
            base.Delete($"DELETE FROM EmployeeTasks WHERE TaskId = {taskId}");
        }
    }
}
