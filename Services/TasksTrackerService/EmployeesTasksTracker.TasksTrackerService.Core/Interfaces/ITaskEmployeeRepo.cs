using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;

namespace EmployeesTasksTracker.TasksTrackerService.Core.Interfaces
{
    public interface ITaskEmployeeRepo
    {
        Task<IEnumerable<TaskEmployee>> GetAllById(Guid? taskId = null, Guid? employeeId = null , CancellationToken token = default);
        System.Threading.Tasks.Task AddEmployeeAsync(Guid employeeId, Guid taskId, RoleInTask roleInTask, CancellationToken token = default);
    }
}
