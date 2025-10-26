using Microsoft.EntityFrameworkCore.Design;
using Shared.Extensions;

namespace EmployeesTasksTracker.HistoryService.Infrastructure.Data
{
    public class TaskChangesContextFactory : IDesignTimeDbContextFactory<TaskChangesContext>
    {
        public TaskChangesContext CreateDbContext(string[] args)
        {
            var optionsBuilder = ContextFactoryExtensions.GetOptionsBuilder<TaskChangesContext>();

            return new TaskChangesContext(optionsBuilder.Options);
        }
    }
}
