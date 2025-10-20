using EmployeesTasksTracker.ProjectsService.Core.Interfaces;
using EmployeesTasksTracker.ProjectsService.Infrastructure.Data;
using EmployeesTasksTracker.ProjectsService.Infrastructure.DataSeeding;
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
        public static async Task AddDatabaseInitialization(this IServiceProvider services)
        {
                try
                {
                    var context = services.GetRequiredService<ProjectsContext>();
                    var initializer = services.GetRequiredService<DbInitializer>();

                    await initializer.InitializeAsync(context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while seeding the database : {ex.Message}");
                }
        }
    }
}
