using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using Shared.Interfaces;

namespace EmployeesTasksTracker.TasksTrackerService.Core.Interfaces
{
    public interface IProjectsRepo : IRepository<Project>, IIdsGetter
    {
        Task<bool> CheckDeletionCapability(Guid projectId, CancellationToken cancellationToken = default);
    }
}
