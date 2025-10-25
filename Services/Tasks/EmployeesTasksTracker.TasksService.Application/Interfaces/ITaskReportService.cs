using Shared.Models;

namespace EmployeesTasksTracker.TasksService.Application.Interfaces
{
    public interface ITaskReportService
    {
        Task<TaskReportData> GetTaskReportDataAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
