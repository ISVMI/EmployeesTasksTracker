using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Commands.TasksGroups
{
    public record DeleteTasksGroupCommand(Guid Id) : IRequest<bool>;
}
