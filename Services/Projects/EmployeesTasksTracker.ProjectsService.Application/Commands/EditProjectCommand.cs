using EmployeesTasksTracker.ProjectsService.Application.DTOs;
using MediatR;

namespace EmployeesTasksTracker.ProjectsService.Application.Commands
{
    public record EditProjectCommand(EditProjectDTO ProjectToEdit) : IRequest<ProjectDTO>;
}
