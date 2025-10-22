using EmployeesTasksTracker.TasksGroupsService.Infrastructure.Data;

namespace EmployeesTasksTracker.TasksGroupsService.Infrastructure.DataSeeding
{
    public class DbInitializer
    {
        public static async Task InitializeAsync(TasksGroupsContext context)
        {
            if (context.TasksGroups.Any())
            {
                return;
            }

            var tasksGroups = TasksGroupsGenerator.GenerateTasksGroupsAsync(40);

            if (!tasksGroups.Any()) 
            {
                return;
            }

            await context.TasksGroups.AddRangeAsync(tasksGroups);
            await context.SaveChangesAsync();
        }
    }
}
