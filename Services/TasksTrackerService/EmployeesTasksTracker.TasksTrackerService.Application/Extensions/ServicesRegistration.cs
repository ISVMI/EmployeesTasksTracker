using EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Employees;
using EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Projects;
using EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Application.Handlers.TasksGroups;
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
                //CQRS for Employees
                typeof(CreateEmployeeHandler).Assembly,
                typeof(DeleteEmployeeHandler).Assembly,
                typeof(EditEmployeeHandler).Assembly,
                typeof(GetAllEmployeesHandler).Assembly,
                typeof(GetAllEmployeesIdsHandler).Assembly,
                typeof(GetEmployeeByIdHandler).Assembly,
                //CQRS for Projects
                typeof(CreateProjectHandler).Assembly,
                typeof(DeleteProjectHandler).Assembly,
                typeof(EditProjectHandler).Assembly,
                typeof(GetAllProjectsHandler).Assembly,
                typeof(GetAllProjectsIdsHandler).Assembly,
                typeof(GetProjectByIdHandler).Assembly,
                //CQRS for Tasks
                typeof(AddTaskObserverHandler).Assembly,
                typeof(AddTaskPerformerHandler).Assembly,
                typeof(ChangeTaskStatusHandler).Assembly,
                typeof(CreateTaskHandler).Assembly,
                typeof(DeleteTaskHandler).Assembly,
                typeof(EditTaskHandler).Assembly,
                typeof(GetAllTasksHandler).Assembly,
                typeof(GetAllTasksIdsHandler).Assembly,
                typeof(GetProjectIdHandler).Assembly,
                typeof(GetTaskByIdHandler).Assembly,
                typeof(GetTasksByGroupIdHandler).Assembly,
                //CQRS for Tasks groups
                typeof(CreateTasksGroupHandler).Assembly,
                typeof(DeleteTasksGroupHandler).Assembly,
                typeof(EditTasksGroupHandler).Assembly,
                typeof(GetAllTasksGroupsHandler).Assembly,
                typeof(GetAllTasksGroupsIdsHandler).Assembly,
                typeof(GetTasksGroupByIdHandler).Assembly,
            };

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

            return services;
        }
    }
}
