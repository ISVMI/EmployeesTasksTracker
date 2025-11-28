using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Queries.TasksGroups
{
    public record GetAllTasksGroupsIdsQuery : IRequest<IEnumerable<Guid>>;
}
