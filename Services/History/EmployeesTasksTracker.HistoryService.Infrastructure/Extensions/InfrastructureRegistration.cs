using EmployeesTasksTracker.HistoryService.Core.Interfaces;
using EmployeesTasksTracker.HistoryService.Infrastructure.Data;
using EmployeesTasksTracker.HistoryService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Extensions;

namespace EmployeesTasksTracker.HistoryService.Infrastructure.Extensions
{
    public static class InfrastructureRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabaseService<TaskChangesContext>(configuration);
            services.AddScoped<ITaskChangesRepo, TaskChangesRepo>();
        }

        public static async Task AddDatabaseInitialization(this IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var serviceProvider = scope.ServiceProvider;

            try
            {
                var context = serviceProvider.GetRequiredService<TaskChangesContext>();

                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> An error occurred while initializing the database : {ex.Message} / {ex.InnerException?.Message}");
            }
        }
    }
}
