using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using Shared.Interfaces;

namespace EmployeesTasksTracker.TasksTrackerService.Core.Interfaces
{
    public interface IProjectsRepo : IRepository<Project>, IIdsGetter
    {
        //public Task<Project> GetProjectByTaskId(Guid taskId, CancellationToken token = default);
    }
}
