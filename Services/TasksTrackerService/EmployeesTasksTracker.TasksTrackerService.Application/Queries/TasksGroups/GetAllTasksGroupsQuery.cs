using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.TasksGroups;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Queries.TasksGroups
{
    public record GetAllTasksGroupsQuery : IRequest<IEnumerable<TasksGroupDTO>>;
}
