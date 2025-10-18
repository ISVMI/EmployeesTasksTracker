using EmployeesTasksTracker.ProjectsService.Core.Interfaces;
using EmployeesTasksTracker.ProjectsService.Infrastructure.Data;
using EmployeesTasksTracker.ProjectsService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Extensions;

namespace EmployeesTasksTracker.ProjectsService.Infrastructure.Extensions
{
    public static class InfrastructureRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabaseService<ProjectsContext>(configuration);
            services.AddScoped<IProjectsRepo, ProjectsRepo>();
        }
    }
}
