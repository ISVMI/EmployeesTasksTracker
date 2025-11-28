using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Queries.Tasks
{
    public record GetProjectIdQuery(Guid TasksGroupId) : IRequest<Guid>;
}
