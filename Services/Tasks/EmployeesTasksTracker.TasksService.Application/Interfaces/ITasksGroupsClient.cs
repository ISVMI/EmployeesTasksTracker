using Shared.DTOs;
using Shared.Interfaces;

namespace EmployeesTasksTracker.TasksService.Application.Interfaces
{
    public interface ITasksGroupsClient : IIdsGetter
    {
        public Task<string> GetTasksGroupName(Guid id, CancellationToken cancellationToken = default);
    }
}
