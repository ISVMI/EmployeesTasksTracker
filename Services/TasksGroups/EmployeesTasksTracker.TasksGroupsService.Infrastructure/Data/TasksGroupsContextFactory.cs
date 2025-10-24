using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Shared.Extensions;

namespace EmployeesTasksTracker.TasksGroupsService.Infrastructure.Data
{
    public class TasksGroupsContextFactory : IDesignTimeDbContextFactory<TasksGroupsContext>
    {
        public TasksGroupsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = ContextFactoryExtensions.GetOptionsBuilder<TasksGroupsContext>();

            return new TasksGroupsContext(optionsBuilder.Options);
        }
    }
}
