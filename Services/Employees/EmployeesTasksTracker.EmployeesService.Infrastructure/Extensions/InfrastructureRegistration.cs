using EmployeesTasksTracker.EmployeesService.Core.Interfaces;
using EmployeesTasksTracker.EmployeesService.Infrastructure.Data;
using EmployeesTasksTracker.EmployeesService.Infrastructure.DataSeeding;
using EmployeesTasksTracker.EmployeesService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Extensions;

namespace EmployeesTasksTracker.EmployeesService.Infrastructure.Extensions
{
    public static class InfrastructureRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabaseService<EmployeesContext>(configuration);
            services.AddScoped<IEmployeesRepo, EmployeesRepo>();
        }

        public static async Task AddDatabaseInitialization(this IServiceProvider services)
        {
            await services.InitializeDatabaseAsync<EmployeesContext>(DbInitializer.InitializeAsync);
        }
    }
}
