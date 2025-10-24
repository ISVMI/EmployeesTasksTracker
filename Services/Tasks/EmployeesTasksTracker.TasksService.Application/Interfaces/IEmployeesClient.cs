using Shared.DTOs;
using Shared.Interfaces;

namespace EmployeesTasksTracker.TasksService.Application.Interfaces
{
    public interface IEmployeesClient : IIdsGetter
    {
        public Task<EmployeeForReportDTO> GetEmployeeInfo(Guid id, CancellationToken cancellationToken = default);
    }
}
