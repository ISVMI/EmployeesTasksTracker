using EmployeesTasksTracker.HistoryService.Core.Models;

namespace EmployeesTasksTracker.HistoryService.Core.Interfaces
{
    public interface ITaskChangesRepo
    {
        Task<Guid> CreateTaskChangesRecord(TaskChanges taskChanges, CancellationToken cancellationToken = default);
        Task<IEnumerable<TaskChanges>> GetAllChanges(CancellationToken cancellationToken = default);
        Task<IEnumerable<TaskChanges>> GetChangesByTaskId(Guid taskId, CancellationToken cancellationToken = default);
    }
}
