using Shared.Models;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Interfaces
{
    public interface ITasksGroupReportService
    {
        Task<TasksGroupReportModel> GetTasksGroupReportDataAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
