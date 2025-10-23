using EmployeesTasksTracker.TasksGroupsService.Core.Models;
using Shared.Interfaces;

namespace EmployeesTasksTracker.TasksGroupsService.Core.Interfaces
{
    public interface ITasksGroupsRepo : IRepository<TasksGroup>
    {
        Task<IEnumerable<Guid>> GetAllIdsAsync(CancellationToken cancellationToken = default);
    }
}
