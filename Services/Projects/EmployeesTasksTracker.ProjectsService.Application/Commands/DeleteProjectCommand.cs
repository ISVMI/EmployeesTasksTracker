using MediatR;

namespace EmployeesTasksTracker.ProjectsService.Application.Commands
{
    public record DeleteProjectCommand(Guid Id) : IRequest<bool>;
}
