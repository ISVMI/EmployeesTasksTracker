namespace EmployeesTasksTracker.ProjectsService.Application.Interfaces
{
    public interface IEmployeesClient
    {
        Task<IEnumerable<Guid>?> GetAllEmployeesIds(CancellationToken token = default);
    }
}
