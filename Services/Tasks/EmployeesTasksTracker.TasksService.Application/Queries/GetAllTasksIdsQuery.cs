using MediatR;

namespace EmployeesTasksTracker.TasksService.Application.Queries
{
    public record GetAllTasksIdsQuery : IRequest<IEnumerable<Guid>>;
}
