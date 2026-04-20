using Shared.Interfaces;

namespace EmployeesTasksTracker.TasksTrackerService.Core.Interfaces
{
    public interface ITasksRepo : IRepository<Models.Task>, IIdsGetter
    {
        Task<IEnumerable<Models.Task>> GetAllFilteredAsync(Guid? employeeId = null, Guid? tasksGroupId = null, Guid? projectId = null,
            CancellationToken token = default);
        Task<IEnumerable<Models.Task>> GetTasksByGroupId(Guid tasksGroupId, CancellationToken cancellationToken = default);
        Task<Guid> GetProjectId(Guid tasksGroupId, CancellationToken cancellationToken = default);
    }
}
