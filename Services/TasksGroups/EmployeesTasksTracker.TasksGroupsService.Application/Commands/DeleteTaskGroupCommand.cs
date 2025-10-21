using MediatR;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Commands
{
    public record DeleteTaskGroupCommand(Guid Id) : IRequest<bool>;
}
