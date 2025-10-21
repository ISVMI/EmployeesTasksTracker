using EmployeesTasksTracker.TasksGroupsService.Core.Interfaces;
using EmployeesTasksTracker.TasksGroupsService.Infrastructure.Data;
using EmployeesTasksTracker.TasksGroupsService.Infrastructure.DataSeeding;
using EmployeesTasksTracker.TasksGroupsService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Extensions;

namespace EmployeesTasksTracker.TasksGroupsService.Infrastructure.Extensions
{
    public static class InfrastructureRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabaseService<TasksGroupsContext>(configuration);
            services.AddScoped<ITasksGroupsRepo, TasksGroupsRepo>();
        }

        public static async Task AddDatabaseInitialization(this IServiceProvider services)
        {
            await services.InitializeDatabaseAsync<TasksGroupsContext>(DbInitializer.InitializeAsync);
        }
    }
}
