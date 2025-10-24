using Microsoft.EntityFrameworkCore.Design;
using Shared.Extensions;

namespace EmployeesTasksTracker.ProjectsService.Infrastructure.Data
{
    public class ProjectsContextFactory : IDesignTimeDbContextFactory<ProjectsContext>
    {
        public ProjectsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = ContextFactoryExtensions.GetOptionsBuilder<ProjectsContext>();

            return new ProjectsContext(optionsBuilder.Options);
        }
    }
}
