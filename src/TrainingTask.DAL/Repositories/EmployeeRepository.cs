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
    public class EmployeeRepository : BaseRepository, IEmployeeRepository
    {
        public EmployeeRepository(string connectionString) : base(connectionString)
        {
        }

        public Employee Get(int id)
        {
            return base.GetAll(
                $"SELECT TOP 1 Id, FirstName, LastName, Patronymic, Position FROM Employees WHERE Id = {id}",
                record => new Employee()
                {
                    Id = (int)record["Id"],
                    FirstName = (string)record["FirstName"],
                    LastName = (string)record["LastName"],
                    Patronymic = (string)record["Patronymic"],
                    Position = (Position)Enum.Parse(typeof(Position), (string)record["Position"])
                }
                ).FirstOrDefault();
        }

        public int Create(Employee item)
        {
            return base.Create(
                $@"INSERT INTO Employees (FirstName, LastName, Patronymic, Position) 
                   VALUES ({item.FirstName}, {item.LastName}, {item.Patronymic}, {item.Position.ToString()}) 
                   SET @id=SCOPE_IDENTITY()");
        }

        public void Update(Employee item)
        {
            base.Update(
                $@"UPDATE Employees SET 
                    FirstName = {item.FirstName}, LastName = {item.LastName}, Patronymic = {item.Patronymic}, 
                    Position =  {item.Position.ToString()} WHERE Id = {item.Id}");
        }

        public void Delete(int id)
        {
            base.Delete($"DELETE FROM Employees WHERE Id = {id}");
        }

        public IEnumerable<Employee> GetAll()
        {
            return base.GetAll<Employee>(
                "SELECT Id, FirstName, LastName, Patronymic, Position FROM Employees",
                null,
                record => new Employee()
                {
                    Id = (int)record["Id"],
                    FirstName = (string)record["FirstName"],
                    LastName = (string)record["LastName"],
                    Patronymic = (string)record["Patronymic"],
                    Position = (Position)Enum.Parse(typeof(Position), (string)record["Position"])
                });
        }

        public int Count() => base.Count("Employees");

        public Page<Employee> Get(int pageIndex, int limit)
        {
            if (pageIndex <= 0)
            {
                throw new ArgumentException("The page index must be greater than 0");
            }
            if (limit <= 0)
            {
                throw new ArgumentException("The limit must be greater than 0");
            }

            var employees = base.GetAll<Employee>(
                $"SELECT Id, FirstName, LastName, Patronymic, Position FROM Employees ORDER BY Id OFFSET {(pageIndex - 1) * limit} ROWS FETCH NEXT {limit} ROWS ONLY",
                record => new Employee()
                {
                    Id = (int)record["Id"],
                    FirstName = (string)record["FirstName"],
                    LastName = (string)record["LastName"],
                    Patronymic = (string)record["Patronymic"],
                    Position = (Position)Enum.Parse(typeof(Position), (string)record["Position"])
                });

            return new Page<Employee>()
            {
                Items = employees,
                Total = Count()
            };
        }
    }
}

