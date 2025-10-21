using MediatR;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Commands
{
    public record DeleteTasksGroupCommand(Guid Id) : IRequest<bool>;
}
