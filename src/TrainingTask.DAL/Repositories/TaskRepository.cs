using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TrainingTask.Common.Enums;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Interfaces;

namespace TrainingTask.DAL.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly string _connectionString;

        public TaskRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task Get(int id)
        {
            string sqlExpression = "SELECT TOP 1 Id, Name, WorkTime, StartDate, FinishDate, Status, ProjectId FROM Tasks WHERE Id = @id";
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
                            TimeSpan workTime = TimeSpan.FromMinutes(reader.GetInt32(2));
                            DateTime startDate = reader.GetDateTime(3);
                            DateTime finishDate = reader.GetDateTime(4);
                            int status = reader.GetInt32(5);
                            int projectId = reader.GetInt32(6);

                            return new Task
                            {
                                Id = i,
                                Name = name,
                                WorkHours = workTime,
                                StartDate = startDate,
                                FinishDate = finishDate,
                                Status = (Status)status,
                                ProjectId = projectId
                            };
                        }
                    }
                }
            }

            return null;
        }

        public int Create(Task item)
        {
            string sqlExpression =
                "INSERT INTO Tasks (Name, WorkTime, StartDate, FinishDate, Status, ProjectId) VALUES (@name, @workTime, @startDate, @finishDate, @status, @projectId) SET @id=SCOPE_IDENTITY()";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlParameter nameParam = new SqlParameter("@name", item.Name);
                SqlParameter workTimeParam = new SqlParameter("@workTime", item.WorkHours.TotalMinutes);
                SqlParameter startDateParam = new SqlParameter("@startDate", item.StartDate);
                SqlParameter finishDateParam = new SqlParameter("@finishDate", item.FinishDate);
                SqlParameter statusParam = new SqlParameter("@status", (int)item.Status);
                SqlParameter projectIdParam = new SqlParameter("@projectId", item.ProjectId);
                SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.AddRange(new[] { nameParam, workTimeParam, startDateParam, finishDateParam, statusParam, projectIdParam });
                command.Parameters.Add(idParam);
                command.ExecuteNonQuery();

                return (int)idParam.Value;
            }
        }

        public void Update(Task item)
        {
            string sqlExpression =
                "UPDATE Tasks SET Name = @name, WorkTime = @workTime, StartDate = @startDate, FinishDate = @finishDate, Status = @status, ProjectId = @projectId WHERE Id = @id";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlParameter idParam = new SqlParameter("@id", item.Id);
                SqlParameter nameParam = new SqlParameter("@name", item.Name);
                SqlParameter workTimeParam = new SqlParameter("@workTime", item.WorkHours.TotalMinutes);
                SqlParameter startDateParam = new SqlParameter("@startDate", item.StartDate);
                SqlParameter finishDateParam = new SqlParameter("@finishDate", item.FinishDate);
                SqlParameter statusParam = new SqlParameter("@status", (int)item.Status);
                SqlParameter projectIdParam = new SqlParameter("@projectId", item.ProjectId);
                command.Parameters.AddRange(new[] { idParam, nameParam, workTimeParam, startDateParam, finishDateParam, statusParam, projectIdParam });
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            string sqlExpression =
                "DELETE FROM Tasks WHERE Id = @id";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlParameter idParam = new SqlParameter("@id", id);
                command.Parameters.Add(idParam);
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<TaskViewModel> GetAll()
        {
            string sqlExpression =
                "SELECT Tasks.Id, Projects.Abbreviation, Tasks.Name, Tasks.StartDate, Tasks.FinishDate, Employees.FirstName, Employees.LastName, Employees.Patronymic, Tasks.Status FROM Tasks " +
                "JOIN Projects ON Projects.Id = Tasks.ProjectId " +
                "LEFT JOIN EmployeeTasks ON EmployeeTasks.TaskId = Tasks.Id " +
                "LEFT JOIN Employees ON Employees.Id = EmployeeTasks.EmployeeId";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        var tasks = new List<TaskModel>();

                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string abbreviation = reader.GetString(1);
                            string name = reader.GetString(2);
                            DateTime startDate = reader.GetDateTime(3);
                            DateTime finishDate = reader.GetDateTime(4);
                            string firstName = reader.IsDBNull(5) ? String.Empty : reader.GetString(5);
                            string lastName = reader.IsDBNull(6) ? String.Empty : (reader.GetString(6));
                            string patronymic = reader.IsDBNull(7) ? String.Empty : reader.GetString(7);
                            int status = reader.GetInt32(8);

                            var task = new TaskModel
                            {
                                Id = id,
                                ProjectAbbreviation = abbreviation,
                                Name = name,
                                StartDate = startDate,
                                FinishDate = finishDate,
                                FullName = $"{firstName} {lastName} {patronymic}",
                                Status = (Status)status
                            };

                            tasks.Add(task);
                        }


                        var taskGroups = tasks.GroupBy(t => new { t.Id, t.Name, t.Status, t.ProjectAbbreviation, t.FinishDate, t.StartDate })
                            .Select(m => new TaskViewModel
                            {
                                Id = m.Key.Id,
                                ProjectAbbreviation = m.Key.ProjectAbbreviation,
                                Name = m.Key.Name,
                                StartDate = m.Key.StartDate,
                                FinishDate = m.Key.FinishDate,
                                FullNames = m.Where(p => !string.IsNullOrWhiteSpace(p.FullName)).Select(p => p.FullName),
                                Status = m.Key.Status
                            });

                        return taskGroups;
                    }
                }
            }

            return null;
        }

        public TaskViewModel GetViewModel(int i)
        {
            string sqlExpression =
                "SELECT Tasks.Id, Projects.Abbreviation, Tasks.Name, Tasks.StartDate, Tasks.FinishDate, Employees.FirstName, Employees.LastName, Employees.Patronymic, Tasks.Status, Tasks.ProjectId, Employees.Id, Tasks.WorkTime FROM Tasks " +
                "JOIN Projects ON Projects.Id = Tasks.ProjectId " +
                "LEFT JOIN EmployeeTasks ON EmployeeTasks.TaskId = Tasks.Id " +
                "LEFT JOIN Employees ON Employees.Id = EmployeeTasks.EmployeeId " +
                "WHERE Tasks.Id = @id";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlParameter param = new SqlParameter("@id", i);
                command.Parameters.Add(param);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        var tasks = new List<TaskModel>();

                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string abbreviation = reader.GetString(1);
                            string name = reader.GetString(2);
                            DateTime startDate = reader.GetDateTime(3);
                            DateTime finishDate = reader.GetDateTime(4);
                            string firstName = reader.IsDBNull(5) ? String.Empty : reader.GetString(5);
                            string lastName = reader.IsDBNull(6) ? String.Empty : (reader.GetString(6));
                            string patronymic = reader.IsDBNull(7) ? String.Empty : reader.GetString(7);
                            int status = reader.GetInt32(8);
                            int projId = reader.GetInt32(9);
                            int? employeeId = reader.IsDBNull(10) ? default(int?) : reader.GetInt32(10);
                            TimeSpan workTime = TimeSpan.FromMinutes(reader.GetInt32(11));

                            var task = new TaskModel
                            {
                                Id = id,
                                ProjectAbbreviation = abbreviation,
                                Name = name,
                                StartDate = startDate,
                                FinishDate = finishDate,
                                FullName = $"{firstName} {lastName} {patronymic}",
                                Status = (Status)status,
                                ProjectId = projId,
                                EmployeeId = employeeId,
                                WorkHours = workTime
                            };

                            tasks.Add(task);
                        }


                        var taskGroups = tasks.GroupBy(t => new { t.Id, t.Name, t.Status, t.ProjectAbbreviation, t.FinishDate, t.StartDate, t.ProjectId, t.WorkHours })
                            .Select(m => new TaskViewModel
                            {
                                Id = m.Key.Id,
                                ProjectAbbreviation = m.Key.ProjectAbbreviation,
                                Name = m.Key.Name,
                                StartDate = m.Key.StartDate,
                                FinishDate = m.Key.FinishDate,
                                FullNames = m.Where(p => p.EmployeeId != null).Select(p => p.FullName),
                                EmployeeIds = m.Where(p => p.EmployeeId != null).Select(p => p.EmployeeId.Value),
                                Status = m.Key.Status,
                                ProjectId = m.Key.ProjectId,
                                WorkHours = m.Key.WorkHours
                            });

                        return taskGroups.FirstOrDefault();
                    }
                }
            }

            return null;
        }

        private class TaskModel
        {
            public int Id { get; set; }

            public string ProjectAbbreviation { get; set; }

            public string Name { get; set; }

            public DateTime StartDate { get; set; }

            public DateTime FinishDate { get; set; }

            public string FullName { get; set; }

            public Status Status { get; set; }

            public int ProjectId { get; set; }

            public int? EmployeeId { get; set; }

            public TimeSpan WorkHours { get; set; }

        }
    }
}
