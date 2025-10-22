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

            var employees = await generator.GenerateProjectsAsync(20);

            if (!employees.Any())
            {
                return;
            }

            await context.Projects.AddRangeAsync(employees);
            await context.SaveChangesAsync();
        }
    }
}
