using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Projects;
using MediatR;
using Shared.DTOs;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Queries.Projects
{
    public record GetAllProjectsPagedQuery(int Page = 1, int PageSize = 20) : IRequest<PagedResponse<ProjectDTO>>;
}
