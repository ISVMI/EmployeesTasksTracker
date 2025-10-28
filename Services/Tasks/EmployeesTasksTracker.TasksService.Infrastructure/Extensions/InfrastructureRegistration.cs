using EmployeesTasksTracker.TasksService.Core.Interfaces;
using EmployeesTasksTracker.TasksService.Infrastructure.Data;
using EmployeesTasksTracker.TasksService.Infrastructure.DataSeeding;
using EmployeesTasksTracker.TasksService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Extensions;

namespace EmployeesTasksTracker.TasksService.Infrastructure.Extensions
{
    public static class InfrastructureRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabaseService<TasksContext>(configuration);
            services.AddScoped<ITasksRepo, TasksRepo>();
        }
        public static async Task AddDatabaseInitialization(this IServiceProvider services)
        {

            using var scope = services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            try
            {
                var context = serviceProvider.GetRequiredService<TasksContext>();

                await context.Database.MigrateAsync();

                var initializer = serviceProvider.GetRequiredService<DbInitializer>();

                await initializer.InitializeAsync(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> An error occurred while seeding the database : {ex.Message} / {ex.InnerException?.Message}");
            }
        }
    }
}
