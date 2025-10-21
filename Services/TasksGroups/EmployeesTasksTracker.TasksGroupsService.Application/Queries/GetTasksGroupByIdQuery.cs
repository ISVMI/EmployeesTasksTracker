using EmployeesTasksTracker.TasksGroupsService.Application.DTOs;
using MediatR;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Queries
{
    public record GetTasksGroupByIdQuery(Guid Id) : IRequest<TasksGroupDTO>;
}
