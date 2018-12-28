using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Interfaces;

namespace TrainingTask.DAL.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly string _connectionString;

        public ProjectRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Project Get(int id)
        {
            string sqlExpression = "SELECT TOP 1 Id, Name, Abbreviation, Description FROM Projects WHERE Id = @id";
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
                            string name = reader.GetString(1);
                            string abbreviation = reader.GetString(2);
                            string description = reader.GetString(3);

                            return new Project()
                            {
                                Id = i,
                                Name = name,
                                Abbreviation = abbreviation,
                                Description = description,
                            };
                        }
                    }
                }
            }

            return null;
        }

        public int Create(Project item)
        {
            string sqlExpression =
                "INSERT INTO Projects (Name, Abbreviation, Description) VALUES (@name, @abbreviation, @description) SET @id=SCOPE_IDENTITY()";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlParameter nameParam = new SqlParameter("@name", item.Name);
                SqlParameter abbreviationParam = new SqlParameter("@abbreviation", item.Abbreviation);
                SqlParameter descriptionParam = new SqlParameter("@description", item.Description);
                SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.AddRange(new[] { nameParam, abbreviationParam, descriptionParam });
                command.Parameters.Add(idParam);
                command.ExecuteNonQuery();

                return (int)idParam.Value;
            }
        }

        public void Update(Project item)
        {
            string sqlExpression =
                "UPDATE Projects SET Name = @name, LastName = @lastName, Patronymic = @patronymic, Position =  @position WHERE Id = @id";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlParameter idParam = new SqlParameter("@id", item.Id);
                SqlParameter nameParam = new SqlParameter("@name", item.Name);
                SqlParameter abbreviationParam = new SqlParameter("@abbreviation", item.Abbreviation);
                SqlParameter descriptionParam = new SqlParameter("@description", item.Description);
                command.Parameters.AddRange(new[] { idParam, nameParam, abbreviationParam, descriptionParam });
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            string sqlExpression =
                "DELETE FROM Projects WHERE Id = @id";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlParameter idParam = new SqlParameter("@id", id);
                command.Parameters.Add(idParam);
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Project> GetAll()
        {
            string sqlExpression = "SELECT Id, Name, Abbreviation, Description FROM Projects";
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
                            string name = reader.GetString(1);
                            string abbreviation = reader.GetString(2);
                            string description = reader.GetString(3);

                            yield return new Project()
                            {
                                Id = id,
                                Name = name,
                                Abbreviation = abbreviation,
                                Description = description,
                            };
                        }
                    }
                }
            }
        }
    }
}

