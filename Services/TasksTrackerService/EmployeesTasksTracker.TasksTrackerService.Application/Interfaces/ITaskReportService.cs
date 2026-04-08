using Shared.Models;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Interfaces
{
    public interface ITaskReportService
    {
        Task<TaskReportModel> GetTaskReportDataAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
