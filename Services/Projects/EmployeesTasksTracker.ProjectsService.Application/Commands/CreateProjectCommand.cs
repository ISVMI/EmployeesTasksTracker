using EmployeesTasksTracker.ProjectsService.Application.DTOs;
using MediatR;

namespace EmployeesTasksTracker.ProjectsService.Application.Commands
{
    public record CreateProjectCommand(ProjectDTO Project) : IRequest<Guid>;
}
