using EmployeesTasksTracker.TasksGroupsService.Application.DTOs;
using MediatR;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Queries
{
    public record GetTaskGroupByIdQuery(Guid Id) : IRequest<TaskGroupDto>;
}
