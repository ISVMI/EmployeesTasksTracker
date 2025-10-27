using Shared.Models;

namespace EmployeesTasksTracker.TasksService.Application.Interfaces
{
    public interface ITaskReportService
    {
        Task<TaskReportModel> GetTaskReportDataAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
