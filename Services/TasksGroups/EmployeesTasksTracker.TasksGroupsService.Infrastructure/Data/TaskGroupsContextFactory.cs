using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EmployeesTasksTracker.TasksGroupsService.Infrastructure.Data
{
    public class TaskGroupsContextFactory : IDesignTimeDbContextFactory<TaskGroupsContext>
    {
        public TaskGroupsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TaskGroupsContext>();

            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

            return new TaskGroupsContext(optionsBuilder.Options);
        }
    }
}
