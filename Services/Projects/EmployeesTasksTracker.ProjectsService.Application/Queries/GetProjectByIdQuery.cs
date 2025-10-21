using EmployeesTasksTracker.ProjectsService.Application.DTOs;
using MediatR;

namespace EmployeesTasksTracker.ProjectsService.Application.Queries
{
    public record GetProjectByIdQuery(Guid Id) : IRequest<ProjectDTO>;
}
