using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;

namespace EmployeesTasksTracker.TasksTrackerService.Core.Interfaces
{
    public interface IProjectEmployeeRepo
    {
        Task<IEnumerable<ProjectEmployee>> GetAllById(Guid? projectId = null, Guid? employeeId = null, CancellationToken token = default);
        System.Threading.Tasks.Task AddEmployeeAsync(Guid employeeId, Guid projectId, RoleInProject roleInProject, CancellationToken token = default);
    }
}
