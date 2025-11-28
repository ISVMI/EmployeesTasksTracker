using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Commands.Projects
{
    public record DeleteProjectCommand(Guid Id) : IRequest<bool>;
}
