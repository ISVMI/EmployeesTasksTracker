using Shared.Interfaces;

namespace EmployeesTasksTracker.TasksService.Core.Interfaces
{
    public interface ITasksRepo
    {
        Task<Guid> CreateAsync(Models.Task task, CancellationToken token = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken token = default);
        Task<Models.Task> UpdateAsync(Models.Task task, CancellationToken token = default);
        Task<Models.Task> GetByIdAsync(Guid id, CancellationToken token = default);
        Task AddPerformerAsync(Guid performerId, Guid taskId, CancellationToken cancellationToken = default);
        Task AddObserverAsync(Guid observerId, Guid taskId, CancellationToken cancellationToken = default);
        Task ChangeStatusAsync(Guid taskId, string newStatus, CancellationToken cancellationToken = default);
        Task<IEnumerable<Models.Task>> GetAllAsync(Guid? employeeId = null, Guid? tasksGroupId = null, Guid? projectId = null, CancellationToken token = default);
    }
}
