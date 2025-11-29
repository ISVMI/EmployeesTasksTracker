using EmployeesTasksTracker.TasksTrackerService.Core.Models;

namespace EmployeesTasksTracker.TasksTrackerService.Core.Interfaces
{
    public interface ITaskEmployeeRepo
    {
        Task<IEnumerable<TaskEmployee>> GetAllById(Guid? taskId = null, Guid? employeeId = null , CancellationToken token = default);
    }
}
