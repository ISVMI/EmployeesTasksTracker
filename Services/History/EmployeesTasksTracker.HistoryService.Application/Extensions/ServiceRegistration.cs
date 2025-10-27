using EmployeesTasksTracker.HistoryService.Application.Handlers;
using EmployeesTasksTracker.HistoryService.Application.Mapping;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EmployeesTasksTracker.HistoryService.Application.Extensions
{
    public static class ServicesRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => cfg.AddProfile<TaskChangesProfile>(), Assembly.GetExecutingAssembly());

            var assemblies = new Assembly[]
            {
                Assembly.GetExecutingAssembly(),
                typeof(CreateTaskChangesRecordHandler).Assembly,
                typeof(GetAllTasksChangesHandler).Assembly,
                typeof(GetTaskChangesByTaskIdHandler).Assembly
            };

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

            return services;
        }
    }
}
