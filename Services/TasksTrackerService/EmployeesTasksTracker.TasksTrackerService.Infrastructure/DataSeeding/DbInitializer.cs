using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Infrastructure.Data;

namespace EmployeesTasksTracker.TasksTrackerService.Infrastructure.DataSeeding
{
    public class DbInitializer
    {
        private readonly ITaskEmployeeRepo _taskEmployeeRepo;
        private readonly IEmployeesRepo _employeesRepo;
        private readonly ITasksGroupsRepo _tasksGroupsRepo;
        private readonly IProjectsRepo _projectsRepo;
        private readonly IProjectEmployeeRepo _projectEmployeesRepo;

        public DbInitializer(ITaskEmployeeRepo taskEmployeeRepo,
            IEmployeesRepo employeesRepo,
            ITasksGroupsRepo tasksGroupsRepo,
            IProjectsRepo projectsRepo,
            IProjectEmployeeRepo projectEmployeeRepo)
        {
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

                var generator = new ProjectsGenerator(_employeesRepo, _projectEmployeesRepo);

                var projects = await generator.GenerateProjectsAsync(20);

                if (projects.Count > 0)
                {
                    await context.Projects.AddRangeAsync(projects);
                    await context.SaveChangesAsync();
                }
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

                var genereator = new TasksGenerator(_taskEmployeeRepo, _employeesRepo, _tasksGroupsRepo, _projectsRepo);

                var tasks = await genereator.GenerateTasksAsync(120);

                if (tasks.Count > 0)
                {
                    await context.Tasks.AddRangeAsync(tasks);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
