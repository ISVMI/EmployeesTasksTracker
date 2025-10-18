using MediatR;

namespace EmployeesTasksTracker.EmployeesService.Application.Queries
{
    public record GetAllEmployeesIdsQuery : IRequest<IEnumerable<Guid>>;
}
