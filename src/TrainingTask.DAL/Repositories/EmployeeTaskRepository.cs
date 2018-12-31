using System.Data.SqlClient;
using TrainingTask.DAL.Interfaces;

namespace TrainingTask.DAL.Repositories
{
    public class EmployeeTaskRepository : BaseRepository, IEmployeeTaskRepository
    {
        private readonly string _connectionString;

        public EmployeeTaskRepository(string connectionString) : base(connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(int employeeId, int taskId)
        {
            string sqlExpression =
                "INSERT INTO EmployeeTasks (EmployeeId, TaskId) VALUES (@employeeId, @taskId)";
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

        public void DeleteEmployeesFromTask(int taskId)
        {
            base.Delete($"DELETE FROM EmployeeTasks WHERE TaskId = {taskId}");
        }
    }
}
