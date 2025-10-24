using Shared.DTOs;
using Shared.Interfaces;

namespace EmployeesTasksTracker.TasksService.Application.Interfaces
{
    public interface IEmployeesClient : IIdsGetter
    {
        public Task<IEnumerable<EmployeeForReportDTO>> GetEmployeesInfo(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    }
}
