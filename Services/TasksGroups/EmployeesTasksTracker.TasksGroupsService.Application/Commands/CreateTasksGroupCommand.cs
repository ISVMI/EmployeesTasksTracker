using EmployeesTasksTracker.TasksGroupsService.Application.DTOs;
using MediatR;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Commands
{
    public record CreateTasksGroupCommand(TasksGroupDTO TasksGroup) : IRequest<Guid>;
}
