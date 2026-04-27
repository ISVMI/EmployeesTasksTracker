using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using Shared.Interfaces;

namespace EmployeesTasksTracker.TasksTrackerService.Core.Interfaces
{
    public interface ITasksGroupsRepo : IRepository<TasksGroup>, IIdsGetter
    {
        Task<bool> CheckDeletionCapability(Guid tasksGroupId, CancellationToken cancellationToken = default);
    }
}
