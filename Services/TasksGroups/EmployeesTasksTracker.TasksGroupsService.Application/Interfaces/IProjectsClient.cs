namespace EmployeesTasksTracker.TasksGroupsService.Application.Interfaces
{
    public interface IProjectsClient
    {
        public Task<string> GetProjectName(Guid id, CancellationToken cancellationToken = default);
    }
}
