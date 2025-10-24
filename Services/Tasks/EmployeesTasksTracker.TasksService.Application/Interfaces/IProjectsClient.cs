using Shared.Interfaces;

namespace EmployeesTasksTracker.TasksService.Application.Interfaces
{
    public interface IProjectsClient : IIdsGetter
    {
        public Task<string> GetProjectName(Guid id, CancellationToken cancellationToken = default);
    }
}
