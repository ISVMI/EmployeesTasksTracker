using EmployeesTasksTracker.ProjectsService.Application.Interfaces;
using EmployeesTasksTracker.ProjectsService.Infrastructure.Data;

namespace EmployeesTasksTracker.ProjectsService.Infrastructure.DataSeeding
{
    public class DbInitializer
    {
        private readonly IEmployeesClient _employeesClient;

        public DbInitializer(IEmployeesClient employeesClient)
        {
            _employeesClient = employeesClient;
        }

        public async Task InitializeAsync(ProjectsContext context)
        {
            if (context.Projects.Any())
            {
                return;
            }

            var generator = new ProjectsGenerator(_employeesClient);

            var projects = await generator.GenerateProjectsAsync(20);

            if (!projects.Any())
            {
                return;
            }

            await context.Projects.AddRangeAsync(projects);
            await context.SaveChangesAsync();
        }
    }
}
