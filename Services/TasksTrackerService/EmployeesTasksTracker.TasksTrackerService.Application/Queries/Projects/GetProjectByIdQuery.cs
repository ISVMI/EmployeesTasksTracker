using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Projects;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Queries.Projects
{
    public record GetProjectByIdQuery(Guid Id) : IRequest<ProjectDTO>;
}
