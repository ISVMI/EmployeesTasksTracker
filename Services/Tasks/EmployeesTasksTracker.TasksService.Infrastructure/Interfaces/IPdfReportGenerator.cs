namespace EmployeesTasksTracker.TasksService.Infrastructure.Interfaces
{
    public interface IPdfReportGenerator
    {
        Task<byte[]> GenerateTaskReportAsync(Guid taskId, CancellationToken cancellationToken = default);
    }
}
