using MediatR;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Queries
{
    public record GetAllTasksGroupsIdsQuery : IRequest<IEnumerable<Guid>>;
}
