using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EmployeesTasksTracker.TasksService.Infrastructure.Data
{
    public class TasksContextFactory
    {
        public TasksContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TasksContext>();

            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

            return new TasksContext(optionsBuilder.Options);
        }
    }
}
