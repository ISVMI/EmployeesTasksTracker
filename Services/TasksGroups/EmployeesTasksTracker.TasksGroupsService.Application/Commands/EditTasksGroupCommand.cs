using EmployeesTasksTracker.TasksGroupsService.Application.DTOs;
using MediatR;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Commands
{
    public record EditTasksGroupCommand(EditTasksGroupDTO TasksGroupToEdit) : IRequest<TasksGroupDTO>;
}
