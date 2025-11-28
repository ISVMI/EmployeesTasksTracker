using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Queries.Tasks
{
    public record GetAllTasksIdsQuery : IRequest<IEnumerable<Guid>>;
}
