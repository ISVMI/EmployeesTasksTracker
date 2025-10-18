using EmployeesTasksTracker.ProjectsService.Application.DTOs;
using MediatR;

namespace EmployeesTasksTracker.ProjectsService.Application.Queries
{
    public record GetAllProjectsQuery : IRequest<IEnumerable<ProjectDTO>>;
}
