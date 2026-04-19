using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeesTasksTracker.TasksTrackerService.Infrastructure.DataSeeding
{
    public class DbInitializer
    {
        private readonly IEmployeesRepo _employeesRepo;
        private readonly ITasksGroupsRepo _tasksGroupsRepo;
        private readonly IProjectsRepo _projectsRepo;

        public DbInitializer(
            IEmployeesRepo employeesRepo,
            ITasksGroupsRepo tasksGroupsRepo,
            IProjectsRepo projectsRepo)
        {
            _employeesRepo = employeesRepo;
            _tasksGroupsRepo = tasksGroupsRepo;
            _projectsRepo = projectsRepo;
        }

        public async Task InitializeAsync(TasksTrackerContext context)
        {
            var connectionString = context.Database.GetConnectionString();

            if (!context.Employees.Any())
            {
                await EmployeesGenerator.GenerateEmployees(600000, 10000, connectionString);
            }

            if (!context.Projects.Any())
            {
                var generator = new ProjectsGenerator(_employeesRepo);

                await generator.GenerateProjectsAsync(20000, 10000, connectionString);
            }

            if (!context.TasksGroups.Any())
            {
                await TasksGroupsGenerator.GenerateTasksGroupsAsync(40000, 10000, connectionString);
            }

            if (!context.Tasks.Any())
            {
                var genereator = new TasksGenerator(_employeesRepo, _tasksGroupsRepo, _projectsRepo);

                await genereator.GenerateTasksAsync(120000, 10000, connectionString);
            }
        }
    }
}