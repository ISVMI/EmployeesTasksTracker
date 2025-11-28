using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks
{
    public record DeleteTaskCommand(Guid Id) : IRequest<bool>;
}
