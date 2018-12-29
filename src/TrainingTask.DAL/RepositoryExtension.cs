using Microsoft.Extensions.DependencyInjection;
using TrainingTask.DAL.Interfaces;
using TrainingTask.DAL.Repositories;

namespace TrainingTask.DAL
{
    public static class RepositoryExtension
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services, string connection)
        {
            services.AddScoped<IEmployeeRepository, EmployeeRepository>(provider => new EmployeeRepository(connection));
            services.AddScoped<ITaskRepository, TaskRepository>(provider => new TaskRepository(connection));
            services.AddScoped<IProjectRepository, ProjectRepository>(provider => new ProjectRepository(connection));
            services.AddScoped<IEmployeeTaskRepository, EmployeeTaskRepository>(provider => new EmployeeTaskRepository(connection));
            return services;
        }

    }
}
