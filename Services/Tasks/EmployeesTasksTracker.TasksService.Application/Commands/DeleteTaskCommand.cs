using MediatR;

namespace EmployeesTasksTracker.TasksService.Application.Commands
{
    public record DeleteTaskCommand(Guid Id) : IRequest<bool>;
}
