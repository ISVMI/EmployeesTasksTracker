using EmployeesTasksTracker.TasksGroupsService.Application.DTOs;
using MediatR;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Commands
{
    public record CreateTaskGroupCommand(TaskGroupDto TaskGroup) : IRequest<Guid>;
}
