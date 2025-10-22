using MediatR;

namespace EmployeesTasksTracker.TasksService.Application.Commands
{
    public record AddTaskObserverCommand(Guid ObserverId, Guid TaskId) : IRequest;
}
