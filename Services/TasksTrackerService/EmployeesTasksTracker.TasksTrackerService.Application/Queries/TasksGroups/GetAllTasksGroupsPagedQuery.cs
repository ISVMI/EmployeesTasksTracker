using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.TasksGroups;
using MediatR;
using Shared.DTOs;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Queries.TasksGroups
{
    public record GetAllTasksGroupsPagedQuery(int Page = 1, int PageSize = 20) : IRequest<PagedResponse<TasksGroupDTO>>;
}
