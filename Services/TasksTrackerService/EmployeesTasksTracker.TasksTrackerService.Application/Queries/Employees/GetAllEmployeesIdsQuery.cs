using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Queries.Employees
{
    public record GetAllEmployeesIdsQuery : IRequest<IEnumerable<Guid>>;
}
