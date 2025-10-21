using EmployeesTasksTracker.TasksGroupsService.Application.DTOs;
using MediatR;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Commands
{
    public record EditTaskGroupCommand(EditTaskGroupDTO TaskGRoupToEdit) : IRequest<TaskGroupDto>;
}
