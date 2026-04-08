using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Infrastructure.Data;

namespace EmployeesTasksTracker.TasksTrackerService.Infrastructure.DataSeeding
{
    public class DbInitializer
    {
        private readonly ITasksRepo _tasksRepo;
        private readonly ITaskEmployeeRepo _taskEmployeeRepo;
        private readonly IEmployeesRepo _employeesRepo;
        private readonly ITasksGroupsRepo _tasksGroupsRepo;
        private readonly IProjectsRepo _projectsRepo;
        private readonly IProjectEmployeeRepo _projectEmployeesRepo;

        public DbInitializer(ITasksRepo tasksRepo,
            ITaskEmployeeRepo taskEmployeeRepo,
            IEmployeesRepo employeesRepo,
            ITasksGroupsRepo tasksGroupsRepo,
            IProjectsRepo projectsRepo,
            IProjectEmployeeRepo projectEmployeeRepo)
        {
            _tasksRepo = tasksRepo;
            _taskEmployeeRepo = taskEmployeeRepo;
            _employeesRepo = employeesRepo;
            _tasksGroupsRepo = tasksGroupsRepo;
            _projectsRepo = projectsRepo;
            _projectEmployeesRepo = projectEmployeeRepo;
        }

        public async Task InitializeAsync(TasksTrackerContext context)
        {

            if (!context.Employees.Any())
            {

                var employees = await EmployeesGenerator.GenerateEmployeesAsync(600);

                if (employees.Count > 0)
                {
                    await context.Employees.AddRangeAsync(employees);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.Projects.Any())
            {

                var generator = new ProjectsGenerator(_projectsRepo, _employeesRepo, _projectEmployeesRepo);

                await generator.GenerateProjectsAsync(20);
                await context.SaveChangesAsync();
            }

            if (!context.TasksGroups.Any())
            {

                var tasksGroups = TasksGroupsGenerator.GenerateTasksGroupsAsync(40);

                if (tasksGroups.Count > 0)
                {
                    await context.TasksGroups.AddRangeAsync(tasksGroups);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.Tasks.Any())
            {

                var genereator = new TasksGenerator(_tasksRepo, _taskEmployeeRepo, _employeesRepo, _tasksGroupsRepo, _projectsRepo);

                await genereator.GenerateTasksAsync(120);

                await context.SaveChangesAsync();
            }
        }
    }
}
