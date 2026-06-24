using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks
{
    public record AddTaskObserverCommand(Guid ObserverId, Guid TaskId) : IRequest;
}
