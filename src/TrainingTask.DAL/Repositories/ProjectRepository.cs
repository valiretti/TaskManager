using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TrainingTask.Common.Exceptions;
using TrainingTask.Common.Interfaces;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Interfaces;

namespace TrainingTask.DAL.Repositories
{
    public class ProjectRepository : BaseRepository, IProjectRepository
    {
        private readonly string _connectionString;

        public ProjectRepository(string connectionString) : base(connectionString)
        {
            _connectionString = connectionString;
        }

        public Project Get(int id)
        {
            return base.GetAll(
                $"SELECT TOP 1 Id, Name, Abbreviation, Description FROM Projects WHERE Id = {id}",
                record => new Project()
                {
                    Id = record.GetInt32(0),
                    Name = record.GetString(1),
                    Abbreviation = record.GetString(2),
                    Description = record.GetString(3)
                }
            ).FirstOrDefault();
        }

        public int Create(Project item)
        {
            try
            {
                return base.Create(
                    $@"INSERT INTO Projects (Name, Abbreviation, Description) 
                   VALUES ({item.Name}, {item.Abbreviation}, {item.Description}) 
                   SET @id=SCOPE_IDENTITY()");
            }
            catch (SqlException ex) when (ex.Number == 2601)
            {
                throw new UniquenessViolationException($"The project with the name {item.Name} and abbreviation {item.Abbreviation} already exists.", ex);
            }

        }

        public void Update(Project item)
        {
            base.Update(
                $@"UPDATE Projects SET 
                   Name = {item.Name}, Abbreviation = {item.Abbreviation}, Description = {item.Description} WHERE Id = {item.Id}");
        }

        public void Delete(int id)
        {
            base.Delete($"DELETE FROM Projects WHERE Id = {id}");
        }

        public IEnumerable<Project> GetAll()
        {
            return base.GetAll<Project>(
                "SELECT Id, Name, Abbreviation, Description FROM Projects",
                null,
                record => new Project()
                {
                    Id = record.GetInt32(0),
                    Name = record.GetString(1),
                    Abbreviation = record.GetString(2),
                    Description = record.GetString(3)
                });
        }
    }
}

