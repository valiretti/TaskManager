using Microsoft.Extensions.DependencyInjection;
using TrainingTask.BLL.Interfaces;
using TrainingTask.BLL.Services;

namespace TrainingTask.BLL
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IProjectService, ProjectService>();
            return services;
        }
    }
}
