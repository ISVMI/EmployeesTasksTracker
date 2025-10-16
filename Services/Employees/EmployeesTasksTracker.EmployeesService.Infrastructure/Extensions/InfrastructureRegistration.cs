using EmployeesTasksTracker.EmployeesService.Core.Interfaces;
using EmployeesTasksTracker.EmployeesService.Infrastructure.Data;
using EmployeesTasksTracker.EmployeesService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeesTasksTracker.EmployeesService.Infrastructure.Extensions
{
    public static class InfrastructureRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabaseService<EmployeesContext>(configuration);
            services.AddScoped<IEmployeesRepo, EmployeesRepo>();
        }
    }
}
