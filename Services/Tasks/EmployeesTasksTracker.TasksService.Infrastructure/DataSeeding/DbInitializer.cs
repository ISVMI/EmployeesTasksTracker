using EmployeesTasksTracker.TasksService.Application.Interfaces;
using EmployeesTasksTracker.TasksService.Infrastructure.Data;

namespace EmployeesTasksTracker.TasksService.Infrastructure.DataSeeding
{
    public class DbInitializer
    {
        private readonly IEmployeesClient _employeesClient;
        private readonly ITasksGroupsClient _tasksGroupsClient;
        private readonly IProjectsClient _projectsClient;

        public DbInitializer(IEmployeesClient employeesClient, ITasksGroupsClient tasksGroupsClient, IProjectsClient projectsClient)
        {
            _employeesClient = employeesClient;
            _tasksGroupsClient = tasksGroupsClient;
            _projectsClient = projectsClient;
        }

        public async Task InitializeAsync(TasksContext context)
        {
            if (context.Tasks.Any())
            {
                return;
            }

            var genereator = new TasksGenerator(_employeesClient, _tasksGroupsClient,_projectsClient);

            var tasks = await genereator.GenerateTasksAsync(120);

            if (!tasks.Any())
            {
                return;
            }

            await context.Tasks.AddRangeAsync(tasks);
            await context.SaveChangesAsync();
        }
    }
}
