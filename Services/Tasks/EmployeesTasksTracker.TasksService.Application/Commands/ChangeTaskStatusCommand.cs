using MediatR;

namespace EmployeesTasksTracker.TasksService.Application.Commands
{
    public record ChangeTaskStatusCommand(Guid TaskId, string NewStatus) : IRequest;
}
