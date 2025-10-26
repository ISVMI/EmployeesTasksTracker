using MediatR;

namespace EmployeesTasksTracker.TasksService.Application.Queries
{
    public record GetProjectIdQuery(Guid TasksGroupId) : IRequest<Guid>;
}
