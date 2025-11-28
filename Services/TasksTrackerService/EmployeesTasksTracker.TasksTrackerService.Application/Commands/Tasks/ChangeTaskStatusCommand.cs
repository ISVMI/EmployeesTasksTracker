using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks
{
    public record ChangeTaskStatusCommand(Guid TaskId, string NewStatus) : IRequest;
}
