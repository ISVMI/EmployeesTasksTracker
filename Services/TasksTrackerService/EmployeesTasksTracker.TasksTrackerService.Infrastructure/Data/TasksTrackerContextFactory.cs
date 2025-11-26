using Microsoft.EntityFrameworkCore.Design;
using Shared.Extensions;

namespace EmployeesTasksTracker.TasksTrackerService.Infrastructure.Data
{
    public class TasksTrackerContextFactory : IDesignTimeDbContextFactory<TasksTrackerContext>
    {
        public TasksTrackerContext CreateDbContext(string[] args)
        {
            var optionsBuilder = ContextFactoryExtensions.GetOptionsBuilder<TasksTrackerContext>();

            return new TasksTrackerContext(optionsBuilder.Options);
        }
    }
}
