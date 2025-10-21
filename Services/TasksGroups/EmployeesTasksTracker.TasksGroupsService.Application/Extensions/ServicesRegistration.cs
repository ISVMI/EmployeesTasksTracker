using EmployeesTasksTracker.TasksGroupsService.Application.Handlers;
using EmployeesTasksTracker.TasksGroupsService.Application.Mapping;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Extensions
{
    public static class ServicesRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => cfg.AddProfile<TasksGroupsProfile>(), Assembly.GetExecutingAssembly());

            var assemblies = new Assembly[]
            {
                Assembly.GetExecutingAssembly(),
                typeof(CreateTasksGroupHandler).Assembly,
                typeof(EditTasksGroupHandler).Assembly,
                typeof(DeleteTasksGroupHandler).Assembly,
                typeof(GetAllTasksGroupsHandler).Assembly,
                typeof(GetTasksGroupByIdHandler).Assembly,
            };

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

            return services;
        }
    }
}
