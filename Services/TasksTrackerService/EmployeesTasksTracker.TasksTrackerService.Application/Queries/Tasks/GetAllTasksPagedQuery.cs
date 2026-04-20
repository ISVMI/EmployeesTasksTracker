using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Tasks;
using MediatR;
using Shared.DTOs;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Queries.Tasks
{
    public record GetAllTasksPagedQuery(int Page = 1, int PageSize = 20) : IRequest<PagedResponse<TaskDTO>>;
}
