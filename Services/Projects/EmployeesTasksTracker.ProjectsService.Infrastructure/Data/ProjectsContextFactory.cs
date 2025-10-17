using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EmployeesTasksTracker.ProjectsService.Infrastructure.Data
{
    public class ProjectsContextFactory : IDesignTimeDbContextFactory<ProjectsContext>
    {
        public ProjectsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProjectsContext>();

            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

            return new ProjectsContext(optionsBuilder.Options);
        }
    }
}
