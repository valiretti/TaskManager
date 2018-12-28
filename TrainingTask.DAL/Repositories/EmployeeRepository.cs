using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Interfaces;

namespace TrainingTask.DAL.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Employee Get(int id)
        {
            string sqlExpression = "SELECT TOP 1 Id, FirstName, LastName, Patronymic, Position FROM Employees WHERE Id = @id";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlParameter param = new SqlParameter("@id", id);
                command.Parameters.Add(param);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int i = reader.GetInt32(0);
                            string firstName = reader.GetString(1);
                            string lastName = reader.GetString(2);
                            string patronymic = reader.GetString(3);
                            string position = reader.GetString(4);

                            return new Employee()
                            {
                                Id = i,
                                FirstName = firstName,
                                LastName = lastName,
                                Patronymic = patronymic,
                                Position = position
                            };
                        }
                    }
                }
            }

            return null;
        }

        public int Create(Employee item)
        {
            string sqlExpression =
                "INSERT INTO Employees (FirstName, LastName, Patronymic, Position) VALUES (@firstName, @lastName, @patronymic, @position) SET @id=SCOPE_IDENTITY()";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlParameter firstNameParam = new SqlParameter("@firstName", item.FirstName);
                SqlParameter lastNameParam = new SqlParameter("@lastName", item.LastName);
                SqlParameter patronymicParam = new SqlParameter("@patronymic", item.Patronymic);
                SqlParameter positionParam = new SqlParameter("@position", item.Position);
                SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int) {Direction = ParameterDirection.Output};
                command.Parameters.AddRange(new[] { firstNameParam, lastNameParam, patronymicParam, positionParam });
                command.Parameters.Add(idParam);
                command.ExecuteNonQuery();

                return (int)idParam.Value;
            }
        }

        public void Update(Employee item)
        {
            string sqlExpression =
                "UPDATE Employees SET FirstName = @firstName, LastName = @lastName, Patronymic = @patronymic, Position =  @position WHERE Id = @id";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlParameter idParam = new SqlParameter("@id", item.Id);
                SqlParameter firstNameParam = new SqlParameter("@firstName", item.FirstName);
                SqlParameter lastNameParam = new SqlParameter("@lastName", item.LastName);
                SqlParameter patronymicParam = new SqlParameter("@patronymic", item.Patronymic);
                SqlParameter positionParam = new SqlParameter("@position", item.Position);
                command.Parameters.AddRange(new[] { idParam, firstNameParam, lastNameParam, patronymicParam, positionParam });
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            string sqlExpression =
                "DELETE FROM Employees WHERE Id = @id";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlParameter idParam = new SqlParameter("@id", id);
                command.Parameters.Add(idParam);
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Employee> GetAll()
        {
            string sqlExpression = "SELECT Id, FirstName, LastName, Patronymic, Position FROM Employees";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string firstName = reader.GetString(1);
                            string lastName = reader.GetString(2);
                            string patronymic = reader.GetString(3);
                            string position = reader.GetString(4);

                            yield return new Employee()
                            {
                                Id = id,
                                FirstName = firstName,
                                LastName = lastName,
                                Patronymic = patronymic,
                                Position = position
                            };
                        }
                    }
                }
            }
        }
    }
}
