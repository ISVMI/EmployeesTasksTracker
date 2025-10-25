using Shared.DTOs;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Interfaces
{
    public interface ITasksClient
    {
        public Task<IEnumerable<TaskForReportDTO>> GetTasks(Guid tasksGroupId, CancellationToken cancellationToken = default);
        public Task<Guid> GetProjectId(Guid tasksGroupId, CancellationToken cancellationToken = default);
    }
}
