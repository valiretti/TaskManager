using System.Data.SqlClient;
using TrainingTask.DAL.Interfaces;

namespace TrainingTask.DAL.Repositories
{
    public class EmployeeTaskRepository : IEmployeeTaskRepository
    {
        private readonly string _connectionString;

        public EmployeeTaskRepository(string connectionString)
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
            string sqlExpression =
                "DELETE FROM EmployeeTasks WHERE TaskId = @id";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlParameter idParam = new SqlParameter("@id", taskId);
                command.Parameters.Add(idParam);
                command.ExecuteNonQuery();
            }
        }
    }
}
