using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
                    Id = record.GetInt32(0),
                    FirstName = record.GetString(1),
                    LastName = record.GetString(2),
                    Patronymic = record.GetString(3),
                    Position = record.GetString(4)
                }
                ).FirstOrDefault();
        }

        public int Create(Employee item)
        {
            return base.Create(
                $@"INSERT INTO Employees (FirstName, LastName, Patronymic, Position) 
                   VALUES ({item.FirstName}, {item.LastName}, {item.Patronymic}, {item.Position}) 
                   SET @id=SCOPE_IDENTITY()");
        }

        public void Update(Employee item)
        {
            base.Update(
                $@"UPDATE Employees SET 
                    FirstName = {item.FirstName}, LastName = {item.LastName}, Patronymic = {item.Patronymic}, 
                    Position =  {item.Position} WHERE Id = {item.Id}");
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
                    Id = record.GetInt32(0),
                    FirstName = record.GetString(1),
                    LastName = record.GetString(2),
                    Patronymic = record.GetString(3),
                    Position = record.GetString(4)
                });
        }
    }
}
