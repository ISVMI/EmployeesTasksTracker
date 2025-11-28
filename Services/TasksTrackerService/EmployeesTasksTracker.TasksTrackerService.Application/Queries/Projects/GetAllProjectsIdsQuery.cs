using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Queries.Projects
{
    public record GetAllProjectsIdsQuery : IRequest<IEnumerable<Guid>>;
}
