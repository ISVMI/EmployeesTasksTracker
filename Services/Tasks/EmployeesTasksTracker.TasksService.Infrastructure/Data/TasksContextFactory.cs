using Microsoft.EntityFrameworkCore.Design;
using Shared.Extensions;

namespace EmployeesTasksTracker.TasksService.Infrastructure.Data
{
    public class TasksContextFactory : IDesignTimeDbContextFactory<TasksContext>
    {
        public TasksContext CreateDbContext(string[] args)
        {
            var optionsBuilder = ContextFactoryExtensions.GetOptionsBuilder<TasksContext>();

            return new TasksContext(optionsBuilder.Options);
        }
    }
}
