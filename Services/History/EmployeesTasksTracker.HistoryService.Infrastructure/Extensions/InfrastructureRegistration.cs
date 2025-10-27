using EmployeesTasksTracker.HistoryService.Core.Interfaces;
using EmployeesTasksTracker.HistoryService.Infrastructure.Data;
using EmployeesTasksTracker.HistoryService.Infrastructure.Repositories;
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
        }
}
