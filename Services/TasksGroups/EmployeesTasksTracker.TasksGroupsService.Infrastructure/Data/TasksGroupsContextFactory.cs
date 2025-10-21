using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EmployeesTasksTracker.TasksGroupsService.Infrastructure.Data
{
    public class TasksGroupsContextFactory : IDesignTimeDbContextFactory<TasksGroupsContext>
    {
        public TasksGroupsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TasksGroupsContext>();

            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

            return new TasksGroupsContext(optionsBuilder.Options);
        }
    }
}
