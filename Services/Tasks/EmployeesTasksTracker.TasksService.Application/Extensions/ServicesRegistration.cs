using EmployeesTasksTracker.TasksService.Application.Handlers;
using EmployeesTasksTracker.TasksService.Application.Mapping;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EmployeesTasksTracker.TasksService.Application.Extensions
{
    public static class ServicesRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => cfg.AddProfile<TasksProfile>(), Assembly.GetExecutingAssembly());

            var assemblies = new Assembly[]
            {
                Assembly.GetExecutingAssembly(),
                typeof(CreateTaskHandler).Assembly,
                typeof(EditTaskHandler).Assembly,
                typeof(DeleteTaskHandler).Assembly,
                typeof(GetAllTasksHandler).Assembly,
                typeof(GetTaskByIdHandler).Assembly,
                typeof(ChangeTaskStatusHandler).Assembly,
                typeof(AddTaskPerformerHandler).Assembly,
                typeof(AddTaskObserverHandler).Assembly,
            };

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

            return services;
        }
    }
}
