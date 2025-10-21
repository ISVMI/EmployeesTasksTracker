using EmployeesTasksTracker.TasksGroupsService.Application.DTOs;
using MediatR;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Queries
{
    public record GetAllTasksGroupsQuery : IRequest<IEnumerable<TasksGroupDTO>>;
}
