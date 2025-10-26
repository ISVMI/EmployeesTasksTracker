using Shared.Models;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Interfaces
{
    public interface ITasksGroupReportService
    {
        Task<TasksGroupReportModel> GetTasksGroupReportDataAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
