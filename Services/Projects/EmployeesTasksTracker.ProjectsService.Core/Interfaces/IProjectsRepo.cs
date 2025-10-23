using EmployeesTasksTracker.ProjectsService.Core.Models;
using Shared.Interfaces;

namespace EmployeesTasksTracker.ProjectsService.Core.Interfaces
{
    public interface IProjectsRepo : IRepository<Project>
    {
        Task<IEnumerable<Guid>> GetAllIdsAsync(CancellationToken cancellationToken = default);
    }
}
