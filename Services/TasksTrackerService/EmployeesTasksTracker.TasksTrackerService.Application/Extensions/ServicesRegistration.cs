using EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Projects;
using EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Application.Handlers.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Employees;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Extensions
{
    public static class ServicesRegistration
    {

        public static IServiceCollection AddApplication(this IServiceCollection services) 
        {

            var assemblies = new Assembly[]
            {
                Assembly.GetExecutingAssembly(),
                typeof(CreateEmployeeHandler).Assembly,
                typeof(EditEmployeeHandler).Assembly,
                typeof(DeleteEmployeeHandler).Assembly,
                typeof(GetAllEmployeesHandler).Assembly,
                typeof(GetEmployeeByIdHandler).Assembly,
                typeof(CreateProjectHandler).Assembly,
                typeof(EditProjectHandler).Assembly,
                typeof(DeleteProjectHandler).Assembly,
                typeof(GetAllProjectsHandler).Assembly,
                typeof(GetProjectByIdHandler).Assembly,
                typeof(CreateTasksGroupHandler).Assembly,
                typeof(EditTasksGroupHandler).Assembly,
                typeof(DeleteTasksGroupHandler).Assembly,
                typeof(GetAllTasksGroupsHandler).Assembly,
                typeof(GetTasksGroupByIdHandler).Assembly,
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
