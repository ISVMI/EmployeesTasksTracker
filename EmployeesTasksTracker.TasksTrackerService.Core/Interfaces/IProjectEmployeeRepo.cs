using EmployeesTasksTracker.TasksTrackerService.Core.Models;

namespace EmployeesTasksTracker.TasksTrackerService.Core.Interfaces
{
    public interface IProjectEmployeeRepo
    {
        Task<IEnumerable<ProjectEmployee>> GetAllById(Guid? projectId = null, Guid? employeeId = null, CancellationToken token = default);
    }
}
