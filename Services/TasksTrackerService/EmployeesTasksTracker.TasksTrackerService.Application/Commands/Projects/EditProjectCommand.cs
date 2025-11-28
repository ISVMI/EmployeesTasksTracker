using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Projects;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Commands.Projects
{
    public record EditProjectCommand(EditProjectDTO ProjectToEdit) : IRequest<ProjectDTO>;
}
